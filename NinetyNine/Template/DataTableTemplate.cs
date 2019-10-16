using System.Data;

namespace NinetyNine.Template
{
    abstract class DataTableTemplate
    {
        protected ExcelDataManager excelDataManager = new ExcelDataManager();

        abstract public DataTable Create(string tableName);
    }
}