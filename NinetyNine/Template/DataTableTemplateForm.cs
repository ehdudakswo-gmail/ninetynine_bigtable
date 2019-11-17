using System.Data;

namespace NinetyNine.Template
{
    enum FormTitle
    {
        NO = 1,
        부위,
        구분FWC,
        구분,
        재료,
        규격,
        산식,
        수량자동계산,
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