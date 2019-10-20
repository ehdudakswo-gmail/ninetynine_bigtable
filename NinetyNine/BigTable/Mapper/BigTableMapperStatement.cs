using NinetyNine.BigTable.Dictionary;
using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Mapper
{
    class BigTableMapperStatement : BigTableMapper
    {
        internal DataTable bigTable { get; set; }
        internal DataTable statementMappingTable { get; set; }
        internal DataTable statementTable { get; set; }
        internal Dictionary<string, DataRow> statementMappingDictionary { get; set; }
        internal Dictionary<string, DataRow> statementDictionary { get; set; }

        internal override void Mapping()
        {
            var bigTableRows = bigTable.Rows;
            var bigTableRowsCount = bigTableRows.Count;

            for (int bigTableRowIdx = 0; bigTableRowIdx < bigTableRowsCount; bigTableRowIdx++)
            {
                if (bigTableRowIdx == 0)
                {
                    //template check
                }
                else
                {
                    var bigTableRow = bigTableRows[bigTableRowIdx];

                    Enum[] statementMappingKeys = new Enum[] {
                        BigTableTitle.HOW5
                    };
                    string statementMappingKey = BigtableDictionary.GetKey(bigTableRow, statementMappingKeys);
                    if (statementMappingDictionary.ContainsKey(statementMappingKey) == false)
                    {
                        ThrowStatementMappingKeyError(bigTableRowIdx, statementMappingKeys);
                    }

                    DataRow statementMappingRow = statementMappingDictionary[statementMappingKey];
                    Enum[] statementKeys = new Enum[] {
                        StatementMappingTitle.Statement_Name,
                        StatementMappingTitle.Statement_Standard
                    };
                    string statementKey = BigtableDictionary.GetKey(statementMappingRow, statementKeys);
                    if (statementDictionary.ContainsKey(statementKey) == false)
                    {
                        ThrowStatementKeyError(
                            GetRowIndex(statementMappingTable, statementMappingRow),
                            statementKeys
                        );
                    }

                    DataRow statementRow = statementDictionary[statementKey];
                    MappingAttribute(bigTableRow, BigTableTitle.ATTRIBUTE1, statementRow, StatementTitle.Unit);
                    MappingAttribute(bigTableRow, BigTableTitle.ATTRIBUTE2, statementRow, StatementTitle.MaterialCost);
                    MappingAttribute(bigTableRow, BigTableTitle.ATTRIBUTE3, statementRow, StatementTitle.LaborCost);
                    MappingAttribute(bigTableRow, BigTableTitle.ATTRIBUTE4, statementRow, StatementTitle.Expenses);
                    MappingAttribute(bigTableRow, BigTableTitle.ATTRIBUTE5, statementRow, StatementTitle.Total);

                    ConvertQuantity(bigTableRow);
                    MappingResult(bigTableRow, BigTableTitle.RESULT3, BigTableTitle.ATTRIBUTE2);
                    MappingResult(bigTableRow, BigTableTitle.RESULT4, BigTableTitle.ATTRIBUTE3);
                    MappingResult(bigTableRow, BigTableTitle.RESULT5, BigTableTitle.ATTRIBUTE4);
                    MappingResult(bigTableRow, BigTableTitle.RESULT6, BigTableTitle.ATTRIBUTE5);
                }
            }
        }

        private void MappingAttribute(DataRow bigTableRow, BigTableTitle bigTableTitle, DataRow statementRow, StatementTitle statementTitle)
        {
            int bigTableIdx = GetColumnIdx(bigTableTitle);
            int statementTableIdx = GetColumnIdx(statementTitle);
            bigTableRow[bigTableIdx] = statementRow[statementTableIdx];
        }

        private void ConvertQuantity(DataRow bigTableRow)
        {
            int result2Idx = GetColumnIdx(BigTableTitle.RESULT2);
            int result2_1Idx = GetColumnIdx(BigTableTitle.RESULT2_1);
            bigTableRow[result2_1Idx] = bigTableRow[result2Idx];
        }

        private void MappingResult(DataRow bigTableRow, BigTableTitle result, BigTableTitle attribute)
        {
            int conversionIdx = GetColumnIdx(BigTableTitle.RESULT2_1);
            int resultIdx = GetColumnIdx(result);
            int attributeIdx = GetColumnIdx(attribute);

            string conversionStr = bigTableRow[conversionIdx].ToString();
            string attributeStr = bigTableRow[attributeIdx].ToString();

            double conversionNum;
            double attributeNum;

            double.TryParse(conversionStr, out conversionNum);
            double.TryParse(attributeStr, out attributeNum);

            double resultNum = conversionNum * attributeNum;
            bigTableRow[resultIdx] = resultNum.ToString();
        }

        private void ThrowStatementMappingKeyError(int rowIdx, Enum[] keys)
        {
            BigTableErrorData errrorData = new BigTableErrorData
            {
                dataTable = bigTable,
                cells = new BigTableErrorCell[] {
                    new BigTableErrorCell
                    {
                        rowIdx = rowIdx,
                        colIdx = GetColumnIdx(keys[0]),
                    },
                },
                error = string.Format(ERROR_DATA_NONE, statementMappingTable.TableName)
            };

            bigTableError.ThrowException(errrorData);
        }

        private void ThrowStatementKeyError(int rowIdx, Enum[] keys)
        {
            BigTableErrorData errrorData = new BigTableErrorData
            {
                dataTable = statementMappingTable,
                cells = new BigTableErrorCell[]
                {
                    new BigTableErrorCell
                    {
                        rowIdx = rowIdx,
                        colIdx = GetColumnIdx(keys[0]),
                    },
                    new BigTableErrorCell
                    {
                        rowIdx = rowIdx,
                        colIdx = GetColumnIdx(keys[1]),
                    }
                },
                error = string.Format(ERROR_DATA_NONE, statementTable.TableName)
            };

            bigTableError.ThrowException(errrorData);
        }
    }
}