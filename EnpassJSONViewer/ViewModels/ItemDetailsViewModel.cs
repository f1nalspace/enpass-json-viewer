using DevExpress.Mvvm;
using EnpassJSONViewer.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Text;

namespace EnpassJSONViewer.ViewModels
{
    public class ItemDetailsViewModel : ViewModelBase
    {
        public EnpassItem Item { get; }

        public string Title { get; }

        public ItemDetailsViewModel(EnpassItem item)
        {
            Item = item;
            Title = item.Title;
        }
    }
}
