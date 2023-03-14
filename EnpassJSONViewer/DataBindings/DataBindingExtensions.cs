using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Immutable;
using EnpassJSONViewer.Utils;
using DevExpress.Mvvm;

namespace EnpassJSONViewer.DataBindings
{
    static class DataBindingExtensions
    {
        interface ITreeViewBinding<T> : IDisposable
        {
            TreeView View { get; }
            Binding Binding { get; }
            IEnumerable<T> Items { get; }
            void Update();
        }

        interface ITreeViewBindingManager<T>
        {
            ITreeViewBinding<T> Bind(TreeView treeView, Binding binding, Func<T, string> keyFunc, Func<T, string> parentFunc, Func<T, int> imageIndexFunc);
        }

        interface IListViewBinding<T> : IDisposable
        {
            ListView View { get; }
            Binding Binding { get; }
            IEnumerable<T> Items { get; }
            void Update();
        }

        public class ListViewColumnBinding<T>
        {
            public string DisplayName { get; }
            public Func<T, string> ValueFunc { get; }

            public ListViewColumnBinding(string displayName, Func<T, string> valueFunc)
            {
                DisplayName = displayName;
                ValueFunc = valueFunc;
            }
        }

        interface IListViewBindingManager<T>
        {
            IListViewBinding<T> Bind(ListView listView, Binding binding, IEnumerable<ListViewColumnBinding<T>> columns, Func<T, int> imageIndexFunc);
        }

        interface ICommandBindings<TControl, TValue> : IDisposable where TControl : class
        {
            TControl Control { get; }
            Binding CommandBinding { get; }
            Binding ParameterBinding { get; }
            void Update();
            bool CanExecute();
            void Execute();
        }

        readonly struct NotifyItemKey : IEquatable<NotifyItemKey>
        {
            public object Source { get; }
            public string PropertyName { get; }

            public NotifyItemKey(object source, string propertyName)
            {
                Source = source;
                PropertyName = propertyName;
            }

            public bool Equals(NotifyItemKey other)
                => object.ReferenceEquals(Source, other.Source) && string.Equals(PropertyName, other.PropertyName);
            public override bool Equals(object obj) => obj is NotifyItemKey keyItem && Equals(keyItem);
            public override int GetHashCode() => HashCode.Combine(Source, PropertyName);
        }

        static class NotifyUtils
        {
            public static void Clear(LinkedList<MemberItem> notifyItems, IDictionary<NotifyItemKey, LinkedListNode<MemberItem>> notifyMap, PropertyChangedEventHandler notifyPropertyChanged)
            {
                foreach (MemberItem notify in notifyItems)
                {
                    if (notify.PropertyChanged != null)
                        notify.PropertyChanged.PropertyChanged -= notifyPropertyChanged;
                }
                notifyItems.Clear();
                notifyMap.Clear();
            }

            public static void UpdateNotify(Binding binding, LinkedList<MemberItem> notifyItems, IDictionary<NotifyItemKey, LinkedListNode<MemberItem>> notifyMap, PropertyChangedEventHandler notifyPropertyChanged)
            {
                Clear(notifyItems, notifyMap, notifyPropertyChanged);

                string path = binding.BindingMemberInfo.BindingMember;

                string[] splittedPath = path.Split(new[] { '.' });

                LinkedListNode<MemberItem> currentNode = null;
                object container = binding.DataSource;
                for (int i = 0; i < splittedPath.Length; ++i)
                {
                    bool isValue = i == splittedPath.Length - 1;

                    string member = splittedPath[i];

                    INotifyPropertyChanged propertyChanged = container as INotifyPropertyChanged;

                    MemberItem item = new MemberItem(container, member, isValue, propertyChanged);

                    if (notifyItems.Count == 0)
                        currentNode = notifyItems.AddFirst(item);
                    else
                        currentNode = notifyItems.AddAfter(currentNode, item);

                    NotifyItemKey key = new NotifyItemKey(container, member);
                    notifyMap.Add(key, currentNode);

                    if (propertyChanged != null)
                        propertyChanged.PropertyChanged += notifyPropertyChanged;

                    if (container != null)
                    {
                        Type t = container.GetType();
                        PropertyInfo prop = t.GetProperty(member);
                        if (prop != null)
                        {
                            object value = prop.GetValue(container);
                            container = value;
                        }
                        else
                            container = null;
                    }
                    else
                        container = null;
                }
            }

