using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.BigTable.Parser
{
    class BigTableParserForm : BigTableParser
    {
        enum FormTitle
        {
            [Description("층")]
            Floor = 0,

            [Description("부호")]
            Mark = 1,

            [Description("명칭")]
            Name = 2,

            [Description("규격")]
            Standard = 3,

            [Description("산출식")]
            Calculation = 4,

            [Description("결과값")]
            Result = 5,
        }

        enum ParserFormDataType
        {
            Title,
            Construction,
            Block,
            Floor,
            FloorDetail,
            Unknown,
        }

        private Enum[] constructionColumns =
        {
            FormTitle.Floor,
        };

        private Enum[] blockColumns =
        {
            FormTitle.Floor,
        };


        private Enum[] floorColumns =
        {
            FormTitle.Floor,
            FormTitle.Mark,
        };

        private Enum[] floorDetailColumns =
        {
            FormTitle.Name,
            FormTitle.Standard,
            FormTitle.Calculation,
            FormTitle.Result,
        };

        private readonly string CONSTRUCTION_NAME = "공사명";
        private readonly string BLOCK_NAME = "동명";

        internal override void Parse()
        {
            var rows = formTable.Rows;
            int rowsCount = rows.Count;

            DataTableTemplate template = new DataTableTemplateForm();
            DataTable templateTable = template.GetTemplateDataTable();
            int templateRowCount = templateTable.Rows.Count;

            for (int rowIdx = 0; rowIdx < rowsCount; rowIdx++)
            {
                if (rowIdx < templateRowCount)
                {
                    //template check
                }
                else
                {
                    IsAddDataRow = false;
                    DataRow row = rows[rowIdx];
                    ParserFormDataType dataType = GetDataType(row, rowIdx);

                    SetBigTableData(dataType, row, rowIdx);
                    AddDataRow();
                }
            }
        }

        private ParserFormDataType GetDataType(DataRow row, int rowIdx)
        {
            Array values = Enum.GetValues(typeof(FormTitle));
            Enum[] validColumns = GetValidColumns(row, values);

            if (values.Length == validColumns.Length)
            {
                if (IsMatched(values, row))
                {
                    return ParserFormDataType.Title;
                }
            }

            if (IsSame(validColumns, constructionColumns))
            {
                if (IsStartWith(row, constructionColumns[0], CONSTRUCTION_NAME))
                {
                    return ParserFormDataType.Construction;
                }
            }

            if (IsSame(validColumns, blockColumns))
            {
                if (IsStartWith(row, blockColumns[0], BLOCK_NAME))
                {
                    return ParserFormDataType.Block;
                }
            }

            if (IsContain(validColumns, floorColumns))
            {
                int nameColIdx = GetColumnIdx(FormTitle.Name);
                string nameStr = row[nameColIdx].ToString();
                string nameStrTrim = Trim(nameStr);
                string NOTE = "[비고]";

                if (nameStrTrim == NOTE)
                {
                    return ParserFormDataType.Floor;
                }
            }

            if (IsSame(validColumns, floorDetailColumns))
            {
                return ParserFormDataType.FloorDetail;
            }

            return ParserFormDataType.Unknown;
        }

        private void SetBigTableData(ParserFormDataType dataType, DataRow row, int rowIdx)
        {
            switch (dataType)
            {
                case ParserFormDataType.Title:
                    //ignore
                    break;
                case ParserFormDataType.Construction:
                    HandleConstruction(row, rowIdx);
                    break;
                case ParserFormDataType.Block:
                    HandleBlock(row, rowIdx);
                    break;
                case ParserFormDataType.Floor:
                    HandleFloor(row, rowIdx);
                    break;
                case ParserFormDataType.FloorDetail:
                    HandleFloorDetail(row, rowIdx);
                    break;
                case ParserFormDataType.Unknown:
                    Array titles = Enum.GetValues(typeof(FormTitle));
                    ThrowException(formTable, GetErrorCells(rowIdx, titles), ERROR_DATATYPE_UNKNOWN);
                    break;
                default:
                    throw new Exception(ERROR_DATATYPE_DEFAULT);
            }
        }

        private void HandleConstruction(DataRow row, int rowIdx)
        {
            int colIdx = GetColumnIdx(constructionColumns[0]);
            string construction = row[colIdx].ToString();

            string SEPARATOR = ":";
            int separatorIdx = construction.IndexOf(SEPARATOR);
            if (separatorIdx == -1)
            {
                string error = string.Format(ERROR_NONE, SEPARATOR);
                ThrowException(formTable, GetErrorCells(rowIdx, colIdx), error);
            }

            string[] name_value = Separate(construction, separatorIdx);
            string name = name_value[0];
            string value = name_value[1];

            string nameTrim = Trim(name);
            if (nameTrim != CONSTRUCTION_NAME)
            {
                string error = string.Format(ERROR_NONE, CONSTRUCTION_NAME);
                ThrowException(formTable, GetErrorCells(rowIdx, colIdx), error);
            }

            if (IsEmpty(value))
            {
                string error = string.Format(ERROR_VALUE_NONE, CONSTRUCTION_NAME);
                ThrowException(formTable, GetErrorCells(rowIdx, colIdx), error);
            }

            SetData(BigTableTitle.WHERE1, value);
        }

        private void HandleBlock(DataRow row, int rowIdx)
        {
            int colIdx = GetColumnIdx(blockColumns[0]);
            string block = row[colIdx].ToString();

            string SEPARATOR = ":";
            int separatorIdx = block.IndexOf(SEPARATOR);
            if (separatorIdx == -1)
            {
                string error = string.Format(ERROR_NONE, SEPARATOR);
                ThrowException(formTable, GetErrorCells(rowIdx, colIdx), error);
            }

            string[] name_value = Separate(block, separatorIdx);
            string name = name_value[0];
            string value = name_value[1];

            string nameTrim = Trim(name);
            if (nameTrim != BLOCK_NAME)
            {
                string error = string.Format(ERROR_NONE, BLOCK_NAME);
                ThrowException(formTable, GetErrorCells(rowIdx, colIdx), error);
            }

            string VALUE_SEPARATOR = "-";
            int valueSeparatorIdx = value.IndexOf(VALUE_SEPARATOR);
            if (valueSeparatorIdx == -1)
            {
                string error = string.Format(ERROR_NONE, VALUE_SEPARATOR);
                ThrowException(formTable, GetErrorCells(rowIdx, colIdx), error);
            }

            string[] blockContainer_what = Separate(value, valueSeparatorIdx);
            string blockContainer = blockContainer_what[0];
            string what = blockContainer_what[1];

            string blockContainerFirst = blockContainer.Substring(0, 1);
            string blockContainerLast = blockContainer.Substring(blockContainer.Length - 1);
            string BLOCK_CONTAINER_FIRST = "[";
            string BLOCK_CONTAINER_LAST = "]";

            if (blockContainerFirst != BLOCK_CONTAINER_FIRST)
            {
                string error = string.Format(ERROR_NONE, BLOCK_CONTAINER_FIRST);
                ThrowException(formTable, GetErrorCells(rowIdx, colIdx), error);
            }

            if (blockContainerLast != BLOCK_CONTAINER_LAST)
            {
                string error = string.Format(ERROR_NONE, BLOCK_CONTAINER_LAST);
                ThrowException(formTable, GetErrorCells(rowIdx, colIdx), error);
            }

            string[] blockContainerRemoves = new string[] { BLOCK_CONTAINER_FIRST, BLOCK_CONTAINER_LAST };
            string blockValue = Remove(blockContainer, blockContainerRemoves);

            if (IsEmpty(blockValue))
            {
                string blockCotainer = BLOCK_CONTAINER_FIRST + BLOCK_CONTAINER_LAST;
                string error = string.Format(ERROR_VALUE_NONE, blockCotainer);
                ThrowException(formTable, GetErrorCells(rowIdx, colIdx), error);
            }

            if (IsEmpty(what))
            {
                string blockWhatSeparator = VALUE_SEPARATOR;
                string error = string.Format(ERROR_VALUE_NONE, blockWhatSeparator);
                ThrowException(formTable, GetErrorCells(rowIdx, colIdx), error);
            }

            SetData(BigTableTitle.WHAT3, what);
        }

        private void HandleFloor(DataRow row, int rowIdx)
        {
            int floorColIdx = GetColumnIdx(FormTitle.Floor);
            int markColIdx = GetColumnIdx(FormTitle.Mark);

            string floorStr = row[floorColIdx].ToString();
            string markStr = row[markColIdx].ToString();

            SetData(BigTableTitle.WHERE2, floorStr);
            SetData(BigTableTitle.WHAT4, markStr);
        }

        private void HandleFloorDetail(DataRow row, int rowIdx)
        {
            int resultColIdx = GetColumnIdx(FormTitle.Result);
            string resultStr = row[resultColIdx].ToString();
            if (IsNumber(resultStr) == false)
            {
                string error = string.Format(ERROR_NUMBER_CHECK, resultStr);
                ThrowException(formTable, GetErrorCells(rowIdx, resultColIdx), error);
            }

            int nameColIdx = GetColumnIdx(FormTitle.Name);
            int standardColIdx = GetColumnIdx(FormTitle.Standard);
            int calculationColIdx = GetColumnIdx(FormTitle.Calculation);

            string nameStr = row[nameColIdx].ToString();
            string standardStr = row[standardColIdx].ToString();
            string calculationStr = row[calculationColIdx].ToString();

            SetData(BigTableTitle.HOW4, nameStr);
            SetData(BigTableTitle.HOW5, standardStr);
            SetData(BigTableTitle.RESULT1, calculationStr);
            SetData(BigTableTitle.RESULT2, resultStr);
            IsAddDataRow = true;
        }
    }
}