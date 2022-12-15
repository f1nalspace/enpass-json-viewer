using System.Collections;
using System.Windows.Forms;

namespace EnpassJSONViewer.Comparer
{
    class SimpleListViewItemComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            ListViewItem itemX = x as ListViewItem;
            ListViewItem itemY = y as ListViewItem;
            int result = itemX.Text.CompareTo(itemY.Text);
            return result;
        }
    }
}
