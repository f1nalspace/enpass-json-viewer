using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EnpassJSONViewer.Services
{
    class WinformsOpenFileDialogService : IOpenFileDialogService
    {
        private readonly OpenFileDialog _dlg = new OpenFileDialog();
        private readonly Form _parent;

        public WinformsOpenFileDialogService(Form parent)
        {
            _parent = parent;
        }

        IFileInfo IOpenFileDialogService.File => _file;
        private IFileInfo _file = null;

        public IEnumerable<IFileInfo> Files => _files;
        private ImmutableArray<IFileInfo> _files = ImmutableArray<IFileInfo>.Empty;

        public bool Multiselect { get => _dlg.Multiselect; set => _dlg.Multiselect = value; }
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

        public void Reset()
        {
            _file = null;
            _files = ImmutableArray<IFileInfo>.Empty;
            _dlg.Reset();
        }

        public bool ShowDialog(Action<CancelEventArgs> fileOK, string directoryName)
        {
            _file = null;
            _files = ImmutableArray<IFileInfo>.Empty;
            _dlg.InitialDirectory = directoryName;
            if (_dlg.ShowDialog(_parent) == DialogResult.OK)
            {
                if (_dlg.FileNames.Length > 1)
                {
                    _files = _dlg.FileNames.Select(s => new DialogFileInfo(new FileInfo(s))).ToImmutableArray<IFileInfo>();
                    _file = _files.First();
                }
                else
                {
                    _file = new DialogFileInfo(new FileInfo(_dlg.FileName));
                    _files = new[] { _file }.ToImmutableArray();
                }
                return true;
            }
            return false;
        }
    }
}
