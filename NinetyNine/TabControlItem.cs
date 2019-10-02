using System.Windows.Forms;

namespace NinetyNine
{
    internal class TabControlItem
    {
        internal TabPage tabPage { get; }
        internal DataGridView dataGridView { get; }

        internal TabControlItem(TabPage tabPage, DataGridView dataGridView)
        {
            this.tabPage = tabPage;
            this.dataGridView = dataGridView;
        }
    }
}