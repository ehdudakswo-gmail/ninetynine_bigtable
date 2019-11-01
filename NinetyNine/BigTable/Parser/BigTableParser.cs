using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Parser
{
    abstract class BigTableParser
    {
        protected readonly string ERROR_NONE = "{0} 없음";
        protected readonly string ERROR_VALUE_NONE = "{0} 값 없음";
        protected readonly string ERROR_NUMBER_CHECK = "{0} 유효 숫자 아님";
        protected readonly string ERROR_DATATYPE_UNKNOWN = "ERROR_DATATYPE_UNKNOWN";
        protected readonly string ERROR_DATATYPE_DEFAULT = "ERROR_DATATYPE_DEFAULT";

        protected DataTable bigTable;
        protected DataTable formTable;

        protected BigTableData bigTableData = new BigTableData();
        protected BigTableError bigTableError = BigTableError.GetInstance();
        protected bool IsAddDataRow = false;

        abstract internal void Parse();

        internal void SetTables(DataTable bigTable, DataTable formTable)
        {
            this.bigTable = bigTable;
            this.formTable = formTable;
        }

        internal void SetBigTableTitles()
        {
            Array values = Enum.GetValues(typeof(BigTableTitle));
            List<string> texts = EnumManager.GetTexts(values);

            DataRow row = bigTable.NewRow();
            bigTable.Rows.Add(row);

            for (int i = 0; i < texts.Count; i++)
            {
                row[i] = texts[i];
            }
        }

        protected bool IsStartWith(DataRow row, Enum column, string TARGET)
        {
            int colIdx = GetColumnIdx(column);
            string rowStr = row[colIdx].ToString();
            string rowStrTrim = Trim(rowStr);
            bool isStartWith = rowStrTrim.StartsWith(TARGET);

            return isStartWith;
        }

        protected string Trim(string str)
        {
            return str.Replace(" ", "").Trim();
        }

        protected string[] Separate(string str, int idx)
        {
            string[] ret = new string[2];

            ret[0] = str.Substring(0, idx).Trim();
            ret[1] = str.Substring(idx + 1).Trim();

            return ret;
        }

        protected bool IsEmpty(string str)
        {
            if (str == null)
            {
                return true;
            }

            string trimStr = str.Trim();
            if (trimStr.Equals(""))
            {
                return true;
            }

            return false;
        }

        protected string Remove(string str, string[] removes)
        {
            foreach (string remove in removes)
            {
                str = str.Replace(remove, "");
            }

            string strTrim = str.Trim();
            return strTrim;
        }

        protected int GetColumnIdx(Enum value)
        {
            int idx = EnumManager.GetIndex(value);
            return idx;
        }

        protected Enum[] GetValidColumns(DataRow row, Array values)
        {
            List<Enum> columnList = new List<Enum>();

            foreach (Enum enumValue in values)
            {
                int colIdx = GetColumnIdx(enumValue);
                string rowString = row[colIdx].ToString();
                string rowStringTrim = Trim(rowString);

                if (IsEmpty(rowStringTrim))
                {
                    continue;
                }

                columnList.Add(enumValue);
            }

            Enum[] columns = new Enum[columnList.Count];
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = columnList[i];
            }

            return columns;
        }

        protected bool IsSame(Enum[] a, Enum[] b)
        {
            int aLen = a.Length;
            int bLen = b.Length;

            if (aLen != bLen)
            {
                return false;
            }

            for (int i = 0; i < aLen; i++)
            {
                if (a[i].Equals(b[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        protected bool IsContain(Enum[] a, Enum[] b)
        {
            int aLen = a.Length;
            int bLen = b.Length;

            if (aLen < bLen)
            {
                return false;
            }

            Enum[] cutA = Cut(a, bLen);
            bool isSame = IsSame(cutA, b);

            return isSame;
        }

        private Enum[] Cut(Enum[] arr, int shortLen)
        {
            Enum[] cut = new Enum[shortLen];
            for (int i = 0; i < cut.Length; i++)
            {
                cut[i] = arr[i];
            }

            return cut;
        }

        protected bool IsMatched(Array values, DataRow row)
        {
            foreach (Enum value in values)
            {
                int colIdx = GetColumnIdx(value);
                string rowStr = row[colIdx].ToString();
                string rowStrTrim = Trim(rowStr);
                string description = EnumManager.GetDescription(value);

                if (rowStrTrim != description)
                {
                    return false;
                }
            }

            return true;
        }

        protected bool IsNumber(string str)
        {
            double num;
            bool isNumber = double.TryParse(str, out num);

            return isNumber;
        }

        protected void SetData(BigTableTitle bigTableValue, string str)
        {
            bigTableData.Set(bigTableValue, str);
        }

        protected void AddDataRow()
        {
            if (IsAddDataRow == false)
            {
                return;
            }

            string[] values = bigTableData.GetValues();
            DataRow row = bigTable.NewRow();

            for (int i = 0; i < values.Length; i++)
            {
                row[i] = values[i];
            }

            bigTable.Rows.Add(row);
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

        protected BigTableErrorCell[] GetErrorCells(int rowIdx, int colIdx)
        {
            BigTableErrorCell[] cells = new BigTableErrorCell[]
            {
                new BigTableErrorCell
                {
                    rowIdx = rowIdx,
                    colIdx = colIdx,
                },
            };

            return cells;
        }

        protected BigTableErrorCell[] GetErrorCells(int rowIdx, Array values)
        {
            Enum[] enums = EnumManager.GetEnums(values);
            BigTableErrorCell[] cells = GetErrorCells(rowIdx, enums);

            return cells;
        }

        protected BigTableErrorCell[] GetErrorCells(int rowIdx, Enum[] enums)
        {
            int len = enums.Length;
            BigTableErrorCell[] cells = new BigTableErrorCell[len];

            for (int i = 0; i < len; i++)
            {
                cells[i] = new BigTableErrorCell
                {
                    rowIdx = rowIdx,
                    colIdx = GetColumnIdx(enums[i]),
                };
            }

            return cells;
        }
    }
}