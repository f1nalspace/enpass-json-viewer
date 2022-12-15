using DevExpress.Mvvm;
using EnpassJSONViewer.Models;
using EnpassJSONViewer.Services;
using EnpassJSONViewer.Types;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace EnpassJSONViewer.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        const string AppName = "Enpass JSON Viewer";

        private IOpenFileDialogService OpenDlgService => GetService<IOpenFileDialogService>();
        private IEnpassDatabaseLoader LoaderService => GetService<IEnpassDatabaseLoader>();

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

        public MainViewModel()
        {
            Title = AppName;
            SelectFileCommand = new DelegateCommand(SelectFile);
        }

        private void ActiveFolderChanged(EnpassFolder folder)
        {
            if (Database != null && folder != null)
                ActiveItems = Database.GetItemsForFolder(folder);
            else
                ActiveItems = ImmutableArray<EnpassItem>.Empty;
        }

        private void SelectFile()
        {
            UnloadFile();

            OpenDlgService.Title = "Select JSON File";
            OpenDlgService.Filter = "Enpass JSON File (*.json)|*.json";
            OpenDlgService.Multiselect = false;
            if (OpenDlgService.ShowDialog())
            {
                string filePath = OpenDlgService.GetFullFileName();
                LoadFile(filePath);
            }
        }

        private void UnloadFile()
        {
            ActiveFilePath = null;
            Database = null;
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
            ActiveFolder = null;
            return true;
        }

        private void UpdateTitle(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
                Title = $"{AppName} - {filePath}";
            else
                Title = $"{AppName}";
        }
    }
}
