using NinetyNine.BigTable.Parser;
using NinetyNine.DataTableTemplate;
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
                bigTable = FindDataTable(MainTabPage.BigTable);
                Check();

                bigTable.Clear();
                SetBigTableTitleTexts();
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

        private void SetBigTableTitleTexts()
        {
            List<string> texts = DataTableTemplateBigTable.GetBigTableTitleTexts();
            DataRow row = bigTable.NewRow();
            bigTable.Rows.Add(row);

            for (int i = 0; i < texts.Count; i++)
            {
                row[i] = texts[i];
            }
        }

        private void CreateBigTable()
        {
            DataTable formTable = FindDataTable(MainTabPage.Form);
            BigTableParserForm formParser = new BigTableParserForm(bigTable, formTable);
            formParser.Parse();

            //DataTable whatTable = FindDataTable(MainTabPage.AutoComplete_WHAT);
            //DataTable howTable = FindDataTable(MainTabPage.AutoComplete_HOW);
            //BigTableAutoComplete autoComplete = new BigTableAutoComplete(bigTable, whatTable, howTable);
            //autoComplete.Complete();
        }

        private DataTable FindDataTable(MainTabPage targetTabPage)
        {
            foreach (DataTable dataTable in dataSet.Tables)
            {
                string dataTableName = dataTable.TableName;
                string targetTableName = MainTabPageEnum.GetDescription(targetTabPage);
                if (dataTableName == targetTableName)
                {
                    return dataTable;
                }
            }

            return null;
        }
    }
}
