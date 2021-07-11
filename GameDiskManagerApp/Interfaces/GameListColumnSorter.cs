using GDMLib;
using System;
using System.Collections;
using System.Windows.Forms;

namespace GameDiskManagerApp.Interfaces
{
    public class GameListColumnSorter : IComparer
    {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;

        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;

        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor. Initializes various elements
        /// </summary>
        public GameListColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            //System.Console.WriteLine(ColumnToSort);

            // Compare the two items
            //compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);
            compareResult = CompareGames(GetGameFromListViewItem(listviewX), GetGameFromListViewItem(listviewY), ColumnToSort);

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        private Game GetGameFromListViewItem(ListViewItem item)
        {
            return Data.GetGameByID((int)item.Tag);
        }

        private int CompareGames(Game g1, Game g2, int col)
        {
            
            int result = 0;
            if (col == 1)
            {
                result = ObjectCompare.Compare(g1.Name, g2.Name);
            }
            else if (col == 2)
            {
                throw new NotImplementedException("Add this feature dumbass");
            }
            else if (col == 3)
            {
                throw new NotImplementedException("Add this feature dumbass");
            }
            else if (col == 4)
            {
                throw new NotImplementedException("Add this feature dumbass");
            }
            else if (col == 5)
            {
                throw new NotImplementedException("Add this feature dumbass");
            }
            else if (col == 6)
            {
                throw new NotImplementedException("Add this feature dumbass");
            }
            return result;
        }

        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }
    }
}
