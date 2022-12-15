using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Immutable;

namespace EnpassJSONViewer.DataBindings
{
    static class DataBindingManager
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
            ITreeViewBinding<T> Bind(TreeView treeView, Binding binding, Func<T, string> keyFunc, Func<T, string> parentFunc);
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
            IListViewBinding<T> Bind(ListView listView, Binding binding, IEnumerable<ListViewColumnBinding<T>> columns);
        }

        abstract class BaseBinding<TView, TItem> : IDisposable
        {
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

            private readonly LinkedList<MemberItem> _notifyItems = new LinkedList<MemberItem>();
            private readonly Dictionary<NotifyItemKey, LinkedListNode<MemberItem>> _notifyMap = new Dictionary<NotifyItemKey, LinkedListNode<MemberItem>>();

            public TView View { get; }
            public Binding Binding { get; }
            public IEnumerable<TItem> Items { get; protected set; }

            public BaseBinding(TView view, Binding binding)
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

                UpdateNotify();
            }

            private void UpdateNotify()
            {
                foreach (MemberItem notify in _notifyItems)
                {
                    if (notify.PropertyChanged != null)
                        notify.PropertyChanged.PropertyChanged -= OnItemPropertyChanged;
                }

                _notifyItems.Clear();
                _notifyMap.Clear();

                string path = Binding.BindingMemberInfo.BindingMember;

                string[] splittedPath = path.Split(new[] { '.' });

                LinkedListNode<MemberItem> currentNode = null;
                object container = Binding.DataSource;
                for (int i = 0; i < splittedPath.Length; ++i)
                {
                    bool isValue = i == splittedPath.Length - 1;

                    string member = splittedPath[i];

                    INotifyPropertyChanged propertyChanged = container as INotifyPropertyChanged;

                    MemberItem item = new MemberItem(container, member, isValue, propertyChanged);

                    if (_notifyItems.Count == 0)
                        currentNode = _notifyItems.AddFirst(item);
                    else
                        currentNode = _notifyItems.AddAfter(currentNode, item);

                    NotifyItemKey key = new NotifyItemKey(container, member);
                    _notifyMap.Add(key, currentNode);

                    if (propertyChanged != null)
                        propertyChanged.PropertyChanged += OnItemPropertyChanged;

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

            private bool ResolveNode(LinkedListNode<MemberItem> node)
            {
                MemberItem item = node.Value;
                Type t = item.Source?.GetType();
                PropertyInfo prop = t?.GetProperty(item.MemberName);
                object value = prop?.GetValue(item.Source);

                if (item.IsValue)
                {
                    Items = value as IEnumerable<TItem>;
                    UpdateValue(Items);
                    return true;
                }

                if (value == null)
                    return false;

                Debug.Assert(node.Next != null);

                if (!object.ReferenceEquals(node.Next.Value.Source, value))
                    node.Next.Value.Source = value;

                return ResolveNode(node.Next);
            }

            private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                NotifyItemKey key = new NotifyItemKey(sender, e.PropertyName);
                if (_notifyMap.TryGetValue(key, out LinkedListNode<MemberItem> node))
                {
                    if (!ResolveNode(node))
                    {
                        UpdateNotify();
                        Update();
                    }
                }
            }

            public void Update()
            {
                if (_notifyItems.Count == 0)
                    UpdateNotify();

                if (_notifyItems.Count > 0)
                    ResolveNode(_notifyItems.First);
            }

            public void Dispose()
            {
                foreach (MemberItem notify in _notifyItems)
                {
                    if (notify.PropertyChanged != null)
                        notify.PropertyChanged.PropertyChanged -= OnItemPropertyChanged;
                }
                _notifyItems.Clear();
                _notifyMap.Clear();
            }

            protected abstract void UpdateValue(IEnumerable<TItem> items);
        }

        class TreeViewBinding<T> : BaseBinding<TreeView, T>, ITreeViewBinding<T>
        {
            private readonly Func<T, string> _keyFunc;
            private readonly Func<T, string> _parentFunc;

            public TreeViewBinding(TreeView view, Binding binding, Func<T, string> keyFunc, Func<T, string> parentFunc) : base(view, binding)
            {
                if (keyFunc == null)
                    throw new ArgumentNullException(nameof(keyFunc));
                if (parentFunc == null)
                    throw new ArgumentNullException(nameof(parentFunc));
                _keyFunc = keyFunc;
                _parentFunc = parentFunc;
            }

            protected override void UpdateValue(IEnumerable<T> items)
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
                        TreeNode node;
                        if (parentNode == null)
                            node = View.Nodes.Add(key, display);
                        else
                            node = parentNode.Nodes.Add(key, display);
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

            public ITreeViewBinding<T> Bind(TreeView treeView, Binding binding, Func<T, string> keyFunc, Func<T, string> parentFunc)
            {
                if (_bindings.TryGetValue(treeView, out ITreeViewBinding<T> found))
                {
                    found.Dispose();
                    _bindings.Remove(treeView);
                }
                ITreeViewBinding<T> result = new TreeViewBinding<T>(treeView, binding, keyFunc, parentFunc);
                _bindings.Add(treeView, result);
                result.Update();
                return result;
            }
        }

        class ListViewBinding<T> : BaseBinding<ListView, T>, IListViewBinding<T>
        {
            private readonly ImmutableArray<ListViewColumnBinding<T>> _columns;

            public ListViewBinding(ListView view, Binding binding, IEnumerable<ListViewColumnBinding<T>> columns) : base(view, binding)
            {
                _columns = columns.ToImmutableArray();

                view.BeginUpdate();
                view.Columns.Clear();
                foreach (ListViewColumnBinding<T> columnBinding in columns)
                    view.Columns.Add(columnBinding.DisplayName);
                view.EndUpdate();
            }

            protected override void UpdateValue(IEnumerable<T> items)
            {
                View.BeginUpdate();
                View.Items.Clear();
                foreach (T item in items)
                {
                    ListViewItem listItem = new ListViewItem()
                    {
                        ImageIndex = 0,
                        StateImageIndex = 0,
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
                View.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                View.EndUpdate();
            }
        }

        class ListViewBindingManager<T> : IListViewBindingManager<T>
        {
            public static readonly IListViewBindingManager<T> Instance = new ListViewBindingManager<T>();

            private readonly Dictionary<ListView, IListViewBinding<T>> _bindings = new Dictionary<ListView, IListViewBinding<T>>();

            public IListViewBinding<T> Bind(ListView listView, Binding binding, IEnumerable<ListViewColumnBinding<T>> columns)
            {
                if (_bindings.TryGetValue(listView, out IListViewBinding<T> found))
                {
                    found.Dispose();
                    _bindings.Remove(listView);
                }
                IListViewBinding<T> result = new ListViewBinding<T>(listView, binding, columns);
                _bindings.Add(listView, result);
                result.Update();
                return result;
            }
        }

        public static void BindNodes<T>(TreeView treeView, Binding binding, Func<T, string> keyFunc, Func<T, string> parentFunc)
        {
            if (treeView == null)
                throw new ArgumentNullException(nameof(treeView));
            if (binding == null)
                throw new ArgumentNullException(nameof(binding));
            if (binding.DataSource == null)
                throw new ArgumentException($"The binding has no data source set", nameof(binding));
            if (keyFunc == null)
                throw new ArgumentNullException(nameof(keyFunc));
            TreeViewBindingManager<T>.Instance.Bind(treeView, binding, keyFunc, parentFunc);
        }

        public static void Bind<T>(ListView listView, Binding binding, IEnumerable<ListViewColumnBinding<T>> columns)
        {
            if (listView == null)
                throw new ArgumentNullException(nameof(listView));
            if (binding == null)
                throw new ArgumentNullException(nameof(binding));
            if (binding.DataSource == null)
                throw new ArgumentException($"The binding has no data source set", nameof(binding));
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));
            ListViewBindingManager<T>.Instance.Bind(listView, binding, columns);
        }
    }
}
