using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NinetyNine
{
    internal class TabControlItemManager
    {
        private List<TabControlItem> tabControlItems;
        private TabControlItem selectedItem;

        public object MessageBox { get; internal set; }

        public TabControlItemManager(List<TabControlItem> tabControlItems)
        {
            this.tabControlItems = tabControlItems;
        }

        internal void SetSelectedItem(TabPage selectedTab)
        {
            selectedItem = GetSelectedItem(selectedTab);
            Resize();
        }

        private TabControlItem GetSelectedItem(TabPage selectedTab)
        {
            foreach (TabControlItem item in tabControlItems)
            {
                if (item.tabPage == selectedTab)
                {
                    return item;
                }
            }

            return null;
        }

        internal void Resize()
        {
            TabPage selectedTabPage = selectedItem.tabPage;
            DataGridView selectedDataGridView = selectedItem.dataGridView;
            selectedDataGridView.Size = selectedTabPage.Size;
        }

        internal TabControlItem GetSelectedItem()
        {
            return selectedItem;
        }
    }
}