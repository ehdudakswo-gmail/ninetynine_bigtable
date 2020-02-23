using NinetyNine.Template;
using NinetyNine.Template.Mapping;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary.Mapping
{
    class BigTableDictionaryHow : BigTableDictionary
    {
        private const int CONTENT_ROWIDX = 1;
        private Enum[] keys = new Enum[] { HowTitle.BigTable_WorkName, HowTitle.BigTable_WorkStandard };

        internal BigTableDictionaryHow(DataTable dataTable, DataTableTemplate template) : base(dataTable, template)
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

        internal List<string> GetDataList(HowTitle title)
        {
            List<string> dataList = new List<string>();

            for (int rowIdx = 0; rowIdx < rows.Count; rowIdx++)
            {
                if (rowIdx < CONTENT_ROWIDX)
                {

                }
                else
                {
                    DataRow row = rows[rowIdx];
                    int colIdx = GetColumnIdx(title);
                    string value = row[colIdx].ToString();
                    string valueTrim = value.Trim();
                    dataList.Add(valueTrim);
                }
            }

            return dataList;
        }
    }
}