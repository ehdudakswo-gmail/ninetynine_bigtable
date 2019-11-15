
using Newtonsoft.Json;
using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary
{
    abstract class BigTableDictionary
    {
        protected readonly string ERROR_ROW = "ERROR_ROW";
        protected readonly string ERROR_KEY_CONTAIN = "ERROR_KEY_CONTAIN";
        protected readonly string ERROR_FORMAT_DATETIME = "ERROR_FORMAT_DATETIME";

        protected readonly string ERROR_FORMAT = "ERROR_FORMAT";
        protected readonly string ERROR_EMPTY = "ERROR_EMPTY";
        protected readonly string ERROR_NOT_NUMBER = "ERROR_NOT_NUMBER";

        protected readonly string ERROR_VALUE_EMPTY = "값 없음";

        protected Dictionary<string, DataRow> dictionary = new Dictionary<string, DataRow>();
        protected BigTableError bigTableError = BigTableError.GetInstance();

        protected DataTable dataTable;
        protected DataTableTemplate template;
        protected DataRowCollection rows;
        protected int templateRowsCount;

        abstract internal void SetMappingKeys(SortedSet<string[]> sortedKeys);
        abstract internal Dictionary<string, DataRow> Create();

        internal BigTableDictionary(DataTable dataTable, DataTableTemplate template)
        {
            this.dataTable = dataTable;
            this.template = template;

            rows = dataTable.Rows;
            templateRowsCount = template.GetTemplateDataTable().Rows.Count;
        }

        protected void RefreshTemplate()
        {
            template.Refresh(dataTable);
        }

        protected void SetMappingKeys(SortedSet<string[]> sortedKeys, Enum[] keys)
        {
            foreach (string[] sortedKey in sortedKeys)
            {
                DataRow row = dataTable.NewRow();
                dataTable.Rows.Add(row);

                int keyLen = keys.Length;
                for (int i = 0; i < keyLen; i++)
                {
                    string keyStr = sortedKey[i];
                    int keyColIdx = GetColumnIdx(keys[i]);
                    row[keyColIdx] = keyStr;
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

        private bool IsNumber(string str)
        {
            double num;
            return double.TryParse(str, out num);
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
