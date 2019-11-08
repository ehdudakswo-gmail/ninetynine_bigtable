using System;
using System.Collections.Generic;
using System.Data;
using NinetyNine.BigTable;
using NinetyNine.BigTable.Dictionary;
using NinetyNine.Template;

namespace NinetyNine
{
    internal class BigTableDictionaryStatement : BigTableDictionary
    {
        internal BigTableDictionaryStatement(DataTable dataTable, DataTableTemplate template) : base(dataTable, template)
        {
        }

        internal override void SetMappingKeys(SortedSet<string[]> sortedKeys)
        {
        }

        internal override Dictionary<string, DataRow> Create()
        {
            Array enumValues = Enum.GetValues(typeof(StatementTitle));
            Enum[] keys = new Enum[] { StatementTitle.Name, StatementTitle.Standard, };
            HashSet<Enum> emptyCheckSkip = new HashSet<Enum> { StatementTitle.Standard, };

            for (int rowIdx = 0; rowIdx < rows.Count; rowIdx++)
            {
                if (rowIdx < templateRowsCount)
                {
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

                    CheckNumber(StatementTitle.Quantity, row, rowIdx, dataTable);
                    CheckNumber(StatementTitle.MaterialCost, row, rowIdx, dataTable);
                    CheckNumber(StatementTitle.LaborCost, row, rowIdx, dataTable);
                    CheckNumber(StatementTitle.Expenses, row, rowIdx, dataTable);
                    CheckNumber(StatementTitle.Total, row, rowIdx, dataTable);

                    dictionary.Add(key, row);
                }
            }

            return dictionary;
        }

        private void CheckNumber(StatementTitle value, DataRow row, int rowIdx, DataTable dataTable)
        {
            int colIdx = GetColumnIdx(value);
            string cellValue = row[colIdx].ToString();

            if (isNumber(cellValue))
            {
                return;
            }

            BigTableErrorCell[] errorCells = GetErrorCells(rowIdx, colIdx);
            ThrowException(dataTable, errorCells, ERROR_NOT_NUMBER);
        }
    }
}