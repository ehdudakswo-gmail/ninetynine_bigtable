using System.Data;

namespace NinetyNine.Template
{
    internal class DataTableTemplateOrganization : DataTableTemplate
    {
        internal override DataTable GetTemplateDataTable()
        {
            Init();

            DataRow row0 = dataTable.NewRow();
            SetRow0(row0);
            rows.Add(row0);

            return dataTable;
        }

        private void SetRow0(DataRow row)
        {
            row[0] = "조직도";
        }
    }
}