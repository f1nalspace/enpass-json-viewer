using DevExpress.Mvvm;
using EnpassJSONViewer.Models;
using EnpassJSONViewer.Services;
using EnpassJSONViewer.Types;
using System.Collections.Immutable;

namespace EnpassJSONViewer.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private IOpenFileDialogService OpenDlgService => GetService<IOpenFileDialogService>();
        private IEnpassDatabaseLoader LoaderService => GetService<IEnpassDatabaseLoader>();
        private IModalDialogsService DialogService => GetService<IModalDialogsService>();
        private IClipboardService ClipboardService => GetService<IClipboardService>();

        public EnpassDatabase Database
        {
            get => GetValue<EnpassDatabase>();
            private set => SetValue(value);
        }

        public EnpassFolder ActiveFolder
        {
            get => GetValue<EnpassFolder>();
            set => SetValue(value, () => ActiveFolderChanged(value));
        }

        public EnpassItem ActiveItem
        {
            get => GetValue<EnpassItem>();
            set => SetValue(value, () => ActiveItemChanged(value));
        }

        public ImmutableArray<EnpassItem> ActiveItems
        {
            get => _activeItems;
            private set => SetValue(ref _activeItems, value);
        }
        private ImmutableArray<EnpassItem> _activeItems = ImmutableArray<EnpassItem>.Empty;

        public string ActiveFilePath
        {
            get => _activeFilePath;
            private set => SetValue(ref _activeFilePath, value, () =>
            {
                UpdateTitle(value);
                if (!string.IsNullOrEmpty(value))
                    LoadFile(value);
                else
                    UnloadFile();
            });
        }
        private string _activeFilePath = null;

        public string Title { get => GetValue<string>(); private set => SetValue(value); }

        public DelegateCommand SelectFileCommand { get; }
        public DelegateCommand<EnpassItem> ShowItemDetailsCommand { get; }
        public DelegateCommand<EnpassItem> CopyUsernameToClipboardCommand { get; }
        public DelegateCommand<EnpassItem> CopyPasswordToClipboardCommand { get; }
        public DelegateCommand<EnpassItem> CopyUrlToClipboardCommand { get; }

        public MainViewModel()
        {
            Title = AppConstants.AppName;
            SelectFileCommand = new DelegateCommand(SelectFile);
            ShowItemDetailsCommand = new DelegateCommand<EnpassItem>(ShowItemDetails, CanShowItemDetails);
            CopyUsernameToClipboardCommand = new DelegateCommand<EnpassItem>(CopyUsernameToClipboard, CanCopyUsernameToClipboard);
            CopyPasswordToClipboardCommand = new DelegateCommand<EnpassItem>(CopyPasswordToClipboard, CanCopyPasswordToClipboard);
            CopyUrlToClipboardCommand = new DelegateCommand<EnpassItem>(CopyUrlToClipboard, CanCopyUrlToClipboard);
        }

        private void ActiveItemChanged(EnpassItem item)
        {
            ShowItemDetailsCommand.RaiseCanExecuteChanged();
            CopyUsernameToClipboardCommand.RaiseCanExecuteChanged();
            CopyPasswordToClipboardCommand.RaiseCanExecuteChanged();
            CopyUrlToClipboardCommand.RaiseCanExecuteChanged();
        }

        private bool CanShowItemDetails(EnpassItem item) => item != null;
        private void ShowItemDetails(EnpassItem item) => DialogService.ShowItemDetails(item);

        private bool CanCopyUsernameToClipboard(EnpassItem item) => item != null && !string.IsNullOrWhiteSpace(item.Username);
        private void CopyUsernameToClipboard(EnpassItem item) => ClipboardService.SetText(item.Username);

        private bool CanCopyPasswordToClipboard(EnpassItem item) => item != null && !string.IsNullOrWhiteSpace(item.Password);
        private void CopyPasswordToClipboard(EnpassItem item) => ClipboardService.SetText(item.Password);

        private bool CanCopyUrlToClipboard(EnpassItem item) => item != null && !string.IsNullOrWhiteSpace(item.Url);
        private void CopyUrlToClipboard(EnpassItem item) => ClipboardService.SetText(item.Url);

        private void ActiveFolderChanged(EnpassFolder folder)
        {
            if (Database != null && folder != null)
                ActiveItems = Database.GetItemsForFolder(folder);
            else
                ActiveItems = ImmutableArray<EnpassItem>.Empty;
        }

        private void SelectFile()
        {
            OpenDlgService.Title = "Select JSON File";
            OpenDlgService.Filter = "Enpass JSON File (*.json)|*.json";
            OpenDlgService.Multiselect = false;
            if (OpenDlgService.ShowDialog())
            {
                string filePath = OpenDlgService.GetFullFileName();
                UnloadFile();
                LoadFile(filePath);
            }
        }

        private void UnloadFile()
        {
            ActiveFilePath = null;
            Database = null;
            ActiveItem = null;
            ActiveFolder = null;
        }

        private bool LoadFile(string filePath)
        {
            Result<EnpassDatabase> res = LoaderService.Load(filePath);
            if (!res.Success)
            {
                return false;
            }

            ActiveFilePath = filePath;
            Database = res.Value;
            ActiveItem = null;
            ActiveFolder = null;
            return true;
        }

        private void UpdateTitle(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
                Title = $"{AppConstants.AppName} - {filePath}";
            else
                Title = $"{AppConstants.AppName}";
        }
    }
}
