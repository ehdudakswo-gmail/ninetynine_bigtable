using System.Data;

namespace NinetyNine.Template
{
    enum FormTitle
    {
        층,
        부호,
        명칭,
        규격,
        산출식,
        결과값,
    }

    class DataTableTemplateForm : DataTableTemplate
    {
        internal override DataTable GetTemplateDataTable()
        {
            Init();

            return dataTable;
        }
    }
}