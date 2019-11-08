
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
        protected readonly string ERROR_EMPTY = "ERROR_EMPTY";
        protected readonly string ERROR_KEY_CONTAIN = "ERROR_KEY_CONTAIN";
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
