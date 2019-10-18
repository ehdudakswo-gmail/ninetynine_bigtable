using System;
using System.Data;
using NinetyNine.BigTable.Dictionary;
using NinetyNine.Template;

namespace NinetyNine
{
    internal class BigtableDictionaryStatement : BigtableDictionary
    {
        private Array titles = Enum.GetValues(typeof(StatementTitleEnum));
        private Enum[] keys = new Enum[] {
            StatementTitleEnum.Name,
            StatementTitleEnum.Standard,
        };

        internal override void Create(DataTable dataTable)
        {
            var rows = dataTable.Rows;
            int rowCount = rows.Count;
            int columnCount = GetColumnCount(titles);

            for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
            {
                if (rowIdx == 0)
                {
                    //template check
                }
                else
                {
                    DataRow row = rows[rowIdx];

                    int emptyCheckLastIdx = 0;
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

                    CheckNumber(StatementTitleEnum.Quantity, row, rowIdx, dataTable);
                    CheckNumber(StatementTitleEnum.MaterialCost, row, rowIdx, dataTable);
                    CheckNumber(StatementTitleEnum.LaborCost, row, rowIdx, dataTable);
                    CheckNumber(StatementTitleEnum.Expenses, row, rowIdx, dataTable);
                    CheckNumber(StatementTitleEnum.Total, row, rowIdx, dataTable);

                    dictionary.Add(key, row);
                }
            }
        }
    }
}