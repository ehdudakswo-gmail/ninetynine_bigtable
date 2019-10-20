using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Parser
{
    enum ParserFormState
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

    class BigTableParserForm : BigTableParser
    {
        private readonly string ERROR_MESSAGE_FORMAT = "빅테이블 에러 : {0} LINE {1}";

        private ParserFormState state;
        private BigTableData data = new BigTableData();
        private DataTable bigTable;
        private DataTable formTable;

        internal BigTableParserForm(DataTable bigTable, DataTable formTable)
        {
            this.bigTable = bigTable;
            this.formTable = formTable;
            SetBigTableTitle();
        }

        private void SetBigTableTitle()
        {
            Array values = Enum.GetValues(typeof(BigTableTitle));
            List<string> texts = EnumManager.GetTexts(values);

            DataRow row = bigTable.NewRow();
            bigTable.Rows.Add(row);

            for (int i = 0; i < texts.Count; i++)
            {
                row[i] = texts[i];
            }
        }

        internal override void Parse()
        {
            for (int idx = 0; idx < formTable.Rows.Count; idx++)
            {
                DataRow row = formTable.Rows[idx];
                state = ParserFormState.None;

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
                    HandleDataColumns(row);
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
            if (state != ParserFormState.None)
            {
                return;
            }

            string row0 = row[0].ToString();
            string row0Trim = Trim(row0);

            if (row0Trim != DataTableTemplateForm.SUBJECT)
            {
                return;
            }

            state = ParserFormState.Subject;
        }

        private void HandleDataColumns(DataRow row)
        {
            if (state != ParserFormState.None)
            {
                return;
            }

            string[] dataColumns = DataTableTemplateForm.DATA_COLUMNS;
            for (int i = 0; i < dataColumns.Length; i++)
            {
                string typeString = dataColumns[i];
                string rowString = row[i].ToString();
                string rowStringTrim = Trim(rowString);

                if (typeString != rowStringTrim)
                {
                    return;
                }
            }

            state = ParserFormState.DataType;
        }

        private void HandleData(DataRow row)
        {
            if (state != ParserFormState.None)
            {
                return;
            }

            string row0 = row[0].ToString();
            string row0Trim = Trim(row0);
            if (!row0Trim.Contains(DataTableTemplateForm.DATA_TITLE_LEFT))
            {
                return;
            }

            int titleSplitIdx = row0.IndexOf(DataTableTemplateForm.DATA_TITLE_SEPARATOR);
            if (titleSplitIdx == -1)
            {
                return;
            }

            string[] titles = Split(row0, titleSplitIdx);
            string titleRight = titles[1];
            string where1 = titleRight;

            if (isEmpty(titleRight))
            {
                return;
            }

            data.Set(BigTableTitle.WHERE1, where1);
            state = ParserFormState.Data;
        }

        private void HandleData2(DataRow row)
        {
            if (state != ParserFormState.None)
            {
                return;
            }

            string row0 = row[0].ToString();
            string row0Trim = Trim(row0);
            if (!row0Trim.Contains(DataTableTemplateForm.DATA2_TITLE_LEFT))
            {
                return;
            }

            int titleSplitIdx = row0.IndexOf(DataTableTemplateForm.DATA2_TITLE_SEPARATOR);
            if (titleSplitIdx == -1)
            {
                return;
            }

            string[] titles = Split(row0, titleSplitIdx);
            string titleRight = titles[1];

            int contentSplitIdx = titleRight.IndexOf(DataTableTemplateForm.DATA2_CONTENT_SEPARATOR);
            if (contentSplitIdx == -1)
            {
                return;
            }

            string[] contents = Split(titleRight, contentSplitIdx);
            string contentLeftWithContainer = contents[0];
            string openContainer = DataTableTemplateForm.DATA2_CONTENT_SUB_CONTAINER_OPEN;
            string closeContainer = DataTableTemplateForm.DATA2_CONTENT_SUB_CONTAINER_CLOSE;

            if (!contentLeftWithContainer.Contains(openContainer))
            {
                return;
            }

            if (!contentLeftWithContainer.Contains(closeContainer))
            {
                return;
            }

            string removes = openContainer + closeContainer;
            string contentLeft = Remove(contentLeftWithContainer, removes);
            string contentRight = contents[1];

            int contentSubSplitIdx = contentLeft.IndexOf(DataTableTemplateForm.DATA2_CONTENT_SUB_SEPARATOR);
            if (contentSubSplitIdx == -1)
            {
                string where2 = contentLeft;
                string what3 = contentRight;
                data.Set(BigTableTitle.WHERE2, where2);
                data.Set(BigTableTitle.WHAT3, what3);
            }
            else
            {
                string[] contentSubs = Split(contentLeft, contentSubSplitIdx);
                string contentSubLeft = contentSubs[0];
                string contentSubRight = contentSubs[1];

                string where2 = contentSubLeft;
                string where3 = contentSubRight;
                string what3 = contentRight;

                data.Set(BigTableTitle.WHERE2, where2);
                data.Set(BigTableTitle.WHERE3, where3);
                data.Set(BigTableTitle.WHAT3, what3);
            }

            state = ParserFormState.Data2;
        }

        private void HandleData3(DataRow row)
        {
            if (state != ParserFormState.None)
            {
                return;
            }

            if (isLeastOneEmpty(row, 0, 1))
            {
                return;
            }

            string[] values = GetStringValues(row, 0, 1);
            string where4 = values[0];
            string what4 = values[1];

            data.Set(BigTableTitle.WHERE4, where4);
            data.Set(BigTableTitle.WHAT4, what4);
            state = ParserFormState.Data3;
        }

        private void HandleData4(DataRow row)
        {
            if (state != ParserFormState.None &&
                state != ParserFormState.Data3)
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
            state = ParserFormState.Data4;
        }

        private void HandleEmpty(DataRow row)
        {
            if (state != ParserFormState.None)
            {
                return;
            }

            if (!isEmpty(row, formTable.Columns.Count))
            {
                return;
            }

            state = ParserFormState.Empty;
        }

        private void CheckError(int rowIdx)
        {
            if (state == ParserFormState.None)
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
            if (state != ParserFormState.Data4)
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