using System;
using System.Collections.Generic;
using System.Data;
using NinetyNine.BigTable.Dictionary;
using NinetyNine.Template;

namespace NinetyNine
{
    internal class BigTableDictionaryStatement : BigTableDictionary
    {
        private Array titles = Enum.GetValues(typeof(StatementTitle));
        private string constructionType;

        internal BigTableDictionaryStatement(DataTable dataTable, DataTableTemplate template) : base(dataTable, template)
        {
        }

        internal override void SetMappingKeys(SortedSet<string[]> sortedKeys)
        {
        }

        internal override Dictionary<string, DataRow> Create()
        {
            Dictionary<string, DataRow> dictionary = new Dictionary<string, DataRow>();

            for (int rowIdx = 0; rowIdx < rows.Count; rowIdx++)
            {
                if (rowIdx < 34)
                {
                }
                else
                {
                    DataRow row = rows[rowIdx];
                    List<Enum> validTitles = GetValidTitles(row, titles);

                    if (validTitles.Count == 0)
                    {
                        continue;
                    }

                    Enum nameTitle = StatementTitle.Name;
                    string nameStr = GetString(row, nameTitle);

                    if (nameStr.EndsWith("계"))
                    {
                        continue;
                    }

                    if (validTitles.Count == 1 && validTitles[0].Equals(nameTitle))
                    {
                        constructionType = nameStr;
                        continue;
                    }

                    if (IsStatement(row) == false)
                    {
                        ThrowException(dataTable, rowIdx, titles, ERROR_ROW);
                    }

                    string standardStr = GetString(row, StatementTitle.Standard);
                    string key = GetKey(new string[] { constructionType, nameStr, standardStr });
                    if (dictionary.ContainsKey(key))
                    {
                        ThrowException(dataTable, rowIdx, titles, ERROR_KEY_CONTAIN);
                    }

                    dictionary.Add(key, row);
                }
            }

            return dictionary;
        }

        private bool IsStatement(DataRow row)
        {
            foreach (Enum title in titles)
            {
                int colIdx = GetColumnIdx(title);
                string str = row[colIdx].ToString();

                switch (title)
                {
                    case StatementTitle.Name:
                        if (IsEmpty(str))
                        {
                            return false;
                        }
                        break;
                    case StatementTitle.Quantity:
                    case StatementTitle.MaterialCost:
                    case StatementTitle.LaborCost:
                    case StatementTitle.ExpenseCost:
                    case StatementTitle.Total:
                        if (IsStatementNumber(str) == false)
                        {
                            return false;
                        }
                        break;
                }
            }

            return true;
        }
    }
}