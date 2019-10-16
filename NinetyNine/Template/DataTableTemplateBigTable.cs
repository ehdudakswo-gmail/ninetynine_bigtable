using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.Template
{
    internal class DataTableTemplateBigTable : DataTableTemplate
    {
        public override DataTable Create(string tableName)
        {
            DataTable dataTable = excelDataManager.GetBasicDataTable(tableName);
            DataRow row0 = dataTable.NewRow();

            var rows = dataTable.Rows;
            rows.Add(row0);

            SetRow0(row0);

            return dataTable;
        }

        private void SetRow0(DataRow row)
        {
            List<string> texts = BigTableTitleEnum.GetTexts();
            for (int i = 0; i < texts.Count; i++)
            {
                row[i] = texts[i];
            }
        }
    }
}