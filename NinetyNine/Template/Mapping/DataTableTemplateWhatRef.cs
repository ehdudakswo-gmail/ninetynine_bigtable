using System;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.Template.Mapping
{
    enum WhatRefTitle
    {
        [Description("빅테이블: 구조체")]
        BigTable_Structure,

        [Description("빅테이블: 구조체구분")]
        BigTable_StructureSeparation,
    }

    class DataTableTemplateWhatRef : DataTableTemplate
    {
        internal override DataTable GetTemplateDataTable()
        {
            Init();

            DataRow row0 = dataTable.NewRow();
            SetRow0(row0);
            rows.Add(row0);

            SetContent(dataTable);

            return dataTable;
        }

        private void SetRow0(DataRow row)
        {
            Array values = Enum.GetValues(typeof(WhatRefTitle));
            SetTitleDescriptions(row, values);
        }

        private void SetContent(DataTable dataTable)
        {
            string[][] contents = new string[][]
            {
               new string[] {"건축 구조체", "지상 구조체"},
               new string[] {"건축 구조체", "지하 구조체"},
               new string[] {"건축 구조체", "지붕 구조체"},
            };

            foreach (string[] content in contents)
            {
                DataRow row = dataTable.NewRow();

                for (int i = 0; i < content.Length; i++)
                {
                    string value = content[i];
                    row[i] = value;
                }

                rows.Add(row);
            }
        }
    }
}