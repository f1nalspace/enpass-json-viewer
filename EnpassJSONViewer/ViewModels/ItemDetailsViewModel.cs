using DevExpress.Mvvm;
using EnpassJSONViewer.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Text;
using System;
using EnpassJSONViewer.Services;
using System.Globalization;
using System.IO;

namespace EnpassJSONViewer.ViewModels
{
    public class ItemDetailsViewModel : ViewModelBase
    {
        private IClipboardService ClipboardService => GetService<IClipboardService>();
        private ISaveFileDialogService SaveDialogService => GetService<ISaveFileDialogService>();

        public EnpassItem Item { get; }

        public string Title { get; }

        public EnpassField SelectedField
        {
            get => GetValue<EnpassField>();
            set => SetValue(value, () => SelectedFieldChanged(value));
        }

        public EnpassAttachment SelectedAttachment
        {
            get => GetValue<EnpassAttachment>();
            set => SetValue(value, () => SelectedAttachmentChanged(value));
        }

        public DelegateCommand<EnpassField> CopyFieldToClipboardCommand { get; }
        public DelegateCommand<EnpassField> CopyFieldNameValueToClipboardCommand { get; }
        public DelegateCommand<EnpassField> CopyFieldValueToClipboardCommand { get; }
        public DelegateCommand<EnpassAttachment> SaveAttachmentCommand { get; }

        public ItemDetailsViewModel(EnpassItem item)
        {
            Item = item;
            Title = item.Title;

            CopyFieldToClipboardCommand = new DelegateCommand<EnpassField>(CopyFieldToClipboard, CanCopyFieldToClipboard);
            CopyFieldNameValueToClipboardCommand = new DelegateCommand<EnpassField>(CopyFieldNameValueToClipboard, CanCopyFieldNameValueToClipboard);
            CopyFieldValueToClipboardCommand = new DelegateCommand<EnpassField>(CopyFieldValueToClipboard, CanCopyFieldValueToClipboard);
            SaveAttachmentCommand = new DelegateCommand<EnpassAttachment>(SaveAttachment, CanSaveAttachment);

            SelectedField = null;
            SelectedAttachment = null;
        }

        private void SelectedAttachmentChanged(EnpassAttachment attachment)
        {
            SaveAttachmentCommand.RaiseCanExecuteChanged();
        }

        private bool CanSaveAttachment(EnpassAttachment attachment) => attachment != null;
        private void SaveAttachment(EnpassAttachment attachment)
        {
            string fileExt = Path.GetExtension(attachment.Name);
            string fileName = attachment.Name;
            SaveDialogService.Filter = $"{attachment.Kind} (*{fileExt})|*{fileExt}";
            SaveDialogService.DefaultExt = fileExt;
            SaveDialogService.Title = $"Save Attachment '{attachment.Name}'";
            if (SaveDialogService.ShowDialog((e) => { }, directoryName: null, fileName: fileName))
            {
                string targetFilePath = SaveDialogService.GetFullFileName();
                using (var stream = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write))
                    stream.Write(attachment.Data.AsSpan());
            }
        }

        private void SelectedFieldChanged(EnpassField field)
        {
            CopyFieldToClipboardCommand.RaiseCanExecuteChanged();
            CopyFieldNameValueToClipboardCommand.RaiseCanExecuteChanged();
            CopyFieldValueToClipboardCommand.RaiseCanExecuteChanged();
        }

        private bool CanCopyFieldToClipboard(EnpassField field) => field != null;
        private void CopyFieldToClipboard(EnpassField field)
        {
            StringBuilder s = new StringBuilder();
            s.Append("Label: ");
            s.Append('\'');
            s.Append(field.Label);
            s.Append('\'');
            s.Append(", ");
            s.Append("Value: ");
            s.Append('\'');
            s.Append(field.Value);
            s.Append('\'');
            s.Append(", ");
            s.Append("Type: ");
            s.Append('\'');
            s.Append(field.Type);
            s.Append('\'');
            s.Append(", ");
            s.Append("Updated At (UTC): ");
            s.Append('\'');
            s.Append(field.UpdatedAtUTC.ToString(CultureInfo.InvariantCulture));
            s.Append('\'');
            string text = s.ToString();
            ClipboardService.SetText(text);
        }

        private bool CanCopyFieldNameValueToClipboard(EnpassField field) => field != null;
        private void CopyFieldNameValueToClipboard(EnpassField field)
        {
            string text = $"{field.Label} => {field.Value}";
            ClipboardService.SetText(text);
        }

        private bool CanCopyFieldValueToClipboard(EnpassField field) => field != null;
        private void CopyFieldValueToClipboard(EnpassField field)
        {
            ClipboardService.SetText(field.Value ?? string.Empty);
        }
    }
}
