using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine
{
    internal class BigTableManager
    {
        private readonly string ERROR_EMPTY_SHEET = "{0} SHEET 내용이 필요합니다.";

        private ExcelDataManager excelDataManager = new ExcelDataManager();
        private BigTableTitleEnum bigTableTitleEnum = new BigTableTitleEnum();
        private DataSet dataSet;
        private DataTable bigTable;

        internal void Refresh(DataSet dataSet, DataTable bigTable)
        {
            this.dataSet = dataSet;
            this.bigTable = bigTable;
            Check();

            bigTable.Clear();
            SetBigTableTitle();
        }

        private void Check()
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

        private void SetBigTableTitle()
        {
            DataRow row = bigTable.NewRow();
            List<string> titles = bigTableTitleEnum.GetAllDescriptions();

            for (int i = 0; i < titles.Count; i++)
            {
                row[i] = titles[i];
            }

            bigTable.Rows.Add(row);
        }
    }
}