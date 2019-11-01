using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Parser
{
    class BigTableParserForm : BigTableParser
    {
        enum ParserFormDataType
        {
            None,
            Construction,
            ColumnTitle,
            Block,
            Floor,
            FloorDetail,
        }

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

        private bool IsAddDataRow = false;
  
        internal override void Parse()
        {
            var rows = formTable.Rows;
            int rowsCount = rows.Count;

            for (int rowIdx = 0; rowIdx < rowsCount; rowIdx++)
            {
                IsAddDataRow = false;
                DataRow row = rows[rowIdx];
                ParserFormDataType dataType = GetDataType(row, rowIdx);

                SetBigTableData(dataType, row, rowIdx);
                AddDataRow();
            }
        }

        private ParserFormDataType GetDataType(DataRow row, int rowIdx)
        {
            if (rowIdx == 0)
            {
                return ParserFormDataType.Construction;
            }

            if (rowIdx == 1)
            {
                return ParserFormDataType.ColumnTitle;
            }

            Array values = Enum.GetValues(typeof(FormTitle));
            Enum[] validColumns = GetValidColumns(row, values);

            if (IsSame(validColumns, blockColumns))
            {
                return ParserFormDataType.Block;
            }

            if (IsContain(validColumns, floorColumns))
            {
                return ParserFormDataType.Floor;
            }

            if (IsSame(validColumns, floorDetailColumns))
            {
                return ParserFormDataType.FloorDetail;
            }

            return ParserFormDataType.None;
        }

        private void SetBigTableData(ParserFormDataType dataType, DataRow row, int rowIdx)
        {
            switch (dataType)
            {
                case ParserFormDataType.None:
                    Array titles = Enum.GetValues(typeof(FormTitle));
                    ThrowException(formTable, GetErrorCells(rowIdx, titles), string.Format(ERROR_DATATYPE_NONE));
                    break;
                case ParserFormDataType.Construction:
                    HandleConstruction(row, rowIdx);
                    break;
                case ParserFormDataType.ColumnTitle:
                    HandleColumnTitle(row, rowIdx);
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
                default:
                    string exceptionMessage = string.Format(ERROR_DATATYPE_DEFAULT, GetType().Name);
                    throw new Exception(exceptionMessage);
            }
        }

        private void HandleConstruction(DataRow row, int rowIdx)
        {
            int floorColumnIdx = GetColumnIdx(FormTitle.Floor);
            string floorColumnStr = row[floorColumnIdx].ToString();
            string[] CONSTRUCTION_NAME_FORMAT = DataTableTemplateForm.CONSTRUCTION_NAME_FORMAT;
            string formatStr = DataTableTemplateForm.GetFormatString(CONSTRUCTION_NAME_FORMAT);

            if (CheckFormat(floorColumnStr, CONSTRUCTION_NAME_FORMAT) == false)
            {
                string error = string.Format(ERROR_FORMAT, formatStr);
                ThrowException(formTable, GetErrorCells(rowIdx, floorColumnIdx), error);
            }

            int mainSeparatorIdx = floorColumnStr.IndexOf(DataTableTemplateForm.CONSTRUCTION_NAME_SEPARATOR);
            string[] label_value = Split(floorColumnStr, mainSeparatorIdx);
            string label = label_value[0];
            string value = label_value[1];

            string labelTrim = Trim(label);
            string CONSTRUCTION_NAME_LABEL = DataTableTemplateForm.CONSTRUCTION_NAME_LABEL;
            if (labelTrim.Equals(CONSTRUCTION_NAME_LABEL) == false)
            {
                string error = string.Format(ERROR_FORMAT, formatStr);
                ThrowException(formTable, GetErrorCells(rowIdx, floorColumnIdx), error);
            }

            if (IsEmpty(value))
            {
                string error = string.Format(ERROR_VALUE_NONE, CONSTRUCTION_NAME_LABEL);
                ThrowException(formTable, GetErrorCells(rowIdx, floorColumnIdx), error);
            }

            data.Set(BigTableTitle.WHERE1, value);
        }

        private void HandleColumnTitle(DataRow row, int rowIdx)
        {
            Array values = Enum.GetValues(typeof(FormTitle));
            Enum value = FindNotMatchedEnum(values, row);

            if (value != null)
            {
                int colIdx = GetColumnIdx(value);
                string valueString = GetValueString(value);
                string error = string.Format(ERROR_VALUE_NONE, valueString);
                ThrowException(formTable, GetErrorCells(rowIdx, colIdx), error);
            }
        }

        private void HandleBlock(DataRow row, int rowIdx)
        {
            int floorColumnIdx = GetColumnIdx(FormTitle.Floor);
            string floorColumnStr = row[floorColumnIdx].ToString();
            string[] BLOCK_NAME_FORMAT = DataTableTemplateForm.BLOCK_NAME_FORMAT;
            string formatStr = DataTableTemplateForm.GetFormatString(BLOCK_NAME_FORMAT);

            if (CheckFormat(floorColumnStr, BLOCK_NAME_FORMAT) == false)
            {
                string error = string.Format(ERROR_FORMAT, formatStr);
                ThrowException(formTable, GetErrorCells(rowIdx, floorColumnIdx), error);
            }

            int mainSeparatorIdx = floorColumnStr.IndexOf(DataTableTemplateForm.BLOCK_NAME_SEPARATOR);
            string[] label_value = Split(floorColumnStr, mainSeparatorIdx);
            string label = label_value[0];
            string value = label_value[1];

            string labelTrim = Trim(label);
            string BLOCK_NAME_LABEL = DataTableTemplateForm.BLOCK_NAME_LABEL;
            if (labelTrim.Equals(BLOCK_NAME_LABEL) == false)
            {
                string error = string.Format(ERROR_FORMAT, formatStr);
                ThrowException(formTable, GetErrorCells(rowIdx, floorColumnIdx), error);
            }

            int valueSeparatorIdx = value.IndexOf(DataTableTemplateForm.BLOCK_NAME_VALUE_SEPARATOR);
            string[] block_what = Split(value, valueSeparatorIdx);
            string blockContainer = block_what[0];
            string what = block_what[1];

            string blockContainerFirst = blockContainer.Substring(0, 1);
            string blockContainerLast = blockContainer.Substring(blockContainer.Length - 1);
            if (blockContainerFirst.Equals(DataTableTemplateForm.BLOCK_NAME_BLOCK_OPEN) == false ||
                blockContainerLast.Equals(DataTableTemplateForm.BLOCK_NAME_BLOCK_CLOSE) == false)
            {
                string error = string.Format(ERROR_FORMAT, formatStr);
                ThrowException(formTable, GetErrorCells(rowIdx, floorColumnIdx), error);
            }

            string[] blockContainerRemoves = new string[] { blockContainerFirst, blockContainerLast };
            string block = Remove(blockContainer, blockContainerRemoves).Trim();
            if (IsEmpty(block))
            {
                string error = string.Format(ERROR_VALUE_ERROR, BLOCK_NAME_LABEL);
                ThrowException(formTable, GetErrorCells(rowIdx, floorColumnIdx), error);
            }

            if (IsEmpty(what))
            {
                string error = string.Format(ERROR_VALUE_ERROR, BLOCK_NAME_LABEL);
                ThrowException(formTable, GetErrorCells(rowIdx, floorColumnIdx), error);
            }

            int blockSeparatorIdx = block.IndexOf(DataTableTemplateForm.BLOCK_NAME_BLOCK_SEPARATOR);
            if (blockSeparatorIdx == -1)
            {
                data.Set(BigTableTitle.WHERE2, block);
                data.Set(BigTableTitle.WHAT3, what);
                return;
            }

            string[] block_type = Split(block, blockSeparatorIdx);
            string blockWithType = block_type[0];
            string type = block_type[1];

            if (IsEmpty(blockWithType) || IsEmpty(type))
            {
                string error = string.Format(ERROR_VALUE_ERROR, BLOCK_NAME_LABEL);
                ThrowException(formTable, GetErrorCells(rowIdx, floorColumnIdx), error);
            }

            data.Set(BigTableTitle.WHERE2, blockWithType);
            data.Set(BigTableTitle.WHERE3, type);
            data.Set(BigTableTitle.WHAT3, what);
        }

        private void HandleFloor(DataRow row, int rowIdx)
        {
            //data set
            int floorIdx = GetColumnIdx(FormTitle.Floor);
            int markIdx = GetColumnIdx(FormTitle.Mark);

            string floorStr = row[floorIdx].ToString();
            string markStr = row[markIdx].ToString();

            data.Set(BigTableTitle.WHERE4, floorStr);
            data.Set(BigTableTitle.WHAT4, markStr);

            //note check
            int nameIdx = GetColumnIdx(FormTitle.Name);
            string nameStr = row[nameIdx].ToString();
            string nameStrTrim = Trim(nameStr);
            string FLOOR_NOTE = DataTableTemplateForm.FLOOR_NOTE;

            if (nameStrTrim.Equals(FLOOR_NOTE))
            {
                return;
            }

            Enum emptyValue = FindEmptyValue(row, floorDetailColumns);
            if (emptyValue != null)
            {
                int colIdx = GetColumnIdx(emptyValue);
                ThrowException(formTable, GetErrorCells(rowIdx, colIdx), ERROR_NONE);
            }

            HandleFloorDetail(row, rowIdx);
        }

        private void HandleFloorDetail(DataRow row, int rowIdx)
        {
            //check valid number
            int resultIdx = GetColumnIdx(FormTitle.Result);
            string resultStr = row[resultIdx].ToString();
            if (IsNumber(resultStr) == false)
            {
                string error = string.Format(ERROR_VALUE_NOT_VALID_NUMBER, resultStr);
                ThrowException(formTable, GetErrorCells(rowIdx, resultIdx), error);
            }

            //data set
            int nameIdx = GetColumnIdx(FormTitle.Name);
            int standardIdx = GetColumnIdx(FormTitle.Standard);
            int calculationIdx = GetColumnIdx(FormTitle.Calculation);

            string nameStr = row[nameIdx].ToString();
            string standardStr = row[standardIdx].ToString();
            string calculationStr = row[calculationIdx].ToString();

            data.Set(BigTableTitle.HOW4, nameStr);
            data.Set(BigTableTitle.HOW5, standardStr);
            data.Set(BigTableTitle.RESULT1, calculationStr);
            data.Set(BigTableTitle.RESULT2, resultStr);

            IsAddDataRow = true;
        }

        private void AddDataRow()
        {
            if (IsAddDataRow == false)
            {
                return;
            }

            string[] values = data.GetValues();
            DataRow row = bigTable.NewRow();

            for (int i = 0; i < values.Length; i++)
            {
                row[i] = values[i];
            }

            bigTable.Rows.Add(row);
        }
    }
}