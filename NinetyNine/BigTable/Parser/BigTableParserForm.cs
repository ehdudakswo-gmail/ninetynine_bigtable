using NinetyNine.Template;
using System;
using System.Data;

namespace NinetyNine.BigTable.Parser
{
    class BigTableParserForm : BigTableParser
    {
        enum FormTitle
        {
            WHERE2,
            WHERE4,
            WHAT3,
            WHAT4,
            HOW4,
            HOW5,
            RESULT1,
            RESULT2,
        }

        enum FormType
        {
            None,
            ErrorRow,
            AddRow,
        }

        private Array titles = Enum.GetValues(typeof(FormTitle));
        private string[] floors = { "지하", "지상", "옥탑" };

        internal override void Parse()
        {
            for (rowIdx = 0; rowIdx < rows.Count; rowIdx++)
            {
                if (rowIdx == 0)
                {
                    HandleWHERE1();
                }
                else if (rowIdx == 1)
                {
                    //title
                }
                else
                {
                    HandleRow();
                }
            }
        }

        private void HandleWHERE1()
        {
            DataRow row = rows[rowIdx];
            int colIdx = GetColumnIdx(FormTitle.WHERE2);
            string rowStr = row[colIdx].ToString();

            if (IsEmpty(rowStr))
            {
                ThrowException(formTable, rowIdx, titles, ERROR_ROW);
            }

            int idx = rowStr.IndexOf(":");
            if (idx == -1)
            {
                ThrowException(formTable, rowIdx, titles, ERROR_ROW);
            }

            string[] nameValue = Split(rowStr, idx);
            string value = nameValue[1];

            if (IsEmpty(value))
            {
                ThrowException(formTable, rowIdx, titles, ERROR_ROW);
            }

            SetData(BigTableTitle.WHERE1, value);
        }

        private void HandleRow()
        {
            DataRow row = rows[rowIdx];

            foreach (Enum title in titles)
            {
                int colIdx = GetColumnIdx(title);
                string str = row[colIdx].ToString();

                if (IsSkip(title, str))
                {
                    continue;
                }

                BigTableTitle bigTableTitle = GetBigTableTitle(title);
                SetData(bigTableTitle, str);
            }

            switch (GetFormType(row))
            {
                case FormType.None:
                    break;
                case FormType.AddRow:
                    AddDataRow();
                    break;
                case FormType.ErrorRow:
                    ThrowException(formTable, rowIdx, titles, ERROR_ROW);
                    break;
            }
        }

        private bool IsSkip(Enum title, string str)
        {
            if (IsEmpty(str))
            {
                return true;
            }

            if (title.Equals(FormTitle.WHERE2))
            {
                return IsSkipFloor(str);
            }

            return false;
        }

        private bool IsSkipFloor(string str)
        {
            foreach (string floor in floors)
            {
                if (str.Contains(floor))
                {
                    return false;
                }
            }

            return true;
        }

        private BigTableTitle GetBigTableTitle(Enum title)
        {
            switch (title)
            {
                case FormTitle.WHERE2:
                    return BigTableTitle.WHERE2;
                case FormTitle.WHERE4:
                    return BigTableTitle.WHERE4;
                case FormTitle.WHAT3:
                    return BigTableTitle.WHAT3;
                case FormTitle.WHAT4:
                    return BigTableTitle.WHAT4;
                case FormTitle.HOW4:
                    return BigTableTitle.HOW4;
                case FormTitle.HOW5:
                    return BigTableTitle.HOW5;
                case FormTitle.RESULT1:
                    return BigTableTitle.RESULT1;
                case FormTitle.RESULT2:
                    return BigTableTitle.RESULT2;
                default:
                    throw new Exception(ERROR_SWITCH_DEFAULT);
            }
        }

        private FormType GetFormType(DataRow row)
        {
            int result1ColIdx = GetColumnIdx(FormTitle.RESULT1);
            int result2ColIdx = GetColumnIdx(FormTitle.RESULT2);

            string result1Str = row[result1ColIdx].ToString();
            string result2Str = row[result2ColIdx].ToString();

            bool isEmptyResult1 = IsEmpty(result1Str);
            bool isEmptyResult2 = IsEmpty(result2Str);

            if (isEmptyResult1 && isEmptyResult2)
            {
                return FormType.None;
            }

            if (isEmptyResult1 == false && isEmptyResult2 == false)
            {
                return FormType.AddRow;
            }

            return FormType.ErrorRow;
        }
    }
}