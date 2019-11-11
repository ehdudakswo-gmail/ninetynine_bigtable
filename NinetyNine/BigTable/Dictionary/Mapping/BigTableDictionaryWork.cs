using NinetyNine.Template;
using NinetyNine.Template.Mapping;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary.Mapping
{
    class BigTableDictionaryWork : BigTableDictionary
    {
        private readonly int BASIC_CONVERSION_QUANTITY = 1;
        private Enum[] keys = new Enum[] { WorkTitle.BigTable_WorkName, WorkTitle.BigTable_WorkStandard, };

        internal BigTableDictionaryWork(DataTable dataTable, DataTableTemplate template) : base(dataTable, template)
        {
        }

        internal override void SetMappingKeys(SortedSet<string[]> sortedKeys)
        {
            RefreshTemplate();
            SetMappingKeys(sortedKeys, keys);
            SetConversionQuantity();
        }

        private void SetConversionQuantity()
        {
            int colIdx = GetColumnIdx(WorkTitle.ConversionQuantity);

            for (int rowIdx = 0; rowIdx < rows.Count; rowIdx++)
            {
                if (rowIdx < templateRowsCount)
                {
                }
                else
                {
                    DataRow row = rows[rowIdx];
                    row[colIdx] = BASIC_CONVERSION_QUANTITY;
                }
            }
        }

        internal override Dictionary<string, DataRow> Create()
        {
            for (int rowIdx = 0; rowIdx < rows.Count; rowIdx++)
            {
                if (rowIdx < templateRowsCount)
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