using NinetyNine.BigTable.Dictionary;
using NinetyNine.Template;
using NinetyNine.Template.Mapping;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Mapper
{
    class BigTableMapperStatement : BigTableMapper
    {
        internal DataTable bigTable { get; set; }
        internal Dictionary<string, DataRow> statementDictionary { get; set; }
        internal Dictionary<string, DataRow> workDictionary { get; set; }

        internal override void Mapping()
        {
            Enum[] bigTableKeys = new Enum[] { BigTableTitle.HOW4, BigTableTitle.HOW5 };
            Enum[] workKeys = new Enum[] { BigTableWorkTitle.Statement_Name, BigTableWorkTitle.Statement_Standard };

            var bigTableRows = bigTable.Rows;
            int bigTableRowsCount = bigTableRows.Count;

            for (int bigTableRowIdx = 0; bigTableRowIdx < bigTableRowsCount; bigTableRowIdx++)
            {
                if (bigTableRowIdx < templateRowsCount)
                {
                }
                else
                {
                    DataRow bigTableRow = bigTableRows[bigTableRowIdx];
                    BigTableErrorCell[] errorCells = GetErrorCells(bigTableRowIdx, bigTableKeys);

                    string bigTableKey = BigTableDictionary.GetKey(bigTableRow, bigTableKeys);
                    if (workDictionary.ContainsKey(bigTableKey) == false)
                    {
                        ThrowException(bigTable, errorCells, ERROR_KEY_NONE);
                    }

                    DataRow workRow = workDictionary[bigTableKey];
                    string workKey = BigTableDictionary.GetKey(workRow, workKeys);
                    if (statementDictionary.ContainsKey(workKey) == false)
                    {
                        ThrowException(bigTable, errorCells, ERROR_KEY_NONE);
                    }

                    DataRow statementRow = statementDictionary[workKey];
                    Mapping(bigTableRow, BigTableTitle.ATTRIBUTE1, statementRow, StatementTitle.Unit);
                    Mapping(bigTableRow, BigTableTitle.ATTRIBUTE2, statementRow, StatementTitle.MaterialCost);
                    Mapping(bigTableRow, BigTableTitle.ATTRIBUTE3, statementRow, StatementTitle.LaborCost);
                    Mapping(bigTableRow, BigTableTitle.ATTRIBUTE4, statementRow, StatementTitle.Expenses);
                    Mapping(bigTableRow, BigTableTitle.ATTRIBUTE5, statementRow, StatementTitle.Total);

                    ConvertQuantity(bigTableRow);
                    MappingResult(bigTableRow, BigTableTitle.RESULT4, BigTableTitle.ATTRIBUTE2);
                    MappingResult(bigTableRow, BigTableTitle.RESULT5, BigTableTitle.ATTRIBUTE3);
                    MappingResult(bigTableRow, BigTableTitle.RESULT6, BigTableTitle.ATTRIBUTE4);
                    MappingResult(bigTableRow, BigTableTitle.RESULT7, BigTableTitle.ATTRIBUTE5);
                }
            }
        }

        private void ConvertQuantity(DataRow bigTableRow)
        {
            int result2Idx = GetColumnIdx(BigTableTitle.RESULT2);
            int result3Idx = GetColumnIdx(BigTableTitle.RESULT3);
            bigTableRow[result3Idx] = bigTableRow[result2Idx];
        }

        private void MappingResult(DataRow bigTableRow, BigTableTitle result, BigTableTitle attribute)
        {
            int conversionIdx = GetColumnIdx(BigTableTitle.RESULT3);
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
    }
}