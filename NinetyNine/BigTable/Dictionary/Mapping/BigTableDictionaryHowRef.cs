using NinetyNine.Template;
using NinetyNine.Template.Mapping;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary.Mapping
{
    class BigTableDictionaryHowRef : BigTableDictionary
    {
        private Enum[] keys = new Enum[] {
            HowRefTitle.BigTable_WorkLarge,
            HowRefTitle.BigTable_WorkMedium,
            HowRefTitle.BigTable_WorkSmall,
        };

        internal BigTableDictionaryHowRef(DataTable dataTable, DataTableTemplate template) : base(dataTable, template)
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
            int contentRowIdx = EnumManager.GetIndex(HowRefRowIdx.First);

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
                    int key3ColIdx = GetColumnIdx(keys[2]);

                    string key1Str = row[key1ColIdx].ToString();
                    string key2Str = row[key2ColIdx].ToString();
                    string key3Str = row[key3ColIdx].ToString();

                    if (IsEmpty(key1Str) || IsEmpty(key2Str) || IsEmpty(key3Str))
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