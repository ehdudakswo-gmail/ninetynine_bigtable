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

    enum FormRowIdx
    {
        First = 3,
        Last = 19883,
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