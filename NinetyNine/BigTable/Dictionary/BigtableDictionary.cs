
using Newtonsoft.Json;
using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary
{
    abstract class BigTableDictionary
    {
        protected readonly string ERROR_FORMAT = "ERROR_FORMAT";
        protected readonly string ERROR_KEY_CONTAINS = "ERROR_KEY_CONTAINS";

        protected readonly string ERROR_VALUE_EMPTY = "값 없음";
        protected readonly string ERROR_KEY_CONTAIN = "값 중복";
        protected readonly string ERROR_NOT_NUMBER = "숫자 아님";

        protected Dictionary<string, DataRow> dictionary = new Dictionary<string, DataRow>();
        protected BigTableError bigTableError = BigTableError.GetInstance();

        abstract internal void SetTemplate(DataTable bigTable, DataTable dictionaryTable);
        abstract internal Dictionary<string, DataRow> Create(DataTable dataTable);

        protected void RefreshTemplate(DataTable dataTable, DataTableTemplate template)
        {
            template.Refresh(dataTable);
        }

        protected void SetKeyTemplate(DataTable bigTable, BigTableTitle bigTableKeyColumn, DataTable dictionaryTable, Enum dictionaryKeyColumn)
        {
            var dictionaryRows = dictionaryTable.Rows;
            int dictionaryColIdx = GetColumnIdx(dictionaryKeyColumn);

            SortedSet<string> sortedBigTableKeys = GetSortedBigTableKeys(bigTable, bigTableKeyColumn);
            foreach (string bigTableKey in sortedBigTableKeys)
            {
                DataRow newRow = dictionaryTable.NewRow();
                dictionaryRows.Add(newRow);
                newRow[dictionaryColIdx] = bigTableKey;
            }
        }

        private SortedSet<string> GetSortedBigTableKeys(DataTable bigTable, BigTableTitle bigTableKeyColumn)
        {
            var comparer = new keyCompare();
            SortedSet<string> sortedKeySet = new SortedSet<string>(comparer);

            var rows = bigTable.Rows;
            int rowCount = rows.Count;
            int colIdx = GetColumnIdx(bigTableKeyColumn);

            for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
            {
                if (rowIdx == 0)
                {
                    //bigTable template
                }
                else
                {
                    string key = rows[rowIdx][colIdx].ToString();
                    sortedKeySet.Add(key);
                }
            }

            return sortedKeySet;
        }

        private class keyCompare : Comparer<string>
        {
            public override int Compare(string aStr, string bStr)
            {
                int aNum;
                int bNum;
                bool isANUM = int.TryParse(aStr, out aNum);
                bool isBNUM = int.TryParse(bStr, out bNum);

                if (isANUM && isBNUM)
                {
                    return aNum - bNum;
                }
                else
                {
                    return string.Compare(aStr, bStr);
                }
            }
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

        protected static string GetKey(string[] keyArr)
        {
            string key = JsonConvert.SerializeObject(keyArr);
            return key;
        }

        protected int GetEmptyIdx(DataRow row, Array enumValues, HashSet<Enum> emptyCheckSkip)
        {
            foreach (Enum enumValue in enumValues)
            {
                if (emptyCheckSkip.Contains(enumValue))
                {
                    continue;
                }

                int colIdx = GetColumnIdx(enumValue);
                string cellValue = row[colIdx].ToString();
                if (IsEmpty(cellValue))
                {
                    return colIdx;
                }
            }

            return -1;
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

        protected int GetColumnIdx(Enum value)
        {
            int idx = EnumManager.GetIndex(value);
            return idx;
        }

        protected bool isNumber(string str)
        {
            double num;
            return double.TryParse(str, out num);
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
