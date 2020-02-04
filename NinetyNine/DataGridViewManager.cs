using NinetyNine.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace NinetyNine
{
    internal class DataGridViewManager
    {
        private const int MIN_IDX = -1;
        private const int MAX_IDX = 987654321;

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

        internal CellIndex GetCellIndex(DataGridViewSelectedCellCollection selectedCells)
        {
            int minRowIdx = MAX_IDX;
            int maxRowIdx = MIN_IDX;
            int minColIdx = MAX_IDX;
            int maxColIdx = MIN_IDX;

            foreach (DataGridViewCell cell in selectedCells)
            {
                int rowIdx = cell.RowIndex;
                int colIdx = cell.ColumnIndex;

                minRowIdx = Math.Min(minRowIdx, rowIdx);
                maxRowIdx = Math.Max(maxRowIdx, rowIdx);
                minColIdx = Math.Min(minColIdx, colIdx);
                maxColIdx = Math.Max(maxColIdx, colIdx);
            }

            CellIndex cellIdx = new CellIndex();
            cellIdx.minRowIdx = minRowIdx;
            cellIdx.maxRowIdx = maxRowIdx;
            cellIdx.minColIdx = minColIdx;
            cellIdx.maxColIdx = maxColIdx;

            return cellIdx;
        }
    }
}
