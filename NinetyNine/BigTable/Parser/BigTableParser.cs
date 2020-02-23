using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Parser
{
    abstract class BigTableParser
    {
        protected readonly string ERROR_ROW = "ERROR_ROW";
        protected BigTableData bigTableData = new BigTableData();
        protected BigTableError bigTableError = BigTableError.GetInstance();
        protected DataTable bigTable;

        abstract internal void Parse();

        public BigTableParser(DataTable bigTable)
        {
            this.bigTable = bigTable;
        }

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
                string str = GetString(row, title);
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

        protected void SetData(BigTableTitle bigTableValue, string str)
        {
            bigTableData.Set(bigTableValue, str);
        }

        protected void AddDataRow()
        {
            string[] values = bigTableData.GetValues();
            DataRow row = bigTable.NewRow();

            for (int i = 0; i < values.Length; i++)
            {
                row[i] = values[i];
            }

            bigTable.Rows.Add(row);
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
        private BigTableErrorCell[] GetErrorCells(int rowIdx, Array values)
        {
            Enum[] enums = EnumManager.GetEnums(values);
            BigTableErrorCell[] cells = GetErrorCells(rowIdx, enums);

            return cells;
        }

        private BigTableErrorCell[] GetErrorCells(int rowIdx, Enum[] enums)
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