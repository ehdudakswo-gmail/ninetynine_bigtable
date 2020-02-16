using NinetyNine.Template;
using NinetyNine.Template.Mapping;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary.Mapping
{
    class BigTableDictionaryWhatRef : BigTableDictionary
    {
        private Enum[] keys = new Enum[] {
            WhatRefTitle.BigTable_Structure,
            WhatRefTitle.BigTable_StructureSeparation,
        };

        internal BigTableDictionaryWhatRef(DataTable dataTable, DataTableTemplate template) : base(dataTable, template)
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
            int contentRowIdx = EnumManager.GetIndex(WhatRefRowIdx.First);

            for (int rowIdx = 0; rowIdx < rows.Count; rowIdx++)
            {
                if (rowIdx < contentRowIdx)
                {
                }
                else
                {
                    DataRow row = rows[rowIdx];
                    BigTableErrorCell[] errorCells = GetErrorCells(rowIdx, keys);

                    int key1ColIdx = GetColumnIdx(keys[0]);
                    int key2ColIdx = GetColumnIdx(keys[1]);

                    string key1Str = row[key1ColIdx].ToString();
                    string key2Str = row[key2ColIdx].ToString();

                    if (IsEmpty(key1Str) || IsEmpty(key2Str))
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
