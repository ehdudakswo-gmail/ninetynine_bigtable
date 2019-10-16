using NinetyNine.BigTable;
using System.Data;

namespace NinetyNine.Template
{
    abstract class DataTableTemplate
    {
        protected BigTableError bigTableError = BigTableError.GetInstance();
        protected ExcelDataManager excelDataManager = ExcelDataManager.GetInstance();

        abstract public DataTable Create(string tableName);
    }
}