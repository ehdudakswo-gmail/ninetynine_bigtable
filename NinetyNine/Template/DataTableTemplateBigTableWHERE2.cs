using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.Template
{
    enum BigTableWHERE2Title
    {
        [Description("WHERE2: 층")]
        BigTable_WHERE2,

        [Description("공정표: 구분")]
        Schedule_Floor,

        [Description("WHAT2: 구조체구분")]
        BigTable_WHAT2,

        [Description("WHAT1: 구조체")]
        BigTable_WHAT1,
    }

    class DataTableTemplateBigTableWHERE2 : DataTableTemplate
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
            Array values = Enum.GetValues(typeof(BigTableWHERE2Title));
            List<string> descriptions = EnumManager.GetAllDescriptions(values);

            for (int i = 0; i < descriptions.Count; i++)
            {
                row[i] = descriptions[i];
            }
        }
    }
}