            public static bool ResolveNode<T>(LinkedListNode<MemberItem> node, Action<T> updateFunc)
            {
                MemberItem item = node.Value;
                Type t = item.Source?.GetType();
                PropertyInfo prop = t?.GetProperty(item.MemberName);
                object value = prop?.GetValue(item.Source);

                if (item.IsValue)
                {
                    if (value is T actual)
                        updateFunc.Invoke(actual);
                    else
                        updateFunc.Invoke(default);
                    return true;
                }

                if (value == null)
                    return false;

                Debug.Assert(node.Next != null);

                if (!object.ReferenceEquals(node.Next.Value.Source, value))
                    node.Next.Value.Source = value;

                return ResolveNode<T>(node.Next, updateFunc);
            }
        }

        protected class MemberItem
        {
            public object Source { get; internal set; }
            public string MemberName { get; }
            public bool IsValue { get; }
            public INotifyPropertyChanged PropertyChanged { get; }

            public MemberItem(object source, string memberName, bool isValue, INotifyPropertyChanged propertyChanged)
            {
                Source = source;
                MemberName = memberName;
                IsValue = isValue;
                PropertyChanged = propertyChanged;
            }
        }

        abstract class BaseItemsBinding<TView, TItem> : IDisposable
        {
            private readonly LinkedList<MemberItem> _notifyItems = new LinkedList<MemberItem>();
            private readonly Dictionary<NotifyItemKey, LinkedListNode<MemberItem>> _notifyMap = new Dictionary<NotifyItemKey, LinkedListNode<MemberItem>>();

            public TView View { get; }
            public Binding Binding { get; }
            public IEnumerable<TItem> Items { get; protected set; }

            public BaseItemsBinding(TView view, Binding binding)
            {
                if (view == null)
                    throw new ArgumentNullException(nameof(view));
                if (binding == null)
                    throw new ArgumentNullException(nameof(binding));
                if (binding.DataSource == null)
                    throw new ArgumentException("The data source for binding is not set", nameof(binding));

                View = view;
                Binding = binding;
                Items = null;

                NotifyUtils.UpdateNotify(Binding, _notifyItems, _notifyMap, OnItemPropertyChanged);
            }

            private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                NotifyItemKey key = new NotifyItemKey(sender, e.PropertyName);
                if (_notifyMap.TryGetValue(key, out LinkedListNode<MemberItem> node))
                {
                    if (!NotifyUtils.ResolveNode<IEnumerable<TItem>>(node, UpdateItems))
                    {
                        NotifyUtils.UpdateNotify(Binding, _notifyItems, _notifyMap, OnItemPropertyChanged);
                        Update();
                    }
                }
            }

            public void Update()
            {
                if (_notifyItems.Count == 0)
                    NotifyUtils.UpdateNotify(Binding, _notifyItems, _notifyMap, OnItemPropertyChanged);

                if (_notifyItems.Count > 0)
                    NotifyUtils.ResolveNode<IEnumerable<TItem>>(_notifyItems.First, UpdateItems);
            }

            public void Dispose() => NotifyUtils.Clear(_notifyItems, _notifyMap, OnItemPropertyChanged);

            protected abstract void UpdateItems(IEnumerable<TItem> items);
        }

        class TreeViewBinding<T> : BaseItemsBinding<TreeView, T>, ITreeViewBinding<T>
        {
            private readonly Func<T, string> _keyFunc;
            private readonly Func<T, string> _parentFunc;
            private readonly Func<T, int> _imageIndexFunc;

            public TreeViewBinding(TreeView view, Binding binding, Func<T, string> keyFunc, Func<T, string> parentFunc, Func<T, int> imageIndexFunc) : base(view, binding)
            {
                if (keyFunc == null)
                    throw new ArgumentNullException(nameof(keyFunc));
                if (parentFunc == null)
                    throw new ArgumentNullException(nameof(parentFunc));
                _keyFunc = keyFunc;
                _parentFunc = parentFunc;
                _imageIndexFunc = imageIndexFunc;
            }

