using DevExpress.Mvvm;
using EnpassJSONViewer.Comparer;
using EnpassJSONViewer.DataBindings;
using EnpassJSONViewer.Models;
using EnpassJSONViewer.Services;
using EnpassJSONViewer.ViewModels;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using static EnpassJSONViewer.DataBindings.DataBindingExtensions;

namespace EnpassJSONViewer
{
    public partial class MainForm : Form
    {
        private readonly MainViewModel _viewModel;

        public MainForm()
        {
            _viewModel = new MainViewModel();

            // Register services
            IServiceContainer serviceContainer = (_viewModel as ISupportServices).ServiceContainer;
            serviceContainer.RegisterService(new WinformsOpenFileDialogService(this));
            serviceContainer.RegisterService(new WinformsSaveFileDialogService(this));
            serviceContainer.RegisterService(new WinformsModalDialogsService(this));

            InitializeComponent();

            DataBindings.Add(new Binding(nameof(Text), _viewModel, nameof(MainViewModel.Title)) { DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged });

            // Data-Bind EnpassFolder´s to TreeView (See DataBindingExtensions class)
            tvCategories.TreeViewNodeSorter = new SimpleTreeNodeComparer();
            tvCategories.Bind<EnpassFolder>(
                new Binding(nameof(TreeView.Nodes), _viewModel, $"{nameof(MainViewModel.Database)}.{nameof(EnpassDatabase.Folders)}"),
                (f) => f.Id.ToString(),
                (f) => (f.Parent?.Id ?? Guid.Empty).ToString(),
                (f) => 0);

            // Data-Bind EnpassItem´s to ListView (See DataBindingExtensions class)
            lvItems.ListViewItemSorter = new SimpleListViewItemComparer();
            lvItems.Bind<EnpassItem>(
                new Binding(nameof(ListView.Items), _viewModel, $"{nameof(MainViewModel.ActiveItems)}"),
                new[] {
                    new ListViewColumnBinding<EnpassItem>("Title", (f) => f.Title),
                    new ListViewColumnBinding<EnpassItem>("Subtitle", (f) => f.Subtitle),
                    new ListViewColumnBinding<EnpassItem>("Username", (f) => f.Username),
                    new ListViewColumnBinding<EnpassItem>("Password", (f) => !string.IsNullOrEmpty(f.Password) ? "yes" : "no"),
                    new ListViewColumnBinding<EnpassItem>("Email", (f) => f.Email),
                    new ListViewColumnBinding<EnpassItem>("Url", (f) => f.Url),
                    new ListViewColumnBinding<EnpassItem>("Updated At", (f) => f.UpdatedAtLocal.ToString(AppConstants.LocalDateFormat)),
                    new ListViewColumnBinding<EnpassItem>("Fields", (f) => f.FieldCount.ToString()),
                    new ListViewColumnBinding<EnpassItem>("Attachments", (f) => f.AttachmentCount.ToString()),
                },
                (f) => 0
            );

            // Show ActiveFolder and ActiveItem in StatusStrip (DataBinding does not work there)
            OnViewModelPropertyChanged(this, new PropertyChangedEventArgs(nameof(MainViewModel.ActiveFolder)));
            OnViewModelPropertyChanged(this, new PropertyChangedEventArgs(nameof(MainViewModel.ActiveItem)));
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (nameof(MainViewModel.ActiveFolder).Equals(e.PropertyName))
                tslblActiveFolder.Text = _viewModel.ActiveFolder?.ToString();
            else if (nameof(MainViewModel.ActiveItem).Equals(e.PropertyName))
                tslblActiveItem.Text = _viewModel.ActiveItem?.ToString();
        }

        private void OnCategoriesAfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            EnpassFolder folder = node.Tag as EnpassFolder;
            _viewModel.ActiveItem = null;
            _viewModel.ActiveFolder = folder;
        }

        private void OnItemsSelectedIndexChanged(object sender, EventArgs e)
        {
            EnpassItem selectedItem = null;
            if (lvItems.SelectedItems.Count == 1)
            {
                ListViewItem listViewItem = lvItems.SelectedItems[0];
                selectedItem = listViewItem.Tag as EnpassItem;
            }
            _viewModel.ActiveItem = selectedItem;
        }

        private void OnSelectFileClick(object sender, EventArgs e) => _viewModel.SelectFileCommand.Execute(null);

        private void OnExitClick(object sender, EventArgs e) => Close();

        private void OnItemsDoubleClick(object sender, EventArgs e)
        {
            if (_viewModel.ShowItemDetailsCommand.CanExecute(_viewModel.ActiveItem))
                _viewModel.ShowItemDetailsCommand.Execute(_viewModel.ActiveItem);
        }
    }
}