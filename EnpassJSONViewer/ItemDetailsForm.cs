using EnpassJSONViewer.Comparer;
using EnpassJSONViewer.Models;
using EnpassJSONViewer.ViewModels;
using System;
using System.Windows.Forms;
using static EnpassJSONViewer.DataBindings.DataBindingExtensions;

namespace EnpassJSONViewer
{
    public partial class ItemDetailsForm : Form
    {
        private readonly ItemDetailsViewModel _viewModel;

        public ItemDetailsForm(EnpassItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _viewModel = new ItemDetailsViewModel(item);

            InitializeComponent();

            DataBindings.Add(new Binding(nameof(Text), _viewModel, nameof(ItemDetailsViewModel.Title)));

            lblID.DataBindings.Add(new Binding(nameof(Label.Text), _viewModel.Item, nameof(EnpassItem.Id)));
            lblTitle.DataBindings.Add(new Binding(nameof(Label.Text), _viewModel.Item, nameof(EnpassItem.Title)));
            lblSubtitle.DataBindings.Add(new Binding(nameof(Label.Text), _viewModel.Item, nameof(EnpassItem.Subtitle)));
            lblUpdatedAt.DataBindings.Add(new Binding(nameof(Label.Text), _viewModel.Item, nameof(EnpassItem.UpdatedAtLocal), true, DataSourceUpdateMode.Never, string.Empty, AppConstants.LocalDateFormat));
            linkLabellURL.DataBindings.Add(new Binding(nameof(LinkLabel.Text), _viewModel.Item, nameof(EnpassItem.Url)));

            // Data-Bind EnpassField´s to ListView (See DataBindingExtensions class)
            lvFields.ListViewItemSorter = new SimpleListViewItemComparer();
            lvFields.Bind<EnpassField>(
                new Binding(nameof(ListView.Items), _viewModel.Item, $"{nameof(EnpassItem.Fields)}"),
                new[] {
                    new ListViewColumnBinding<EnpassField>("Label", (f) => f.Label),
                    new ListViewColumnBinding<EnpassField>("Type", (f) => f.Type),
                    new ListViewColumnBinding<EnpassField>("Value", (f) => !f.IsSensitive ? f.Value : (!string.IsNullOrEmpty(f.Value) ? "****" : string.Empty)),
                    new ListViewColumnBinding<EnpassField>("Updated At", (f) => f.UpdatedAtLocal.ToString(AppConstants.LocalDateFormat)),
                    new ListViewColumnBinding<EnpassField>("Is Sensitive", (f) => f.IsSensitive ? "yes" : "no"),
                    new ListViewColumnBinding<EnpassField>("Is Deleted", (f) => f.IsDeleted ? "yes" : "no"),
                }
            );

            rtbNote.DataBindings.Add(new Binding(nameof(RichTextBox.Text), _viewModel.Item, nameof(EnpassItem.Note)));
        }
    }
}
