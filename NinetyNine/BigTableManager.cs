using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine
{
    internal class BigTableManager
    {
        private readonly int titleLen;
        private readonly string ERROR_EMPTY_SHEET = "{0} SHEET 내용이 필요합니다.";

        private ExcelDataManager excelDataManager = new ExcelDataManager();
        private BigTableTitleManager bigTableTitleManager = new BigTableTitleManager();
        private DataSet dataSet;
        private DataTable bigTable;

        internal BigTableManager()
        {
            titleLen = bigTableTitleManager.GetLength();
        }

        internal void Set(DataSet dataSet, DataTable bigTable)
        {
            this.dataSet = dataSet;
            this.bigTable = bigTable;
        }

        internal void Check()
        {
            foreach (DataTable dataTable in dataSet.Tables)
            {
                if (dataTable == bigTable)
                {
                    continue;
                }

                if (isEmpty(dataTable))
                {
                    string tableName = dataTable.TableName;
                    string errorMessage = string.Format(ERROR_EMPTY_SHEET, tableName);
                    throw new Exception(errorMessage);
                }
            }
        }

        private bool isEmpty(DataTable dataTable)
        {
            return (dataTable.Rows.Count == 0);
        }

        internal void Refresh()
        {
            bigTable.Clear();
            SetBigTableTitle();
            Test();
        }

        private void SetBigTableTitle()
        {
            DataRow row = bigTable.NewRow();
            List<string> titles = bigTableTitleManager.GetAllDescriptions();

            for (int i = 0; i < titles.Count; i++)
            {
                row[i] = titles[i];
            }

            bigTable.Rows.Add(row);
        }

        private void Test()
        {
            int idx1 = bigTableTitleManager.GetIndex(BigTableTitle.WHEN3);
            int idx2 = bigTableTitleManager.GetIndex(BigTableTitle.WHO2);

            BigTableData data = new BigTableData(titleLen);
            data.Set(idx1, "WHEN3");
            data.Set(idx2, "WHO2");
            string[] values = data.GetValues();

            DataRow row = bigTable.NewRow();
            for (int i = 0; i < values.Length; i++)
            {
                row[i] = values[i];
            }

            bigTable.Rows.Add(row);
        }
    }
}