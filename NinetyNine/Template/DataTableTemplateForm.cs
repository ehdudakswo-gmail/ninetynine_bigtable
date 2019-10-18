using System.Data;

namespace NinetyNine.Template
{
    class DataTableTemplateForm : DataTableTemplate
    {
        internal static readonly string SPACE = " ";
        internal static readonly string SUBJECT = "부재별산출서";
        internal static readonly string[] DATA_COLUMNS = new string[]
        { "층", "부호", "명칭", "규격", "산출식", "결과값" };

        internal static readonly string DATA_TITLE_LEFT = "공사명";
        internal static readonly string DATA_TITLE_SEPARATOR = ":";

        internal static readonly string DATA2_TITLE_LEFT = "동명";
        internal static readonly string DATA2_TITLE_SEPARATOR = ":";
        internal static readonly string DATA2_CONTENT_SEPARATOR = "-";
        internal static readonly string DATA2_CONTENT_SUB_CONTAINER_OPEN = "[";
        internal static readonly string DATA2_CONTENT_SUB_CONTAINER_CLOSE = "]";
        internal static readonly string DATA2_CONTENT_SUB_SEPARATOR = ":";

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
            rows.Add(row3);

            SetRow0(row0);
            SetRow1(row1);
            SetRow2(row2);
            SetRow3(row3);
        }

        private void SetRow0(DataRow row)
        {
            row[0] = SUBJECT;
        }

        private void SetRow1(DataRow row)
        {
            row[0] = DATA_TITLE_LEFT + SPACE + DATA_TITLE_SEPARATOR;
        }

        private void SetRow2(DataRow row)
        {
            for (int i = 0; i < DATA_COLUMNS.Length; i++)
            {
                row[i] = DATA_COLUMNS[i];
            }
        }

        private void SetRow3(DataRow row)
        {
            row[0] = DATA2_TITLE_LEFT + SPACE + DATA2_TITLE_SEPARATOR;
        }
    }
}