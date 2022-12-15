﻿using DevExpress.Mvvm;
using EnpassJSONViewer.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Text;
using System;
using EnpassJSONViewer.Services;
using System.Globalization;

namespace EnpassJSONViewer.ViewModels
{
    public class ItemDetailsViewModel : ViewModelBase
    {
        private IClipboardService ClipboardService => GetService<IClipboardService>();

        public EnpassItem Item { get; }

        public string Title { get; }

        public EnpassField SelectedField
        {
            get => GetValue<EnpassField>();
            set => SetValue(value, () => SelectedFieldChanged(value));
        }

        public DelegateCommand<EnpassField> CopyFieldToClipboardCommand { get; }
        public DelegateCommand<EnpassField> CopyFieldNameValueToClipboardCommand { get; }

        public ItemDetailsViewModel(EnpassItem item)
        {
            Item = item;
            Title = item.Title;

            CopyFieldToClipboardCommand = new DelegateCommand<EnpassField>(CopyFieldToClipboard, CanCopyFieldToClipboard);
            CopyFieldNameValueToClipboardCommand = new DelegateCommand<EnpassField>(CopyFieldNameValueToClipboard, CanCopyFieldNameValueToClipboard);

            SelectedField = null;
        }

        private void SelectedFieldChanged(EnpassField field)
        {
            CopyFieldToClipboardCommand.RaiseCanExecuteChanged();
            CopyFieldNameValueToClipboardCommand.RaiseCanExecuteChanged();
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
            s.Append(", ");
            string text = s.ToString();
            ClipboardService.SetText(text);
        }

        private bool CanCopyFieldNameValueToClipboard(EnpassField field) => field != null;
        private void CopyFieldNameValueToClipboard(EnpassField field)
        {
            string text = $"{field.Label} => {field.Value}";
            ClipboardService.SetText(text);
        }
    }
}
