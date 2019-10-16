using NinetyNine.BigTable;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.Template
{
    class DataTableTemplateAutoComplete : DataTableTemplate
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

        internal void CheckError(DataTable dataTable)
        {
            CheckErrorRow0(dataTable);
        }

        private void CheckErrorRow0(DataTable dataTable)
        {
            DataRow row0 = dataTable.Rows[0];
            List<string> texts = BigTableTitleEnum.GetTexts();

            for (int i = 0; i < texts.Count; i++)
            {
                string bigTableText = texts[i];
                string row0Text = row0[i].ToString();

                if (bigTableText != row0Text)
                {
                    string error = string.Format("{0} 값이 필요합니다.", bigTableText);
                    bigTableError.ThrowException(dataTable, 1, i, error);
                }
            }
        }
    }
}
