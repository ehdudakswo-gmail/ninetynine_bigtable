
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary
{
    abstract class BigtableDictionary
    {
        protected readonly string ERROR_VALUE_EMPTY = "값 없음";
        protected readonly string ERROR_KEY_CONTAIN = "{0} 중복";
        protected readonly string ERROR_NOT_NUMBER = "{0} 숫자 아님";

        protected Dictionary<string, DataRow> dictionary = new Dictionary<string, DataRow>();
        protected BigTableError bigTableError = BigTableError.GetInstance();

        abstract internal Dictionary<string, DataRow> Create(DataTable dataTable);

        protected int GetColumnCount(Array titles)
        {
            int columnCount = EnumManager.GetLength(titles);
            return columnCount;
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

            string key = JsonConvert.SerializeObject(keyArr);
            return key;
        }

        protected int GetKeyIndex(Enum[] keys)
        {
            int idx = EnumManager.GetIndex(keys[0]);
            return idx;
        }

        protected int GetEmptyIdx(DataRow row, int startIdx, int endIdx)
        {
            for (int i = startIdx; i <= endIdx; i++)
            {
                string value = row[i].ToString();
                if (isEmpty(value))
                {
                    return i;
                }
            }

            return -1;
        }

        private bool isEmpty(string value)
        {
            return (value == null || value.Equals(""));
        }

        protected void CheckNumber(Enum enumValue, DataRow row, int rowIdx, DataTable dataTable)
        {
            int columnIdx = EnumManager.GetIndex(enumValue);
            string cellValue = row[columnIdx].ToString();

            if (isNumber(cellValue))
            {
                return;
            }

            string error = string.Format(ERROR_NOT_NUMBER, cellValue);
            bigTableError.ThrowException(dataTable, rowIdx, columnIdx, error);
        }

        private bool isNumber(string str)
        {
            double num;
            return double.TryParse(str, out num);
        }
    }
}