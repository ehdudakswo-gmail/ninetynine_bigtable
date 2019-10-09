using NinetyNine.BigTable.Parser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace NinetyNine
{
    internal class BigTableManager
    {
        private readonly string ERROR_EMPTY_SHEET = "{0} SHEET 내용이 필요합니다.";

        private ExcelDataManager excelDataManager = new ExcelDataManager();
        private DataSet dataSet;
        private DataTable bigTable;

        internal Task<string> Refresh(DataSet dataSet)
        {
            return Task.Run(() =>
            {
                this.dataSet = dataSet;
                string bigTableName = TableEnum.GetDescription(Table.BigTable);
                bigTable = FindDataTable(bigTableName);
                Check();

                bigTable.Clear();
                SetBigTableTitle();
                CreateBigTable();

                return dataSet.ToString();
            });
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
            List<string> titles = BigTableTitleEnum.GetAllDescriptions();

            for (int i = 0; i < titles.Count; i++)
            {
                row[i] = titles[i];
            }

            bigTable.Rows.Add(row);
        }

        private void CreateBigTable()
        {
            string formTableName = TableEnum.GetDescription(Table.Form);
            DataTable formTable = FindDataTable(formTableName);
            BigTableFormParser formParser = new BigTableFormParser(bigTable, formTable);
            formParser.Parse();
        }

        private DataTable FindDataTable(string targetName)
        {
            foreach (DataTable dataTable in dataSet.Tables)
            {
                string tableName = dataTable.TableName;
                if (tableName == targetName)
                {
                    return dataTable;
                }
            }

            return null;
        }
    }
}
