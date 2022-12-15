using DevExpress.Mvvm;
using EnpassJSONViewer.DataBindings;
using EnpassJSONViewer.Models;
using EnpassJSONViewer.Services;
using EnpassJSONViewer.ViewModels;
using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using static EnpassJSONViewer.DataBindings.DataBindingManager;

namespace EnpassJSONViewer
{
    public partial class MainForm : Form
    {
        private readonly MainViewModel _viewModel;

        class EnpassFolderComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                TreeNode nodeX = x as TreeNode;
                TreeNode nodeY = y as TreeNode;
                int result = nodeX.Text.CompareTo(nodeY.Text);
                return result;
            }
        }

        class EnpassItemComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                ListViewItem itemX = x as ListViewItem;
                ListViewItem itemY = y as ListViewItem;
                int result = itemX.Text.CompareTo(itemY.Text);
                return result;
            }
        }

        public MainForm()
        {
            _viewModel = new MainViewModel();

            IServiceContainer serviceContainer = (_viewModel as ISupportServices).ServiceContainer;

            serviceContainer.RegisterService(new WinformsOpenFileDialogService(this));

            InitializeComponent();

            DataBindings.Add(new Binding(nameof(Text), _viewModel, nameof(MainViewModel.Title)) { DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged });

            tvCategories.TreeViewNodeSorter = new EnpassFolderComparer();

            tvCategories.AfterSelect += OnCategoriesAfterSelect;

            DataBindingManager.BindNodes<EnpassFolder>(
                tvCategories, 
                new Binding(nameof(TreeView.Nodes), _viewModel, $"{nameof(MainViewModel.Database)}.{nameof(EnpassDatabase.Folders)}"),
                (f) => f.Id.ToString(),
                (f) => (f.Parent?.Id ?? Guid.Empty).ToString());

            DataBindingManager.Bind<EnpassItem>(
                lvItems,
                new Binding(nameof(ListView.Items), _viewModel, $"{nameof(MainViewModel.ActiveItems)}"),
                new[] {
                    new ListViewColumnBinding<EnpassItem>("Title", (f) => f.Title),
                    new ListViewColumnBinding<EnpassItem>("Subtitle", (f) => f.Subtitle),
                    new ListViewColumnBinding<EnpassItem>("Username", (f) => f.Username),
                    new ListViewColumnBinding<EnpassItem>("Email", (f) => f.Email),
                    new ListViewColumnBinding<EnpassItem>("Password", (f) => !string.IsNullOrEmpty(f.Password) ? "yes" : "no"),
                    new ListViewColumnBinding<EnpassItem>("Fields", (f) => f.Fields.Count().ToString()),
                }
            );

            lvItems.ListViewItemSorter = new EnpassItemComparer();

            _viewModel.PropertyChanged += OnViewModelPropertyChanged;

            tslblCurrentFolder.Text = _viewModel.ActiveFolder?.ToString();
        }

        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (nameof(MainViewModel.ActiveFolder).Equals(e.PropertyName))
                tslblCurrentFolder.Text = _viewModel.ActiveFolder?.ToString();
        }

        private void OnCategoriesAfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            EnpassFolder folder = node.Tag as EnpassFolder;
            _viewModel.ActiveFolder = folder;
        }

        private void tsbtnLoadFile_Click(object sender, System.EventArgs e)
            => _viewModel.SelectFileCommand.Execute(null);
    }
}