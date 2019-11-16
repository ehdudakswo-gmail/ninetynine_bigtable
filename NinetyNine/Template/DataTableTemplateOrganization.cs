using System.Data;

namespace NinetyNine.Template
{
    internal class DataTableTemplateOrganization : DataTableTemplate
    {
        internal override DataTable GetTemplateDataTable()
        {
            Init();

            return dataTable;
        }
    }
}