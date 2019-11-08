using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.Template
{
    enum BigTableHOW5Title
    {
        [Description("HOW5: 규격")]
        HOW5,

        [Description("내역서: 품명")]
        Statement_Name,

        [Description("내역서: 규격")]
        Statement_Standard,

        [Description("공정표: Description")]
        Schedule_Description,
    }

    internal class DataTableTemplateBigTableHOW5 : DataTableTemplate
    {
        internal override DataTable GetTemplateDataTable()
        {
            string templateTableName = GetTemplateTableName();
            DataTable templateTable = ExcelDataManager.GetBasicDataTable(templateTableName);
            DataRowCollection rows = templateTable.Rows;

            DataRow row0 = templateTable.NewRow();
            SetRow0(row0);
            rows.Add(row0);

            return templateTable;
        }

        private void SetRow0(DataRow row)
        {
            Array values = Enum.GetValues(typeof(BigTableHOW5Title));
            List<string> descriptions = EnumManager.GetAllDescriptions(values);

            for (int i = 0; i < descriptions.Count; i++)
            {
                row[i] = descriptions[i];
            }
        }
    }
}