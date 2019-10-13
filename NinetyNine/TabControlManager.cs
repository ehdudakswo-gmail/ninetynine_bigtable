using System;
using System.Data;
using System.Windows.Forms;

namespace NinetyNine
{
    internal class TabControlManager
    {
        private readonly string ERROR_SHEET_COUNT_WRONG = "엑셀 SHEET {0}개가 필요합니다.";
        private readonly string ERROR_SHEET_NONE = "엑셀 {0} SHEET가 필요합니다.";

        private TabControl tabControl;
        private DataGridViewManager dataGridViewManager = new DataGridViewManager();
        private ExcelDataManager excelDataManager = new ExcelDataManager();
        private BigTableManager bigTableManager = new BigTableManager();

        public TabControlManager(TabControl tabControl)
        {
            this.tabControl = tabControl;
            SetTabPageNames();
            SetDataGridViews();
        }

        private void SetTabPageNames()
        {
            var tabPages = tabControl.TabPages;
            var tableNames = MainTabPageEnum.GetAllDescriptions();
            int cnt = tabPages.Count;

            for (int i = 0; i < cnt; i++)
            {
                tabPages[i].Text = tableNames[i];
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
                        string tableName = tabPage.Text;
                        DataTable dataTable = excelDataManager.GetBasicDataTable(tableName);
                        dataGridViewManager.Add(dataGridView, dataTable);
                    }
                }
            }
        }

        internal void Resize()
        {
            int idx = tabControl.SelectedIndex;
            DataGridView dataGridView = dataGridViewManager.Get(idx);
            TabPage tabPage = tabControl.TabPages[idx];
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
    }
}