            protected override void UpdateItems(IEnumerable<T> items)
            {
                View.BeginUpdate();
                View.Nodes.Clear();
                if (items != null)
                {
                    foreach (T item in items)
                    {
                        string display = item.ToString();
                        string key = _keyFunc(item);
                        string parentKey = _parentFunc(item);
                        TreeNode parentNode = null;
                        if (View.Nodes.ContainsKey(parentKey))
                            parentNode = View.Nodes[parentKey];
                        int imageIndex = _imageIndexFunc(item);
                        TreeNode node;
                        if (parentNode == null)
                            node = View.Nodes.Add(key, display, imageIndex);
                        else
                            node = parentNode.Nodes.Add(key, display, imageIndex);
                        node.Tag = item;
                    }
                }
                View.EndUpdate();
            }
        }

        class TreeViewBindingManager<T> : ITreeViewBindingManager<T>
        {
            public static readonly ITreeViewBindingManager<T> Instance = new TreeViewBindingManager<T>();

            private readonly Dictionary<TreeView, ITreeViewBinding<T>> _bindings = new Dictionary<TreeView, ITreeViewBinding<T>>();

            public ITreeViewBinding<T> Bind(TreeView treeView, Binding binding, Func<T, string> keyFunc, Func<T, string> parentFunc, Func<T, int> imageIndexFunc)
            {
                if (_bindings.TryGetValue(treeView, out ITreeViewBinding<T> found))
                {
                    found.Dispose();
                    _bindings.Remove(treeView);
                }
                ITreeViewBinding<T> result = new TreeViewBinding<T>(treeView, binding, keyFunc, parentFunc, imageIndexFunc);
                _bindings.Add(treeView, result);
                result.Update();
                return result;
            }
        }

        class ListViewBinding<T> : BaseItemsBinding<ListView, T>, IListViewBinding<T>
        {
            private readonly ImmutableArray<ListViewColumnBinding<T>> _columns;
            private readonly Func<T, int> _imageIndexFunc;

            public ListViewBinding(ListView view, Binding binding, IEnumerable<ListViewColumnBinding<T>> columns, Func<T, int> imageIndexFunc) : base(view, binding)
            {
                _columns = columns.ToImmutableArray();
                _imageIndexFunc = imageIndexFunc;

                view.BeginUpdate();
                view.Columns.Clear();
                foreach (ListViewColumnBinding<T> columnBinding in columns)
                    view.Columns.Add(columnBinding.DisplayName);
                view.EndUpdate();
            }

            protected override void UpdateItems(IEnumerable<T> items)
            {
                View.BeginUpdate();
                View.Items.Clear();
                foreach (T item in items)
                {
                    int imageIndex = _imageIndexFunc(item);
                    ListViewItem listItem = new ListViewItem()
                    {
                        ImageIndex = imageIndex,
                        StateImageIndex = imageIndex,
                        Tag = item,
                    };
                    int columnIndex = 0;
                    foreach (ListViewColumnBinding<T> column in _columns)
                    {
                        string cellValue = column.ValueFunc(item);
                        if (columnIndex == 0)
                            listItem.Text = cellValue;
                        else
                            listItem.SubItems.Add(cellValue);
                        ++columnIndex;
                    }
                    View.Items.Add(listItem);
                }
                View.EndUpdate();

                ListViewUtils.AutoSizeColumnList(View);
            }
        }

        class ListViewBindingManager<T> : IListViewBindingManager<T>
        {
            public static readonly IListViewBindingManager<T> Instance = new ListViewBindingManager<T>();

            private readonly Dictionary<ListView, IListViewBinding<T>> _bindings = new Dictionary<ListView, IListViewBinding<T>>();

            public IListViewBinding<T> Bind(ListView listView, Binding binding, IEnumerable<ListViewColumnBinding<T>> columns, Func<T, int> imageIndexFunc)
            {
                if (_bindings.TryGetValue(listView, out IListViewBinding<T> found))
                {
                    found.Dispose();
                    _bindings.Remove(listView);
                }
                IListViewBinding<T> result = new ListViewBinding<T>(listView, binding, columns, imageIndexFunc);
                _bindings.Add(listView, result);
                result.Update();
                return result;
            }
        }

        class CommandBindings<TControl, TValue> : ICommandBindings<TControl, TValue> where TControl : class
        {
            private readonly LinkedList<MemberItem> _commandNotifyItems = new LinkedList<MemberItem>();
            private readonly LinkedList<MemberItem> _parameterNotifyItems = new LinkedList<MemberItem>();

