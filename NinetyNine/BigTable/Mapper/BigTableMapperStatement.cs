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
        internal BigTableMapperStatement(DataTable bigTable) : base(bigTable)
        {
        }

        internal Dictionary<string, DataRow> statementDictionary { get; set; }
        internal Dictionary<string, DataRow> workDictionary { get; set; }

        internal override void Mapping()
        {
            Enum[] bigTableKeys = new Enum[] { BigTableTitle.HOW4, BigTableTitle.HOW5 };
            Enum[] workKeys = new Enum[] { WorkTitle.Statement_Construction, WorkTitle.Statement_Name, WorkTitle.Statement_Standard };

            for (int rowIdx = 0; rowIdx < bigTableRowsCount; rowIdx++)
            {
                if (rowIdx < CONTENT_ROWIDX)
                {
                }
                else
                {
                    DataRow bigTableRow = bigTableRows[rowIdx];
                    BigTableErrorCell[] errorCells = GetErrorCells(rowIdx, bigTableKeys);

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
                    Mapping(bigTableRow, BigTableTitle.WHY1, statementRow, StatementTitle.Note);
                    Mapping(bigTableRow, BigTableTitle.ATTRIBUTE1, statementRow, StatementTitle.Unit);
                    Mapping(bigTableRow, BigTableTitle.ATTRIBUTE2, statementRow, StatementTitle.MaterialCost);
                    Mapping(bigTableRow, BigTableTitle.ATTRIBUTE3, statementRow, StatementTitle.LaborCost);
                    Mapping(bigTableRow, BigTableTitle.ATTRIBUTE4, statementRow, StatementTitle.ExpenseCost);
                    Mapping(bigTableRow, BigTableTitle.ATTRIBUTE5, statementRow, StatementTitle.Total);

                    MappingConversionQuantity(bigTableRow, workRow);
                    MappingResult(bigTableRow, BigTableTitle.RESULT4, BigTableTitle.ATTRIBUTE2);
                    MappingResult(bigTableRow, BigTableTitle.RESULT5, BigTableTitle.ATTRIBUTE3);
                    MappingResult(bigTableRow, BigTableTitle.RESULT6, BigTableTitle.ATTRIBUTE4);
                    MappingResult(bigTableRow, BigTableTitle.RESULT7, BigTableTitle.ATTRIBUTE5);
                }
            }
        }

        private void MappingConversionQuantity(DataRow bigTableRow, DataRow workRow)
        {
            double quantity = GetNum(bigTableRow, BigTableTitle.RESULT2);
            double conversion = GetNum(workRow, WorkTitle.ConversionQuantity);
            double result = (quantity / conversion);

            int resultIdx = GetColumnIdx(BigTableTitle.RESULT3);
            bigTableRow[resultIdx] = result;
        }

        private void MappingResult(DataRow bigTableRow, BigTableTitle resultTitle, BigTableTitle attributeTitle)
        {
            double conversion = GetNum(bigTableRow, BigTableTitle.RESULT3);
            double attribute = GetNum(bigTableRow, attributeTitle);
            double result = (conversion * attribute);

            int resultIdx = GetColumnIdx(resultTitle);
            bigTableRow[resultIdx] = result;
        }

        private double GetNum(DataRow row, Enum title)
        {
            int colIdx = GetColumnIdx(title);
            string str = row[colIdx].ToString();
            double num;

            double.TryParse(str, out num);
            return num;
        }
    }
}