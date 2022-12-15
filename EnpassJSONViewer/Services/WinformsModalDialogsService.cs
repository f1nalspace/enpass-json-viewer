using EnpassJSONViewer.Models;
using EnpassJSONViewer.ViewModels;
using System;
using System.Windows.Forms;

namespace EnpassJSONViewer.Services
{
    class WinformsModalDialogsService : IModalDialogsService
    {
        private readonly Form _parent;

        public WinformsModalDialogsService(Form parent)
        {
            _parent = parent;
        }

        public void ShowItemDetails(EnpassItem item)
        {
            ItemDetailsForm form = new ItemDetailsForm(item);
            form.ShowDialog(_parent);
        }
    }
}
