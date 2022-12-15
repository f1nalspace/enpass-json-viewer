using DevExpress.Mvvm;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace EnpassJSONViewer.Services
{
    class WinformsSaveFileDialogService : ISaveFileDialogService
    {
        private readonly SaveFileDialog _dlg = new SaveFileDialog();
        private readonly Form _parent;

        public WinformsSaveFileDialogService(Form parent)
        {
            _parent = parent;
        }

        public string DefaultExt { get => _dlg.DefaultExt; set => _dlg.DefaultExt = value; }
        public string DefaultFileName { get => _dlg.FileName; set => _dlg.FileName = value; }
        public bool CheckFileExists { get => _dlg.CheckFileExists; set => _dlg.CheckFileExists = value; }
        public bool AddExtension { get => _dlg.AddExtension; set => _dlg.AddExtension = value; }
        public bool AutoUpgradeEnabled { get => _dlg.AutoUpgradeEnabled; set => _dlg.AutoUpgradeEnabled = value; }
        public bool CheckPathExists { get => _dlg.CheckPathExists; set => _dlg.CheckPathExists = value; }
        public bool DereferenceLinks { get => _dlg.DereferenceLinks; set => _dlg.DereferenceLinks = value; }
        public string InitialDirectory { get => _dlg.InitialDirectory; set => _dlg.InitialDirectory = value; }
        public bool RestoreDirectory { get => _dlg.RestoreDirectory; set => _dlg.RestoreDirectory = value; }
        public bool ShowHelp { get => _dlg.ShowHelp; set => _dlg.ShowHelp = value; }
        public bool SupportMultiDottedExtensions { get => _dlg.SupportMultiDottedExtensions; set => _dlg.SupportMultiDottedExtensions = value; }
        public string Title { get => _dlg.Title; set => _dlg.Title = value; }
        public bool ValidateNames { get => _dlg.ValidateNames; set => _dlg.ValidateNames = value; }
        public string Filter { get => _dlg.Filter; set => _dlg.Filter = value; }
        public int FilterIndex { get => _dlg.FilterIndex; set => _dlg.FilterIndex = value; }

        public IFileInfo File { get; private set; }

        public void Reset()
        {
            File = null;
            _dlg.Reset();
        }

        public bool ShowDialog(Action<CancelEventArgs> fileOK, string directoryName, string fileName)
        {
            File = null;
            _dlg.InitialDirectory = directoryName;
            _dlg.FileName = fileName;
            if (_dlg.ShowDialog(_parent) == DialogResult.OK)
            {
                File = new DialogFileInfo(new FileInfo(_dlg.FileName));
                return true;
            }
            return false;
        }
    }
}
