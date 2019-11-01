using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.Template
{
    class DataTableTemplateForm : DataTableTemplate
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
            row[0] = "부재별산출서";
        }
    }
}