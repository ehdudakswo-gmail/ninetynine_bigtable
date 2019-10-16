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
            List<string> texts = GetBigTableTitleTexts();
            for (int i = 0; i < texts.Count; i++)
            {
                row[i] = texts[i];
            }
        }

        internal static List<string> GetBigTableTitleTexts()
        {
            Array values = BigTableTitleEnum.GetValues();
            List<string> texts = new List<string>();

            foreach (BigTableTitle value in values)
            {
                string enumName = value.ToString();
                string enumValue = BigTableTitleEnum.GetDescription(value);
                string text = string.Format("{0}: {1}", enumName, enumValue);
                texts.Add(text);
            }

            return texts;
        }
    }
}