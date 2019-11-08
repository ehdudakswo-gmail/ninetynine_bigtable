using Newtonsoft.Json;
using NinetyNine.Template;
using System;
using System.Data;

namespace NinetyNine.BigTable.Mapper
{
    abstract class BigTableMapper
    {
        protected readonly string ERROR_EMPTY = "ERROR_EMPTY";
        protected readonly string ERROR_KEY_CONTAIN = "ERROR_KEY_CONTAIN";
        protected readonly string ERROR_KEY_NONE = "ERROR_KEY_NONE";
        protected readonly string ERROR_DATA_NONE = "{0} 데이터 없음";

        protected BigTableError bigTableError = BigTableError.GetInstance();
        protected int templateRowsCount;

        abstract internal void Mapping();

        internal BigTableMapper()
        {
            DataTableTemplate bigTableTemplate = new DataTableTemplateBigTable();
            templateRowsCount = bigTableTemplate.GetTemplateDataTable().Rows.Count;
        }

        protected int GetColumnIdx(Enum value)
        {
            int idx = EnumManager.GetIndex(value);
            return idx;
        }

        protected bool IsEmpty(string str)
        {
            if (str == null)
            {
                return true;
            }

            string trim = str.Trim();
            if (trim == "")
            {
                return true;
            }

            return false;
        }

        protected string GetKey(DataRow row, Enum[] keys)
        {
            int len = keys.Length;
            string[] keyArr = new string[len];

            for (int i = 0; i < len; i++)
            {
                Enum enumValue = keys[i];
                int columnIdx = EnumManager.GetIndex(enumValue);
                keyArr[i] = row[columnIdx].ToString();
            }

            string key = GetKey(keyArr);
            return key;
        }

        protected string GetKey(string[] keyArr)
        {
            string key = JsonConvert.SerializeObject(keyArr);
            return key;
        }

        protected string GetString(DataRow row, Enum title)
        {
            int colIdx = GetColumnIdx(title);
            string str = row[colIdx].ToString();

            return str;
        }

        protected int GetRowIndex(DataTable dataTable, DataRow row)
        {
            int rowIdx = dataTable.Rows.IndexOf(row);
            return rowIdx;
        }

        protected void Mapping(DataRow bigTableRow, Enum bigTableTitle, DataRow mappingRow, Enum mappingTitle)
        {
            int bigTableColIdx = GetColumnIdx(bigTableTitle);
            int mappingColIdx = GetColumnIdx(mappingTitle);

            bigTableRow[bigTableColIdx] = mappingRow[mappingColIdx];
        }

        protected void ThrowException(DataTable dataTable, BigTableErrorCell[] cells, string error)
        {
            BigTableErrorData errrorData = new BigTableErrorData
            {
                dataTable = dataTable,
                cells = cells,
                error = error,
            };

            bigTableError.ThrowException(errrorData);
        }


        protected BigTableErrorCell[] GetErrorCells(int rowIdx, Enum[] keys)
        {
            int len = keys.Length;
            BigTableErrorCell[] cells = new BigTableErrorCell[len];

            for (int i = 0; i < len; i++)
            {
                cells[i] = new BigTableErrorCell
                {
                    rowIdx = rowIdx,
                    colIdx = GetColumnIdx(keys[i]),
                };
            }

            return cells;
        }
    }
}