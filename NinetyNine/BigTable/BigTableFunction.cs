using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable
{
    class BigTableFunction
    {
        protected BigTableError bigTableError = BigTableError.GetInstance();
        protected readonly string ERROR_ROW = "ERROR_ROW";
        protected readonly string ERROR_KEY_CONTAIN = "ERROR_KEY_CONTAIN";
        protected readonly string ERROR_FORMAT_DATETIME = "ERROR_FORMAT_DATETIME";
        protected readonly string ERROR_FORMAT = "ERROR_FORMAT";
        protected readonly string ERROR_EMPTY = "ERROR_EMPTY";
        protected readonly string ERROR_NOT_NUMBER = "ERROR_NOT_NUMBER";
        protected readonly string ERROR_VALUE_EMPTY = "ERROR_VALUE_EMPTY";
        protected readonly string ERROR_KEY_NONE = "ERROR_KEY_NONE";
        protected readonly string ERROR_DATA_NONE = "{0} ERROR_DATA_NONE";

        protected string GetString(DataRow row, Enum title)
        {
            int colIdx = GetColumnIdx(title);
            string str = row[colIdx].ToString();

            return str;
        }

        protected int GetColumnIdx(Enum value)
        {
            int idx = EnumManager.GetIndex(value);
            return idx;
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

        protected bool IsAllEmpty(string[] strArr)
        {
            foreach (string str in strArr)
            {
                if (IsEmpty(str) == false)
                {
                    return false;
                }
            }

            return true;
        }

        protected bool IsLeastOneEmpty(string[] strArr)
        {
            foreach (string str in strArr)
            {
                if (IsEmpty(str))
                {
                    return true;
                }
            }

            return false;
        }

        protected bool IsEmpty(DataTable dataTable, DataRow row)
        {
            int colCnt = dataTable.Columns.Count;
            for (int colIdx = 0; colIdx < colCnt; colIdx++)
            {
                string value = row[colIdx].ToString();
                if (IsEmpty(value) == false)
                {
                    return false;
                }
            }

            return true;
        }

        protected bool IsNumber(string str)
        {
            double num;
            bool IsNumber = double.TryParse(str, out num);

            return IsNumber;
        }

        protected List<Enum> GetValidTitles(DataRow row, Array titles)
        {
            List<Enum> validTitles = new List<Enum>();

            foreach (Enum title in titles)
            {
                int colIdx = GetColumnIdx(title);
                string str = row[colIdx].ToString();

                if (IsEmpty(str))
                {
                    continue;
                }

                validTitles.Add(title);
            }

            return validTitles;
        }

        protected bool IsSame(List<Enum> a, List<Enum> b)
        {
            if (a.Count != b.Count)
            {
                return false;
            }

            int count = a.Count;
            for (int i = 0; i < count; i++)
            {
                if (a[i].Equals(b[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        internal static string GetKey(DataRow row, Enum[] keys)
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

        internal static string GetKey(string[] keyArr)
        {
            string key = JsonConvert.SerializeObject(keyArr);
            return key;
        }

        internal static string[] GetKeyArr(string data)
        {
            string[] keyData = JsonConvert.DeserializeObject<string[]>(data);
            return keyData;
        }

        protected bool IsStatementNumber(string str)
        {
            if (IsNumber(str))
            {
                return true;
            }

            if (str.Equals("-"))
            {
                return true;
            }

            return false;
        }

        protected void ThrowException(DataTable dataTable, int rowIdx, Array titles, string error)
        {
            BigTableErrorData errrorData = new BigTableErrorData
            {
                dataTable = dataTable,
                cells = GetErrorCells(rowIdx, titles),
                error = error,
            };

            bigTableError.ThrowException(errrorData);
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

        private BigTableErrorCell[] GetErrorCells(int rowIdx, Array values)
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
    }
}