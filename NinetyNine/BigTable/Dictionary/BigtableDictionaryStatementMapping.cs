using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary
{
    class BigtableDictionaryStatementMapping : BigtableDictionary
    {
        private Enum[] keys = new Enum[]
        {
            StatementMappingTitle.Form_Standard,
        };

        private HashSet<string> valueKeyCheck = new HashSet<string>();
        private Enum[] valueKeys = new Enum[]
        {
            StatementMappingTitle.Statement_Name,
            StatementMappingTitle.Statement_Standard,
        };

        private Array enumValues = Enum.GetValues(typeof(StatementMappingTitle));
        private HashSet<Enum> emptyCheckSkip = new HashSet<Enum>
        {
            StatementMappingTitle.Statement_Standard,
        };

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