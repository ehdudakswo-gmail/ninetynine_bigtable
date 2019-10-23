using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.Template
{
    enum FormTitle
    {
        [Description("층")]
        Floor,

        [Description("부호")]
        Mark,

        [Description("명칭")]
        Name,

        [Description("규격")]
        Standard,

        [Description("산출식")]
        Calculation,

        [Description("결과값")]
        Result,
    }

    class DataTableTemplateForm : DataTableTemplate
    {

        internal static readonly string SPACE = " ";
        internal static readonly string COLON = ":";
        internal static readonly string DASH = "-";
        internal static readonly string BIG_OPEN = "[";
        internal static readonly string BIG_CLOSE = "]";

        internal static readonly string CONSTRUCTION_NAME_LABEL = "공사명";
        internal static readonly string CONSTRUCTION_NAME_SEPARATOR = COLON;
        internal static readonly string[] CONSTRUCTION_NAME_FORMAT = new string[]
        {
            CONSTRUCTION_NAME_LABEL,
            CONSTRUCTION_NAME_SEPARATOR,
        };

        internal static readonly string BLOCK_NAME_LABEL = "동명";
        internal static readonly string BLOCK_NAME_SEPARATOR = COLON;
        internal static readonly string BLOCK_NAME_BLOCK_OPEN = BIG_OPEN;
        internal static readonly string BLOCK_NAME_BLOCK_CLOSE = BIG_CLOSE;
        internal static readonly string BLOCK_NAME_VALUE_SEPARATOR = DASH;
        internal static readonly string[] BLOCK_NAME_FORMAT = new string[]
        {
            BLOCK_NAME_LABEL,
            BLOCK_NAME_SEPARATOR,
            BLOCK_NAME_BLOCK_OPEN,
            BLOCK_NAME_BLOCK_CLOSE,
            BLOCK_NAME_VALUE_SEPARATOR,
        };
        internal static readonly string BLOCK_NAME_BLOCK_SEPARATOR = COLON;

        internal static readonly string FLOOR_NOTE = "[비고]";

        internal override void Set(DataTable dataTable)
        {
            DataRow row0 = dataTable.NewRow();
            DataRow row1 = dataTable.NewRow();
            DataRow row2 = dataTable.NewRow();
            DataRow row3 = dataTable.NewRow();

            var rows = dataTable.Rows;
            rows.Add(row0);
            rows.Add(row1);
            rows.Add(row2);

            SetRow0(row0);
            SetRow1(row1);
            SetRow2(row2);
        }

        internal static string GetFormatString(string[] format)
        {
            string str = string.Join(SPACE, format);

            return str;
        }

        private void SetRow0(DataRow row)
        {
            row[0] = GetFormatString(CONSTRUCTION_NAME_FORMAT);
        }

        private void SetRow1(DataRow row)
        {
            Array values = Enum.GetValues(typeof(FormTitle));
            List<string> descriptions = EnumManager.GetAllDescriptions(values);

            for (int i = 0; i < descriptions.Count; i++)
            {
                row[i] = descriptions[i];
            }
        }

        private void SetRow2(DataRow row)
        {
            row[0] = GetFormatString(BLOCK_NAME_FORMAT);
        }
    }
}