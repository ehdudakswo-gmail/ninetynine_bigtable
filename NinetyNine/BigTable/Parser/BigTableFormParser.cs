using System;
using System.Data;
using System.Text;

namespace NinetyNine.BigTable.Parser
{

    enum FormParserState
    {
        None,
        Subject,
        DataType,
        Data,
        Data2,
        Data3,
        Data4,
        Empty,
    }

    class BigTableFormParser
    {
        private readonly string SUBJECT = "부재별산출서";
        private readonly string[] DATA_TYPE = new string[]
        { "층", "부호", "명칭", "규격", "산출식", "결과값" };

        private readonly string DATA_TITLE_LEFT = "공사명";
        private readonly string DATA_TITLE_SEPARATOR = ":";

        private readonly string DATA2_TITLE_LEFT = "동명";
        private readonly string DATA2_TITLE_SEPARATOR = ":";
        private readonly string DATA2_CONTENT_SEPARATOR = "-";
        private readonly string DATA2_CONTENT_SUB_SEPARATOR = ":";

        private readonly string ERROR_MESSAGE_FORMAT = "빅테이블 에러 : {0} LINE {1}";

        private FormParserState state;
        private BigTableData data = new BigTableData();
        private DataTable bigTable;
        private DataTable formTable;

        internal BigTableFormParser(DataTable bigTable, DataTable formTable)
        {
            this.bigTable = bigTable;
            this.formTable = formTable;
        }

        internal void Parse()
        {
            for (int idx = 0; idx < formTable.Rows.Count; idx++)
            {
                DataRow row = formTable.Rows[idx];
                state = FormParserState.None;

                if (idx == 0)
                {
                    HandleSubject(row);
                }
                else if (idx == 1)
                {
                    HandleData(row);
                }
                else if (idx == 2)
                {
                    HandleDataType(row);
                }
                else
                {
                    HandleData2(row);
                    HandleData3(row);
                    HandleData4(row);
                }

                HandleEmpty(row);
                CheckError(idx);
                AddRow();
            }
        }

        private void HandleSubject(DataRow row)
        {
            if (state != FormParserState.None)
            {
                return;
            }

            string row0 = row[0].ToString();
            string row0Trim = Trim(row0);

            if (row0Trim != SUBJECT)
            {
                return;
            }

            state = FormParserState.Subject;
        }

        private void HandleDataType(DataRow row)
        {
            if (state != FormParserState.None)
            {
                return;
            }

            for (int i = 0; i < DATA_TYPE.Length; i++)
            {
                string typeString = DATA_TYPE[i];
                string rowString = row[i].ToString();
                string rowStringTrim = Trim(rowString);

                if (typeString != rowStringTrim)
                {
                    return;
                }
            }

            state = FormParserState.DataType;
        }

        private void HandleData(DataRow row)
        {
            if (state != FormParserState.None)
            {
                return;
            }

            string row0 = row[0].ToString();
            string row0Trim = Trim(row0);
            if (!row0Trim.Contains(DATA_TITLE_LEFT))
            {
                return;
            }

            int titleSplitIdx = row0.IndexOf(DATA_TITLE_SEPARATOR);
            if (titleSplitIdx == -1)
            {
                return;
            }

            string[] titles = Split(row0, titleSplitIdx);
            string titleRight = titles[1];
            string where1 = titleRight;

            data.Set(BigTableTitle.WHERE1, where1);
            state = FormParserState.Data;
        }