            private readonly Dictionary<NotifyItemKey, LinkedListNode<MemberItem>> _commandNotifyMap = new Dictionary<NotifyItemKey, LinkedListNode<MemberItem>>();
            private readonly Dictionary<NotifyItemKey, LinkedListNode<MemberItem>> _parameterNotifyMap = new Dictionary<NotifyItemKey, LinkedListNode<MemberItem>>();
            private readonly Action<TValue, bool> _canExecuteChanged;

            public TControl Control { get; }
            public Binding CommandBinding { get; }
            public Binding ParameterBinding { get; }

            private TValue CurrentParameter { get; set; }
            private ICommand<TValue> CurrentCommand { get; set; }

            public CommandBindings(TControl control, Binding commandBinding, Binding parameterBinding, Action<TValue, bool> canExecuteChanged)
            {
                if (control == null)
                    throw new ArgumentNullException(nameof(control));
                if (commandBinding == null)
                    throw new ArgumentNullException(nameof(commandBinding));
                if (parameterBinding == null)
                    throw new ArgumentNullException(nameof(parameterBinding));
                if (canExecuteChanged == null)
                    throw new ArgumentNullException(nameof(canExecuteChanged));

                _canExecuteChanged = canExecuteChanged;

                Control = control;
                CommandBinding = commandBinding;
                ParameterBinding = parameterBinding;
                CurrentParameter = default;
                CurrentCommand = null;

                NotifyUtils.UpdateNotify(ParameterBinding, _parameterNotifyItems, _parameterNotifyMap, OnParameterPropertyChanged);
                NotifyUtils.UpdateNotify(CommandBinding, _commandNotifyItems, _commandNotifyMap, OnCommandPropertyChanged);
            }

            public void Update()
            {
                ResolveParameter();
                ResolveCommand();
            }

            public bool CanExecute() => CurrentCommand?.CanExecute(CurrentParameter) ?? false;

            public void Execute()
            {
                if (CurrentCommand != null)
                    CurrentCommand.Execute(CurrentParameter);
            }

            private void OnCommandPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                NotifyItemKey key = new NotifyItemKey(sender, e.PropertyName);
                if (_commandNotifyMap.TryGetValue(key, out LinkedListNode<MemberItem> node))
                {
                    if (!NotifyUtils.ResolveNode<ICommand<TValue>>(node, UpdateCommand))
                    {
                        NotifyUtils.UpdateNotify(CommandBinding, _commandNotifyItems, _commandNotifyMap, OnCommandPropertyChanged);
                        ResolveCommand();
                    }
                }
            }

            private void RaiseCanExecuteChanged()
            {
                if (CurrentCommand is IDelegateCommand delegateCommand)
                    delegateCommand.RaiseCanExecuteChanged();
                bool canExecute = CurrentCommand?.CanExecute(CurrentParameter) ?? false;
                _canExecuteChanged.Invoke(CurrentParameter, canExecute);
            }

            private void UpdateCommand(ICommand<TValue> command)
            {
                CurrentCommand = command;
                RaiseCanExecuteChanged();
            }

            private void ResolveCommand()
            {
                if (_commandNotifyItems.Count == 0)
                    NotifyUtils.UpdateNotify(CommandBinding, _commandNotifyItems, _commandNotifyMap, OnCommandPropertyChanged);
                if (_commandNotifyItems.Count > 0)
                    NotifyUtils.ResolveNode<ICommand<TValue>>(_commandNotifyItems.First, UpdateCommand);
            }

