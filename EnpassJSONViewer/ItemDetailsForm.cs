using DevExpress.Mvvm;
using EnpassJSONViewer.Comparer;
using EnpassJSONViewer.Models;
using EnpassJSONViewer.Services;
using EnpassJSONViewer.Utils;
using EnpassJSONViewer.ViewModels;
using System;
using System.Linq;
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

            // Register services
            IServiceContainer serviceContainer = (_viewModel as ISupportServices).ServiceContainer;
            serviceContainer.RegisterService(new WinformsSaveFileDialogService(this));

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
                },
                (f) => 0
            );

            // Data-Bind EnpassAttachment´s to ListView (See DataBindingExtensions class)
            lvAttachments.ListViewItemSorter = new SimpleListViewItemComparer();
            lvAttachments.Bind<EnpassAttachment>(
                new Binding(nameof(ListView.Items), _viewModel.Item, $"{nameof(EnpassItem.Attachments)}"),
                new[] {
                    new ListViewColumnBinding<EnpassAttachment>("Name", (f) => f.Name),
                    new ListViewColumnBinding<EnpassAttachment>("Kind", (f) => f.Kind),
                    new ListViewColumnBinding<EnpassAttachment>("Data-Size", (f) => ByteUtils.FormatSize((ulong)f.Data.Length)),
                    new ListViewColumnBinding<EnpassAttachment>("Updated", (f) => f.UpdatedLocal.ToString(AppConstants.LocalDateFormat)),
                },
                (f) => f.Kind.StartsWith("image/", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0
            );

            rtbNote.DataBindings.Add(new Binding(nameof(RichTextBox.Text), _viewModel.Item, nameof(EnpassItem.Note)));

            tsmiFieldCopyToClipboard.BindClickToCommand<EnpassField>(
                new Binding("Command", _viewModel, nameof(ItemDetailsViewModel.CopyFieldToClipboardCommand)),
                new Binding("Parameter", _viewModel, nameof(ItemDetailsViewModel.SelectedField))
            );
            tsmiFieldCopyNameValueToClipboard.BindClickToCommand<EnpassField>(
                new Binding("Command", _viewModel, nameof(ItemDetailsViewModel.CopyFieldNameValueToClipboardCommand)),
                new Binding("Parameter", _viewModel, nameof(ItemDetailsViewModel.SelectedField))
            );
            tsmiSaveAttachment.BindClickToCommand<EnpassAttachment>(
                new Binding("Command", _viewModel, nameof(ItemDetailsViewModel.SaveAttachmentCommand)),
                new Binding("Parameter", _viewModel, nameof(ItemDetailsViewModel.SelectedAttachment))
            );
        }

        private void OnFieldsSelectedIndexChanged(object sender, EventArgs e)
        {
            EnpassField selectedField = null;
            if (lvFields.SelectedItems.Count == 1)
            {
                ListViewItem listViewItem = lvFields.SelectedItems[0];
                selectedField = listViewItem.Tag as EnpassField;
            }
            _viewModel.SelectedField = selectedField;
        }

        private void OnAttachmentsSelectedIndexChanged(object sender, EventArgs e)
        {
            EnpassAttachment selectedAttachment = null;
            if (lvAttachments.SelectedItems.Count == 1)
            {
                ListViewItem listViewItem = lvAttachments.SelectedItems[0];
                selectedAttachment = listViewItem.Tag as EnpassAttachment;
            }
            _viewModel.SelectedAttachment = selectedAttachment;
        }
    }
}
