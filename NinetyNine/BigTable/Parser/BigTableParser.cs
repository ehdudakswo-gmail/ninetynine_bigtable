using NinetyNine.Template;
using System.Data;

namespace NinetyNine.BigTable.Parser
{
    abstract class BigTableParser : BigTableFunction
    {
        protected BigTableData bigTableData = new BigTableData();
        protected DataTable bigTable;

        abstract internal void Parse();

        public BigTableParser(DataTable bigTable)
        {
            this.bigTable = bigTable;
        }

        protected void SetData(BigTableTitle bigTableValue, string str)
        {
            bigTableData.Set(bigTableValue, str);
        }

        protected void AddDataRow()
        {
            string[] values = bigTableData.GetValues();
            DataRow row = bigTable.NewRow();

            for (int i = 0; i < values.Length; i++)
            {
                row[i] = values[i];
            }

            bigTable.Rows.Add(row);
        }
    }
}