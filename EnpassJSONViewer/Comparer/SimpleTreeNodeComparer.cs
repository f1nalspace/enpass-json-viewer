using System.Collections;
using System.Windows.Forms;

namespace EnpassJSONViewer.Comparer
{
    class SimpleTreeNodeComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode nodeX = x as TreeNode;
            TreeNode nodeY = y as TreeNode;
            int result = nodeX.Text.CompareTo(nodeY.Text);
            return result;
        }
    }
}
