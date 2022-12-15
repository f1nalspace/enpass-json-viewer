using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace EnpassJSONViewer.Models
{
    class EnpassDatabase
    {
        public IEnumerable<EnpassFolder> Folders => _folders;
        private ImmutableList<EnpassFolder> _folders = ImmutableList<EnpassFolder>.Empty;

        public IEnumerable<EnpassItem> Items => _items;
        private ImmutableList<EnpassItem> _items = ImmutableList<EnpassItem>.Empty;

        public EnpassDatabase()
        {

        }

        public void AddFolders(ImmutableArray<EnpassFolder> folders)
        {
            if (folders == null)
                throw new ArgumentNullException(nameof(folders));
            _folders = _folders.AddRange(folders);
        }

        public void AddItems(ImmutableArray<EnpassItem> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            _items = _items.AddRange(items);
        }

        public ImmutableArray<EnpassItem> GetItemsForFolder(EnpassFolder folder)
        {
            if (folder == null)
                return ImmutableArray<EnpassItem>.Empty;
            ImmutableArray<EnpassItem> result = _items
                .Where(i => i.Folder == folder)
                .ToImmutableArray();
            return result;
        }
    }
}
