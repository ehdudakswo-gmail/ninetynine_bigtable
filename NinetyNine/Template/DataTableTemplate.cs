using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.Template
{
    abstract class DataTableTemplate
    {
        protected ExcelDataManager ExcelDataManager = ExcelDataManager.GetInstance();
        protected DataTable dataTable;
        protected DataRowCollection rows;

        abstract internal DataTable GetTemplateDataTable();

        protected void Init()
        {
            string tableName = GetType().Name;
            dataTable = ExcelDataManager.GetBasicDataTable(tableName);
            rows = dataTable.Rows;
        }

        protected void SetTitleDescriptions(DataRow row, Array values)
        {
            List<string> descriptions = EnumManager.GetAllDescriptions(values);

            for (int i = 0; i < descriptions.Count; i++)
            {
                row[i] = descriptions[i];
            }
        }

        protected void SetTitleTexts(DataRow row, Array values)
        {
            List<string> texts = EnumManager.GetTexts(values);

            for (int i = 0; i < texts.Count; i++)
            {
                row[i] = texts[i];
            }
        }

        internal void Refresh(DataTable targetTable)
        {
            targetTable.Clear();
            DataTable templateTable = GetTemplateDataTable();
            DataRowCollection dataRows = targetTable.Rows;
            int columnCount = targetTable.Columns.Count;

            foreach (DataRow templateRow in templateTable.Rows)
            {
                DataRow newRow = targetTable.NewRow();
                CopyValue(newRow, templateRow, columnCount);
                dataRows.Add(newRow);
            }
        }

        private void CopyValue(DataRow dataRow, DataRow templateRow, int columnCount)
        {
            for (int i = 0; i < columnCount; i++)
            {
                dataRow[i] = templateRow[i];
            }
        }
    }
}