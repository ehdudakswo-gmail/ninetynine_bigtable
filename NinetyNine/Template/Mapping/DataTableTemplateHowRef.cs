using System;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.Template.Mapping
{
    enum HowRefTitle
    {
        [Description("빅테이블: 대공종")]
        BigTable_WorkLarge,

        [Description("빅테이블: 중공종")]
        BigTable_WorkMedium,

        [Description("빅테이블: 세공종")]
        BigTable_WorkSmall,
    }

    enum HowRefRowIdx
    {
        First = 1,
    }

    class DataTableTemplateHowRef : DataTableTemplate
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
            Array values = Enum.GetValues(typeof(HowRefTitle));
            SetTitleDescriptions(row, values);
        }

        private void SetContent(DataTable dataTable)
        {
            string[][] contents = new string[][]
            {
               new string[]{"건축공사", "철근콘크리트공사", "콘크리트공사"},
               new string[]{"건축공사", "철근콘크리트공사", "철근공사"},
               new string[]{"건축공사", "철근콘크리트공사", "거푸집공사"},
               new string[]{"건축공사", "조적공사", "벽돌쌓기공사"},
               new string[]{"건축공사", "조적공사", "벽돌쌓기보조공사"},
               new string[]{"건축공사", "미장공사", "단열모르터바름"},
               new string[]{"건축공사", "미장공사", "시멘트모르터바름"},
               new string[]{"건축공사", "타일공사", "도기질타일공사"},
               new string[]{"건축공사", "타일공사", "자기질타일공사"},
               new string[]{"건축공사", "타일공사", "플리싱타일공사"},
               new string[]{"건축공사", "방수공사", "도막방수공사"},
               new string[]{"건축공사", "방수공사", "액체방수"},
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
