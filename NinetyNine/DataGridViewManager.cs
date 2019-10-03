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

        internal void Add(DataGridView dataGridView, DataTable dataTable)
        {
            dataGridView.DataSource = dataTable;
            SetRowConfig(dataGridView);
            SetRowHeaderValue(dataGridView);

            dataGridViews.Add(dataGridView);
            dataSet.Tables.Add(dataTable);
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

        private void Refresh(DataGridView dataGridView, DataTable dataTable)
        {
            dataGridView.DataSource = dataTable;
            SetRowHeaderValue(dataGridView);
        }

        internal DataSet GetDataSet()
        {
            return dataSet;
        }

        internal DataTable GetDataTable(string tableName)
        {
            foreach (DataTable dataTable in dataSet.Tables)
            {
                if (dataTable.TableName == tableName)
                {
                    return dataTable;
                }
            }

            return null;
        }
    }
}