        private void HandleData2(DataRow row)
        {
            if (state != FormParserState.None)
            {
                return;
            }

            string row0 = row[0].ToString();
            string row0Trim = Trim(row0);
            if (!row0Trim.Contains(DATA2_TITLE_LEFT))
            {
                return;
            }

            int titleSplitIdx = row0.IndexOf(DATA2_TITLE_SEPARATOR);
            if (titleSplitIdx == -1)
            {
                return;
            }

            string[] titles = Split(row0, titleSplitIdx);
            string titleRight = titles[1];

            int contentSplitIdx = titleRight.IndexOf(DATA2_CONTENT_SEPARATOR);
            if (contentSplitIdx == -1)
            {
                return;
            }

            string[] contents = Split(titleRight, contentSplitIdx);
            string contentLeft = Remove(contents[0], "[]");
            string contentRight = contents[1];

            int contentSubSplitIdx = contentLeft.IndexOf(DATA2_CONTENT_SUB_SEPARATOR);
            if (contentSubSplitIdx == -1)
            {
                return;
            }

            string[] contentSubs = Split(contentLeft, contentSubSplitIdx);
            string contentSubLeft = contentSubs[0];
            string contentSubRight = contentSubs[1];

            string where2 = contentSubLeft;
            string where3 = contentSubRight;
            string what2 = contentRight;

            data.Set(BigTableTitle.WHERE2, where2);
            data.Set(BigTableTitle.WHERE3, where3);
            data.Set(BigTableTitle.WHAT2, what2);
            state = FormParserState.Data2;
        }

        private void HandleData3(DataRow row)
        {
            if (state != FormParserState.None)
            {
                return;
            }

            if (isLeastOneEmpty(row, 0, 1))
            {
                return;
            }

            string[] values = GetStringValues(row, 0, 1);
            string where4 = values[0];
            string what3 = values[1];

            data.Set(BigTableTitle.WHERE4, where4);
            data.Set(BigTableTitle.WHAT3, what3);
            state = FormParserState.Data3;
        }

        private void HandleData4(DataRow row)
        {
            if (state != FormParserState.None &&
                state != FormParserState.Data3)
            {
                return;
            }

            if (isLeastOneEmpty(row, 2, 5))
            {
                return;
            }

            string[] values = GetStringValues(row, 2, 5);
            string how4 = values[0];
            string how5 = values[1];
            string result1 = values[2];
            string result2 = values[3];

            data.Set(BigTableTitle.HOW4, how4);
            data.Set(BigTableTitle.HOW5, how5);
            data.Set(BigTableTitle.RESULT1, result1);
            data.Set(BigTableTitle.RESULT2, result2);
            state = FormParserState.Data4;
        }

        private void HandleEmpty(DataRow row)
        {
            if (state != FormParserState.None)
            {
                return;
            }

            if (!isEmpty(row))
            {
                return;
            }

            state = FormParserState.Empty;
        }


        private string Trim(string str)
        {
            return str.Replace(" ", "").Trim();
        }

        private string[] Split(string str, int idx)
        {
            string[] ret = new string[2];

            ret[0] = str.Substring(0, idx).Trim();
            ret[1] = str.Substring(idx + 1).Trim();

            return ret;
        }

        private string Remove(string str, string removes)
        {
            StringBuilder sb = new StringBuilder(str.Length);

            foreach (char ch in str)
            {
                if (isContain(removes, ch))
                {
                    continue;
                }

                sb.Append(ch);
            }

            return sb.ToString();
        }

        private bool isContain(string str, char target)
        {
            foreach (char ch in str)
            {
                if (ch == target)
                {
                    return true;
                }
            }

            return false;
        }

        private bool isLeastOneEmpty(DataRow row, int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                string str = row[i].ToString();
                if (str == "")
                {
                    return true;
                }
            }

            return false;
        }

        private string[] GetStringValues(DataRow row, int from, int to)
        {
            string[] values = new string[to - from + 1];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = row[from + i].ToString();
            }

            return values;
        }

        private bool isEmpty(DataRow row)
        {
            for (int i = 0; i < formTable.Columns.Count; i++)
            {
                if (row[i].ToString() != "")
                {
                    return false;
                }
            }

            return true;
        }

        private void CheckError(int rowIdx)
        {
            if (state == FormParserState.None)
            {
                string tableName = formTable.TableName;
                int line = rowIdx + 1;
                string errorMessage = string.Format(ERROR_MESSAGE_FORMAT, tableName, line);

                bigTable.Clear();
                throw new Exception(errorMessage);
            }
        }

        private void AddRow()
        {
            if (state != FormParserState.Data4)
            {
                return;
            }

            DataRow row = bigTable.NewRow();
            string[] values = data.GetValues();

            for (int i = 0; i < values.Length; i++)
            {
                row[i] = values[i];
            }

            bigTable.Rows.Add(row);
        }
    }
}