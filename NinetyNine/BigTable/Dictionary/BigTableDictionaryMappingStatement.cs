using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary
{
    class BigTableDictionaryMappingStatement : BigTableDictionary
    {
        private Enum[] keys = new Enum[]
        {
            MappingStatementTitle.Form_Standard,
        };

        private HashSet<string> valueKeyCheck = new HashSet<string>();
        private Enum[] valueKeys = new Enum[]
        {
            MappingStatementTitle.Statement_Name,
            MappingStatementTitle.Statement_Standard,
        };

        private Array enumValues = Enum.GetValues(typeof(MappingStatementTitle));
        private HashSet<Enum> emptyCheckSkip = new HashSet<Enum>
        {
            MappingStatementTitle.Statement_Standard,
        };

        internal override void SetTemplate(DataTable bigTable, DataTable dictionaryTable)
        {
            RefreshTemplate(dictionaryTable, new DataTableTemplateMappingStatement());
            SetKeyTemplate(bigTable, BigTableTitle.HOW5, dictionaryTable, keys[0]);
        }

        internal override Dictionary<string, DataRow> Create(DataTable dataTable)
        {
            var rows = dataTable.Rows;
            int rowCount = rows.Count;

            for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
            {
                if (rowIdx == 0)
                {
                    //template check
                }
                else
                {
                    DataRow row = rows[rowIdx];

                    int emptyColIdx = GetEmptyIdx(row, enumValues, emptyCheckSkip);
                    if (emptyColIdx != -1)
                    {
                        BigTableErrorCell[] errorCells = GetErrorCells(rowIdx, emptyColIdx);
                        ThrowException(dataTable, errorCells, ERROR_VALUE_EMPTY);
                    }

                    string key = GetKey(row, keys);
                    if (dictionary.ContainsKey(key))
                    {
                        BigTableErrorCell[] errorCells = GetErrorCells(rowIdx, keys);
                        ThrowException(dataTable, errorCells, ERROR_KEY_CONTAIN);
                    }

                    string valueKey = GetKey(row, valueKeys);
                    if (valueKeyCheck.Contains(valueKey))
                    {
                        BigTableErrorCell[] errorCells = GetErrorCells(rowIdx, valueKeys);
                        ThrowException(dataTable, errorCells, ERROR_KEY_CONTAIN);
                    }

                    dictionary.Add(key, row);
                    valueKeyCheck.Add(valueKey);
                }
            }

            return dictionary;
        }
    }
}