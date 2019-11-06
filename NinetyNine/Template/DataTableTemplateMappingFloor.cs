using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.Template
{
    enum MappingFloorTitle
    {
        [Description("산출서-층")]
        Form_Floor,

        [Description("공정표-구분")]
        Schedule_Floor,
    }

    class DataTableTemplateMappingFloor : DataTableTemplate
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
            Array values = Enum.GetValues(typeof(MappingFloorTitle));
            List<string> descriptions = EnumManager.GetAllDescriptions(values);

            for (int i = 0; i < descriptions.Count; i++)
            {
                row[i] = descriptions[i];
            }
        }
    }
}