using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Parser
{
    abstract class BigTableParser
    {
        protected readonly string ERROR_NONE = "값 없음";
        protected readonly string ERROR_FORMAT = "'{0}' 형식 에러";
        protected readonly string ERROR_VALUE_NONE = "{0} 값 없음";
        protected readonly string ERROR_VALUE_ERROR = "{0} 값 에러";
        protected readonly string ERROR_VALUE_NOT_VALID_NUMBER = "{0} 유효 숫자 아님";
        protected readonly string ERROR_DATATYPE_NONE = "알수 없는 형식";
        protected readonly string ERROR_DATATYPE_DEFAULT = "ERROR_DATATYPE_DEFAULT ({0})";

        protected DataTable bigTable;
        protected DataTable formTable;

        protected BigTableData data = new BigTableData();
        protected BigTableError bigTableError = BigTableError.GetInstance();
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

        protected bool CheckFormat(string str, string[] format)
        {
            string trimStr = Trim(str);
            int[] idxs = GetFirstIdxs(trimStr, format);

            if (IsContain(idxs, -1))
            {
                return false;
            }

            if (IsASC(idxs) == false)
            {
                return false;
            }

            return true;
        }

        protected string Trim(string str)
        {
            return str.Replace(" ", "").Trim();
        }

        private int[] GetFirstIdxs(string str, string[] format)
        {
            int len = format.Length;
            int[] idxs = new int[len];

            for (int i = 0; i < len; i++)
            {
                string formatItem = format[i];
                idxs[i] = str.IndexOf(formatItem);
            }

            return idxs;
        }

        private bool IsContain(int[] arr, int target)
        {
            foreach (int item in arr)
            {
                if (item == target)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsASC(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                if (arr[i] >= arr[i + 1])
                {
                    return false;
                }
            }

            return true;
        }

        protected string[] Split(string str, int idx)
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

            return str;
        }

        protected Enum FindNotMatchedEnum(Array values, DataRow row)
        {
            foreach (Enum value in values)
            {
                int colIdx = GetColumnIdx(value);
                string enumString = GetValueString(value);
                string rowString = row[colIdx].ToString();
                string rowStringTrim = Trim(row[colIdx].ToString());

                if (enumString.Equals(rowStringTrim) == false)
                {
                    return value;
                }
            }

            return null;
        }

        protected int GetColumnIdx(Enum value)
        {
            int idx = EnumManager.GetIndex(value);
            return idx;
        }

        protected string GetValueString(Enum value)
        {
            string str = EnumManager.GetDescription(value);
            return str;
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

        protected Enum FindEmptyValue(DataRow row, Enum[] columns)
        {
            foreach (Enum column in columns)
            {
                int colIdx = GetColumnIdx(column);
                string rowStr = row[colIdx].ToString();
                string rowStrTrim = Trim(rowStr);

                if (IsEmpty(rowStrTrim))
                {
                    return column;
                }
            }

            return null;
        }

        protected bool IsNumber(string str)
        {
            double num;
            bool isNumber = double.TryParse(str, out num);

            return isNumber;
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