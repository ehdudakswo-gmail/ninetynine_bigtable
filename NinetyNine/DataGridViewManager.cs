using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace NinetyNine
{
    internal class DataGridViewManager
    {
        private List<DataGridView> dataGridViews = new List<DataGridView>();
        private DataSet dataSet = new DataSet();

        internal void Add(DataGridView dataGridView)
        {
            SetRowConfig(dataGridView);
            RefreshRowHeaderValue(dataGridView);
            dataGridViews.Add(dataGridView);
        }

        private void SetRowConfig(DataGridView dataGridView)
        {
            dataGridView.RowHeadersWidth = 80;
        }

        internal void RefreshRowHeaderValue(DataGridView dataGridView)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                dataGridView.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        internal DataGridView Get(int idx)
        {
            return dataGridViews[idx];
        }

        internal void Refresh(DataSet dataSet)
        {
            this.dataSet = dataSet;
            RefreshAll();
        }

        private void RefreshAll()
        {
            for (int i = 0; i < dataGridViews.Count; i++)
            {
                DataGridView dataGridView = dataGridViews[i];
                DataTable dataTable = dataSet.Tables[i];
                Refresh(dataGridView, dataTable);
            }
        }

        internal void Refresh(DataGridView dataGridView, DataTable dataTable)
        {
            dataGridView.DataSource = dataTable;
            RefreshRowHeaderValue(dataGridView);
        }

        internal DataSet GetDataSet()
        {
            return dataSet;
        }
    }
}
