using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary
{
    class BigtableDictionaryStatementMapping : BigtableDictionary
    {
        private Array titles = Enum.GetValues(typeof(StatementMappingTitle));
        private Enum[] keys = new Enum[]
        {
            StatementMappingTitle.Form_Standard,
        };

        internal override Dictionary<string, DataRow> Create(DataTable dataTable)
        {
            var rows = dataTable.Rows;
            int rowCount = rows.Count;
            int columnCount = GetColumnCount(titles);
            int emptyCheckLastIdx = columnCount - 2;

            for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
            {
                if (rowIdx == 0)
                {
                    //template check
                }
                else
                {
                    DataRow row = rows[rowIdx];

                    int emptyColoumnIdx = GetEmptyIdx(row, 0, emptyCheckLastIdx);
                    if (emptyColoumnIdx != -1)
                    {
                        string error = ERROR_VALUE_EMPTY;
                        bigTableError.ThrowException(dataTable, rowIdx, emptyColoumnIdx, error);
                    }

                    string key = GetKey(row, keys);
                    if (dictionary.ContainsKey(key))
                    {
                        int keyColumnIdx = GetKeyIndex(keys);
                        string error = string.Format(ERROR_KEY_CONTAIN, key);
                        bigTableError.ThrowException(dataTable, rowIdx, keyColumnIdx, error);
                    }

                    dictionary.Add(key, row);
                }
            }

            return dictionary;
        }
    }
}