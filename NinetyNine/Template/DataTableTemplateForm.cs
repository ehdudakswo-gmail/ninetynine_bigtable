using System.Data;

namespace NinetyNine.Template
{
    class DataTableTemplateForm : DataTableTemplate
    {
        internal override DataTable GetTemplateDataTable()
        {
            Init();

            return dataTable;
        }

        private void SetRow0(DataRow row)
        {
        }
    }
}