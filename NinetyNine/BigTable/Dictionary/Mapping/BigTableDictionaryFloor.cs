using NinetyNine.Template;
using NinetyNine.Template.Mapping;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary.Mapping
{
    class BigTableDictionaryFloor : BigTableDictionary
    {
        private const int CONTENT_ROWIDX = 1;
        private Enum[] keys = new Enum[] { FloorTitle.BigTable_Floor };

        internal BigTableDictionaryFloor(DataTable dataTable, DataTableTemplate template) : base(dataTable, template)
        {
        }

        internal override void SetMappingKeys(SortedSet<string[]> sortedKeys)
        {
            RefreshTemplate();
            SetMappingKeys(sortedKeys, keys);
        }

        internal override Dictionary<string, DataRow> Create()
        {
            Dictionary<string, DataRow> dictionary = new Dictionary<string, DataRow>();

            for (int rowIdx = 0; rowIdx < rows.Count; rowIdx++)
            {
                if (rowIdx < CONTENT_ROWIDX)
                {
                }
                else
                {
                    DataRow row = rows[rowIdx];
                    BigTableErrorCell[] errorCells = GetErrorCells(rowIdx, keys);

                    int key1ColIdx = GetColumnIdx(keys[0]);
                    string key1Str = row[key1ColIdx].ToString();

                    if (IsEmpty(key1Str))
                    {
                        ThrowException(dataTable, errorCells, ERROR_EMPTY);
                    }

                    string key = GetKey(row, keys);
                    if (dictionary.ContainsKey(key))
                    {
                        ThrowException(dataTable, errorCells, ERROR_KEY_CONTAIN);
                    }

                    dictionary.Add(key, row);
                }
            }

            return dictionary;
        }
    }
}