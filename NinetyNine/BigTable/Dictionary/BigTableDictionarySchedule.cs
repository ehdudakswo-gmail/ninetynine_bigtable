using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary
{
    class BigTableDictionarySchedule : BigTableDictionary
    {
        internal BigTableDictionarySchedule(DataTable dataTable, DataTableTemplate template) : base(dataTable, template)
        {
        }

        internal override Dictionary<string, DataRow> Create()
        {
            throw new System.NotImplementedException();
        }

        internal override void SetMappingKeys(SortedSet<string[]> sortedKeys)
        {
            throw new System.NotImplementedException();
        }

        internal DateTime GetBasicDateTime()
        {
            int rowIdx = 2;
            int colIdx = 0;
            string str = rows[rowIdx][colIdx].ToString();

            DateTime dateTime;
            bool IsParse = DateTime.TryParse(str, out dateTime);
            if (IsParse == false)
            {
                BigTableErrorCell[] errorCells = GetErrorCells(rowIdx, colIdx);
                ThrowException(dataTable, errorCells, ERROR_FORMAT_DATETIME);
            }

            return dateTime;
        }
    }
}