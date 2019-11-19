using NinetyNine.Template;
using System;
using System.Data;

namespace NinetyNine.BigTable.Parser
{
    class BigTableParserForm : BigTableParser
    {
        private Array titles = Enum.GetValues(typeof(FormTitle));
        private string[] floors = new string[] { "지하", "지상", "옥탑" };

        public BigTableParserForm(DataTable bigTable, DataTable formTable) : base(bigTable, formTable)
        {
            firstRowIdx = 8;
            lastRowIdx = 1356;
        }

        internal override void Parse()
        {
            for (rowIdx = 0; rowIdx < rows.Count; rowIdx++)
            {
                DataRow row = rows[rowIdx];
                if (rowIdx >= firstRowIdx && rowIdx <= lastRowIdx)
                {
                    SetData(row);
                    AddDataRow(row);
                }
                else
                {
                    if (rowIdx == 2)
                    {
                        SetConstruction(row);
                    }
                }
            }
        }

        private void SetConstruction(DataRow row)
        {
            string[] strArr =
            {
                 GetString(row, FormTitle.NO),
                 GetString(row, FormTitle.구분FWC),
            };

            if (strArr[0].StartsWith("공사명") == false)
            {
                ThrowException(formTable, rowIdx, titles, ERROR_ROW);
            }

            if (IsEmpty(strArr[1]))
            {
                ThrowException(formTable, rowIdx, titles, ERROR_ROW);
            }

            SetData(BigTableTitle.WHERE1, strArr[1]);
        }

        private void SetData(DataRow row)
        {
            foreach (FormTitle title in titles)
            {
                string str = GetString(row, title);
                if (IsEmpty(str))
                {
                    continue;
                }

                if (title.Equals(FormTitle.NO) && IsFloor(str) == false)
                {
                    continue;
                }

                if (title.Equals(FormTitle.수량자동계산) && IsNumber(str) == false)
                {
                    ThrowException(formTable, rowIdx, titles, ERROR_ROW);
                }

                BigTableTitle bigTableTitle = (BigTableTitle)GetBigTableTitle(title);
                SetData(bigTableTitle, str);
            }
        }

        private bool IsFloor(string str)
        {
            foreach (string floor in floors)
            {
                if (str.StartsWith(floor))
                {
                    return true;
                }
            }

            return false;
        }

        private Enum GetBigTableTitle(FormTitle formTitle)
        {
            switch (formTitle)
            {
                case FormTitle.NO:
                    return BigTableTitle.WHERE2;
                case FormTitle.부위:
                    return BigTableTitle.WHERE4;
                case FormTitle.구분FWC:
                    return BigTableTitle.WHAT3;
                case FormTitle.구분:
                    return BigTableTitle.WHAT4;
                case FormTitle.재료:
                    return BigTableTitle.HOW4;
                case FormTitle.규격:
                    return BigTableTitle.HOW5;
                case FormTitle.산식:
                    return BigTableTitle.RESULT1;
                case FormTitle.수량자동계산:
                    return BigTableTitle.RESULT2;
                default:
                    return null;
            }
        }

        private void AddDataRow(DataRow row)
        {
            string[] strArr =
            {
                 GetString(row, FormTitle.구분FWC),
                 GetString(row, FormTitle.구분),
                 GetString(row, FormTitle.재료),
                 GetString(row, FormTitle.규격),
                 GetString(row, FormTitle.산식),
                 GetString(row, FormTitle.수량자동계산),
            };

            if (IsAllEmpty(strArr))
            {
                return;
            }

            if (IsLeastOneEmpty(strArr))
            {
                ThrowException(formTable, rowIdx, titles, ERROR_ROW);
            }

            AddDataRow();
        }
    }
}