            private void OnParameterPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                NotifyItemKey key = new NotifyItemKey(sender, e.PropertyName);
                if (_parameterNotifyMap.TryGetValue(key, out LinkedListNode<MemberItem> node))
                {
                    if (!NotifyUtils.ResolveNode<TValue>(node, UpdateParameter))
                    {
                        NotifyUtils.UpdateNotify(ParameterBinding, _parameterNotifyItems, _parameterNotifyMap, OnParameterPropertyChanged);
                        ResolveParameter();
                    }
                }
            }

            private void UpdateParameter(TValue value)
            {
                CurrentParameter = value;
                RaiseCanExecuteChanged();
            }

            private void ResolveParameter()
            {
                if (_parameterNotifyItems.Count == 0)
                    NotifyUtils.UpdateNotify(ParameterBinding, _parameterNotifyItems, _parameterNotifyMap, OnParameterPropertyChanged);
                if (_parameterNotifyItems.Count > 0)
                    NotifyUtils.ResolveNode<TValue>(_parameterNotifyItems.First, UpdateParameter);
            }

            public void Dispose()
            {
                NotifyUtils.Clear(_commandNotifyItems, _commandNotifyMap, OnCommandPropertyChanged);
                NotifyUtils.Clear(_parameterNotifyItems, _parameterNotifyMap, OnParameterPropertyChanged);
            }
        }

        interface ICommandBindingsManager<TControl, TValue> where TControl : class
        {
            ICommandBindings<TControl, TValue> Bind(TControl control, Binding commandBinding, Binding parameterBinding, Action<TValue, bool> canExecuteChanged);
        }

        class CommandBindingsManager<TControl, TValue> : ICommandBindingsManager<TControl, TValue> where TControl : class
        {
            public static readonly ICommandBindingsManager<TControl, TValue> Instance = new CommandBindingsManager<TControl, TValue>();

            private readonly Dictionary<TControl, ICommandBindings<TControl, TValue>> _bindings = new Dictionary<TControl, ICommandBindings<TControl, TValue>>();

            public ICommandBindings<TControl, TValue> Bind(TControl control, Binding commandBinding, Binding parameterBinding, Action<TValue, bool> canExecuteChanged)
            {
                if (_bindings.TryGetValue(control, out ICommandBindings<TControl, TValue> found))
                {
                    found.Dispose();
                    _bindings.Remove(control);
                }
                ICommandBindings<TControl, TValue> result = new CommandBindings<TControl, TValue>(control, commandBinding, parameterBinding, canExecuteChanged);
                _bindings.Add(control, result);
                result.Update();
                return result;
            }
        }

        public static void Bind<T>(this TreeView treeView, Binding binding, Func<T, string> keyFunc, Func<T, string> parentFunc, Func<T, int> imageIndexFunc)
        {
            if (treeView == null)
                throw new ArgumentNullException(nameof(treeView));
            if (binding == null)
                throw new ArgumentNullException(nameof(binding));
            if (binding.DataSource == null)
                throw new ArgumentException($"The binding has no data source set", nameof(binding));
            if (keyFunc == null)
                throw new ArgumentNullException(nameof(keyFunc));
            TreeViewBindingManager<T>.Instance.Bind(treeView, binding, keyFunc, parentFunc, imageIndexFunc);
        }

        public static void Bind<T>(this ListView listView, Binding binding, IEnumerable<ListViewColumnBinding<T>> columns, Func<T, int> imageIndexFunc)
        {
            if (listView == null)
                throw new ArgumentNullException(nameof(listView));
            if (binding == null)
                throw new ArgumentNullException(nameof(binding));
            if (binding.DataSource == null)
                throw new ArgumentException($"The binding has no data source set", nameof(binding));
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));
            ListViewBindingManager<T>.Instance.Bind(listView, binding, columns, imageIndexFunc);
        }

        public static void BindClickToCommand<T>(this ToolStripItem item, Binding commandBinding, Binding parameterBinding)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (commandBinding == null)
                throw new ArgumentNullException(nameof(commandBinding));
            if (parameterBinding == null)
                throw new ArgumentNullException(nameof(parameterBinding));

            // TODO(final): Cache click command and can-execute-action
            var binding = CommandBindingsManager<ToolStripItem, T>.Instance.Bind(item, commandBinding, parameterBinding, (value, enabled) =>
            {
                item.Enabled = enabled;
            });
            item.Click += (s, e) => binding.Execute();
        }

        public static void BindEventToCommand<T>(this Control control, string eventName, Binding commandBinding, Binding parameterBinding)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));
            if (string.IsNullOrWhiteSpace(eventName))
                throw new ArgumentNullException(nameof(eventName));
            if (commandBinding == null)
                throw new ArgumentNullException(nameof(commandBinding));
            if (parameterBinding == null)
                throw new ArgumentNullException(nameof(parameterBinding));

            // TODO(final): Cache click command and can-execute-action
            var binding = CommandBindingsManager<Control, T>.Instance.Bind(control, commandBinding, parameterBinding, (value, enabled) =>
            {
            });

            switch (eventName)
            {
                case nameof(Control.Click):
                    control.Click += (s, e) => binding.Execute();
                    break;
                case nameof(Control.DoubleClick):
                    control.DoubleClick += (s, e) => binding.Execute();
                    break;
                default:
                    throw new NotSupportedException($"Event '{eventName}' is not supported!");
            }
        }
    }
}
