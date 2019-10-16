using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable
{
    internal class BigTableAutoComplete
    {
        internal DataTable bigTable { get; set; }
        internal DataTable formTable { get; set; }
        internal DataTable autoCompleteTable { get; set; }

        private BigTableError bigTableError = BigTableError.GetInstance();
        private Dictionary<string, DataRow> whatDictionary = new Dictionary<string, DataRow>();
        private Dictionary<string, DataRow> howDictionary = new Dictionary<string, DataRow>();

        private readonly BigTableTitle whatStart = BigTableTitle.WHAT1;
        private readonly BigTableTitle whatEnd = BigTableTitle.WHAT4;
        private readonly BigTableTitle whatIgnore = BigTableTitle.WHAT3;

        private readonly BigTableTitle howStart = BigTableTitle.HOW1;
        private readonly BigTableTitle howEnd = BigTableTitle.HOW5;
        private readonly BigTableTitle howIgnore = BigTableTitle.HOW4;

        internal void Start()
        {
            SetDictionary();
            CompleteBigTable();
        }

        private void SetDictionary()
        {
            var rows = autoCompleteTable.Rows;
            int count = rows.Count;

            for (int i = 0; i < count; i++)
            {
                if (i == 0)
                {
                    DataTableTemplateAutoComplete template = new DataTableTemplateAutoComplete();
                    template.CheckError(autoCompleteTable);
                }
                else
                {
                    DataRow row = rows[i];
                    AddDictionary(whatDictionary, row, i);
                    AddDictionary(howDictionary, row, i);
                }
            }
        }

        private void AddDictionary(Dictionary<string, DataRow> dictionary, DataRow dictionaryRow, int rowIdx)
        {
            string key = GetKey(dictionary, dictionaryRow);

            if (isEmpty(key))
            {
                return;
            }

            if (dictionary.ContainsKey(key))
            {
                int keyIdx = GetEndIdx(dictionary);
                string error = string.Format("{0} 중복", key);
                bigTableError.ThrowException(autoCompleteTable, rowIdx, keyIdx, error);
            }

            int startIdx = GetStartIdx(dictionary);
            int endIdx = GetEndIdx(dictionary);
            int ignoreIdx = GetIgnoreIdx(dictionary);

            for (int i = startIdx; i <= endIdx; i++)
            {
                if (i == ignoreIdx)
                {
                    continue;
                }

                string dictionaryValue = dictionaryRow[i].ToString();
                if (isEmpty(dictionaryValue))
                {
                    string error = string.Format("값 없음");
                    bigTableError.ThrowException(autoCompleteTable, rowIdx, i, error);
                }
            }

            dictionary.Add(key, dictionaryRow);
        }

        private bool isEmpty(string str)
        {
            return (str == null || str == "");
        }

        private string GetKey(Dictionary<string, DataRow> dictionary, DataRow row)
        {
            if (dictionary == whatDictionary)
            {
                int idx = BigTableTitleEnum.GetIndex(whatEnd);
                return row[idx].ToString();
            }
            else if (dictionary == howDictionary)
            {
                int idx = BigTableTitleEnum.GetIndex(howEnd);
                return row[idx].ToString();
            }
            else
            {
                throw new Exception("BigTableAutoComplete GetKey Exception");
            }
        }

        private void CompleteBigTable()
        {
            var rows = bigTable.Rows;
            int count = rows.Count;
            int startIdx = 1;

            for (int i = startIdx; i < count; i++)
            {
                DataRow row = rows[i];
                CompleteBigTable(whatDictionary, row, i);
                CompleteBigTable(howDictionary, row, i);
            }
        }

        private void CompleteBigTable(Dictionary<string, DataRow> dictionary, DataRow bigTableRow, int rowIdx)
        {
            string key = GetKey(dictionary, bigTableRow);
            int keyIdx = GetEndIdx(dictionary);

            if (isEmpty(key))
            {
                string error = string.Format("값 없음");
                bigTableError.ThrowException(bigTable, rowIdx, keyIdx, error);
            }

            if (!dictionary.ContainsKey(key))
            {
                string error = string.Format("{0} 자동완성 데이터 없음", key);
                bigTableError.ThrowException(bigTable, rowIdx, keyIdx, error);
            }

            DataRow dictionaryRow = dictionary[key];
            int startIdx = GetStartIdx(dictionary);
            int endIdx = GetEndIdx(dictionary);
            int ignoreIdx = GetIgnoreIdx(dictionary);

            for (int i = startIdx; i <= endIdx; i++)
            {
                if (i == ignoreIdx)
                {
                    continue;
                }

                string dictionaryValue = dictionaryRow[i].ToString();
                bigTableRow[i] = dictionaryValue;
            }
        }

        private int GetStartIdx(Dictionary<string, DataRow> dictionary)
        {
            if (dictionary == whatDictionary)
            {
                return BigTableTitleEnum.GetIndex(whatStart);
            }
            else if (dictionary == howDictionary)
            {
                return BigTableTitleEnum.GetIndex(howStart);
            }
            else
            {
                throw new Exception("BigTableAutoComplete GetStartIdx Exception");
            }
        }

        private int GetEndIdx(Dictionary<string, DataRow> dictionary)
        {
            if (dictionary == whatDictionary)
            {
                return BigTableTitleEnum.GetIndex(whatEnd);
            }
            else if (dictionary == howDictionary)
            {
                return BigTableTitleEnum.GetIndex(howEnd);
            }
            else
            {
                throw new Exception("BigTableAutoComplete GetEndIdx Exception");
            }
        }

        private int GetIgnoreIdx(Dictionary<string, DataRow> dictionary)
        {
            if (dictionary == whatDictionary)
            {
                return BigTableTitleEnum.GetIndex(whatIgnore);
            }
            else if (dictionary == howDictionary)
            {
                return BigTableTitleEnum.GetIndex(howIgnore);
            }
            else
            {
                throw new Exception("BigTableAutoComplete GetIgnoreIdx Exception");
            }
        }
    }
}