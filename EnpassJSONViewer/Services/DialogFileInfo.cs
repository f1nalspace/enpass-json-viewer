using DevExpress.Mvvm;
using System;
using System.IO;

namespace EnpassJSONViewer.Services
{
    class DialogFileInfo : IFileInfo
    {
        private readonly FileInfo _f;

        public DialogFileInfo(FileInfo file)
        {
            _f = file;
        }

        public long Length => _f.Length;
        public string DirectoryName => _f.DirectoryName;
        public string Name => _f.Name;
        public bool Exists => _f.Exists;
        public FileAttributes Attributes { get => _f.Attributes; set => throw new NotImplementedException(); }

        public StreamWriter AppendText() => _f.AppendText();
        public FileInfo CopyTo(string destinationFileName, bool overwrite) => _f.CopyTo(destinationFileName, overwrite);
        public FileStream Create() => _f.Create();
        public StreamWriter CreateText() => _f.CreateText();
        public void Delete() => _f.Delete();
        public void MoveTo(string destinationFileName) => _f.MoveTo(destinationFileName);
        public FileStream Open(FileMode mode, FileAccess access, FileShare share) => _f.Open(mode, access, share);
        public FileStream OpenRead() => _f.OpenRead();
        public StreamReader OpenText() => _f.OpenText();
        public FileStream OpenWrite() => _f.OpenWrite();

        public override string ToString() => _f.FullName;
    }
}
