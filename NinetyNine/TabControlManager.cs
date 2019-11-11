using System;
using System.Data;
using System.Windows.Forms;
using NinetyNine.BigTable;

namespace NinetyNine
{
    internal class TabControlManager
    {
        private readonly string ERROR_SHEET_COUNT_WRONG = "엑셀 SHEET {0}개가 필요합니다.";
        private readonly string ERROR_SHEET_NONE = "엑셀 {0} SHEET가 필요합니다.";

        private TabControl tabControl;
        private DataGridViewManager dataGridViewManager = new DataGridViewManager();
        private int selectedTabIdx = 0;

        public TabControlManager(TabControl tabControl)
        {
            this.tabControl = tabControl;
            SetTabPageTexts();
            SetDataGridViews();
        }

        private void SetTabPageTexts()
        {
            var tabPages = tabControl.TabPages;
            var dataTableNames = MainDataTableEnum.GetAllDescriptions();

            int tabPagesCount = tabPages.Count;
            int dataTableCount = dataTableNames.Count;

            if (tabPagesCount != dataTableCount)
            {
                throw new Exception("TabControlManager SetTabPageTexts Exception");
            }

            for (int i = 0; i < tabPagesCount; i++)
            {
                tabPages[i].Text = dataTableNames[i];
            }
        }

        private void SetDataGridViews()
        {
            foreach (TabPage tabPage in tabControl.TabPages)
            {
                foreach (Control control in tabPage.Controls)
                {
                    if (control is DataGridView)
                    {
                        DataGridView dataGridView = (DataGridView)control;
                        dataGridViewManager.Add(dataGridView);
                    }
                }
            }
        }

        internal void SelectedIndexChanged()
        {
            selectedTabIdx = tabControl.SelectedIndex;
            //RefreshFirstDisplayed();
            RefreshRowHeaderValue();
            Resize();
        }

        private void RefreshFirstDisplayed()
        {
            DataGridView dataGridView = dataGridViewManager.Get(selectedTabIdx);
            dataGridView.ClearSelection();
            dataGridView.FirstDisplayedScrollingRowIndex = 0;
            dataGridView.FirstDisplayedScrollingColumnIndex = 0;
        }

        private void RefreshRowHeaderValue()
        {
            DataGridView dataGridView = dataGridViewManager.Get(selectedTabIdx);
            dataGridViewManager.RefreshRowHeaderValue(dataGridView);
        }

        internal void Resize()
        {
            DataGridView dataGridView = dataGridViewManager.Get(selectedTabIdx);
            TabPage tabPage = tabControl.TabPages[selectedTabIdx];
            dataGridView.Size = tabPage.Size;
        }

        internal void Check(DataSet dataSet)
        {
            int tableCount = dataSet.Tables.Count;
            int tabPageCount = tabControl.TabPages.Count;

            if (tableCount != tabPageCount)
            {
                string errorMessage = string.Format(ERROR_SHEET_COUNT_WRONG, tabPageCount);
                throw new Exception(errorMessage);
            }

            for (int i = 0; i < tabPageCount; i++)
            {
                TabPage tabPage = tabControl.TabPages[i];
                DataTable dataTable = dataSet.Tables[i];

                if (tabPage.Text != dataTable.TableName)
                {
                    string errorMessage = string.Format(ERROR_SHEET_NONE, tabPage.Text);
                    throw new Exception(errorMessage);
                }
            }
        }

        internal void Refresh(DataSet dataSet)
        {
            dataGridViewManager.Refresh(dataSet);
        }

        internal DataSet GetDataSet()
        {
            return dataGridViewManager.GetDataSet();
        }

        internal void HighLight(int tabIdx, BigTableErrorCell[] cells)
        {
            DataGridView dataGridView = dataGridViewManager.Get(tabIdx);
            dataGridView.ClearSelection();

            foreach (BigTableErrorCell cell in cells)
            {
                int rowIdx = cell.rowIdx;
                int colIdx = cell.colIdx;
                dataGridView.Rows[rowIdx].Cells[colIdx].Selected = true;
            }

            BigTableErrorCell firstCell = cells[0];
            int firstRowIdx = firstCell.rowIdx;
            int firstColIdx = firstCell.colIdx;
            dataGridView.FirstDisplayedScrollingRowIndex = firstRowIdx;
            dataGridView.FirstDisplayedScrollingColumnIndex = firstColIdx;
        }
    }
}