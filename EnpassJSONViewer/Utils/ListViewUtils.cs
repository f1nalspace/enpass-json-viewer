using System;
using System.Windows.Forms;

namespace EnpassJSONViewer.Utils
{
    static class ListViewUtils
    {
        public static void AutoSizeColumnList(ListView listView, int minColumnWidth = 50)
        {
            // Prevents flickering
            listView.BeginUpdate();

            // Auto size using header
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            // Grab column size based on header
            int[] sizes = new int[listView.Columns.Count];
            for (int i = 0; i < sizes.Length; i++)
                sizes[i] = listView.Columns[i].Width;

            // Auto size using data
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            // Grab comumn size based on data and set max width
            for (int i = 0; i < sizes.Length; i++)
            {
                ColumnHeader colHeader = listView.Columns[i];
                colHeader.Width = Math.Max(sizes[i], colHeader.Width);
            }

            listView.EndUpdate();
        }
    }
}
