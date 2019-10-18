using System.Data;

namespace NinetyNine.Template
{
    internal class DataTableTemplateOrganization : DataTableTemplate
    {
        internal override void Set(DataTable dataTable)
        {
            DataRow row0 = dataTable.NewRow();

            var rows = dataTable.Rows;
            rows.Add(row0);

            SetRow0(row0);
        }

        private void SetRow0(DataRow row)
        {
            row[0] = "조직도";
        }
    }
}