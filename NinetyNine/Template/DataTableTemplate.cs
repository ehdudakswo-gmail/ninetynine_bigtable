using NinetyNine.BigTable;
using System;
using System.Data;

namespace NinetyNine.Template
{
    abstract class DataTableTemplate
    {
        protected BigTableError bigTableError = BigTableError.GetInstance();

        abstract internal void Set(DataTable dataTable);
    }
}