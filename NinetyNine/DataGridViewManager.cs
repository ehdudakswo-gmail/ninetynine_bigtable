using System;
using System.Data;
using System.Windows.Forms;

namespace NinetyNine
{
    internal class DataGridViewManager
    {

        internal void Refresh(DataGridView dataGridView, DataTable dataTable)
        {
            dataGridView.DataSource = dataTable;
            SetRowConfig(dataGridView);
            SetRowHeaderValue(dataGridView);
        }

        private void SetRowConfig(DataGridView dataGridView)
        {
            dataGridView.RowHeadersWidth = 80;
        }

        private void SetRowHeaderValue(DataGridView dataGridView)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }
    }
}