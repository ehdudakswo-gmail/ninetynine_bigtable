using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Parser
{
    class BigTableParserForm : BigTableParser
    {
        enum FormType
        {
            Block,
            Floor,
            Work,
            Unknown,
        }

        private Array titles = Enum.GetValues(typeof(FormTitle));
        private readonly string BLOCK = "동 명";
        private readonly string NOTE = "[ 비 고 ]";

        internal override void Parse()
        {
            for (rowIdx = 0; rowIdx < rows.Count; rowIdx++)
            {
                if (rowIdx == 0)
                {
                    //subject
                }
                else if (rowIdx == 1)
                {
                    DataRow row = rows[rowIdx];
                    SetConstruction(row);
                }
                else if (rowIdx == 2)
                {
                    //titles
                }
                else
                {
                    DataRow row = rows[rowIdx];
                    FormType type = GetFormType(row);

                    switch (GetFormType(row))
                    {
                        case FormType.Block:
                            SetBlock(row);
                            break;
                        case FormType.Floor:
                            SetFloor(row);
                            break;
                        case FormType.Work:
                            SetWork(row);
                            AddDataRow();
                            break;
                        default:
                            ThrowException(formTable, rowIdx, titles, ERROR_ROW);
                            break;
                    }
                }
            }
        }

        private void SetConstruction(DataRow row)
        {
            //공사명 : 평택시 포승읍 도곡 카임하우스 신축
            string str = GetString(row, FormTitle.층);
            int idx = str.IndexOf(":");
            if (idx == -1)
            {
                ThrowException(formTable, rowIdx, titles, ERROR_ROW);
            }

            string[] name_value = Split(str, idx);
            string construction = name_value[1];
            if (IsEmpty(construction))
            {
                ThrowException(formTable, rowIdx, titles, ERROR_ROW);
            }

            SetData(BigTableTitle.WHERE1, construction);
        }

        private FormType GetFormType(DataRow row)
        {
            string[] strArr = new string[]
            {
                GetString(row, FormTitle.층),
                GetString(row, FormTitle.명칭),
            };

            if (strArr[0].StartsWith(BLOCK))
            {
                return FormType.Block;
            }

            if (strArr[1].Equals(NOTE))
            {
                return FormType.Floor;
            }

            List<Enum> validTitles = GetValidTitles(row, titles);
            List<Enum> workTitles = new List<Enum>() { FormTitle.명칭, FormTitle.규격, FormTitle.산출식, FormTitle.결과값 };
            if (IsSame(validTitles, workTitles))
            {
                return FormType.Work;
            }

            return FormType.Unknown;
        }

        private void SetBlock(DataRow row)
        {
            //동 명 : [도생주+근생] - 기초
            string str = GetString(row, FormTitle.층);
            int idx = str.IndexOf(":");
            if (idx == -1)
            {
                ThrowException(formTable, rowIdx, titles, ERROR_ROW);
            }

            string[] name_value = Split(str, idx);
            string value = name_value[1];
            int dashIdx = value.IndexOf("-");
            if (dashIdx == -1)
            {
                ThrowException(formTable, rowIdx, titles, ERROR_ROW);
            }

            string[] block_what = Split(value, dashIdx);
            string what = block_what[1];
            if (IsEmpty(what))
            {
                ThrowException(formTable, rowIdx, titles, ERROR_ROW);
            }

            SetData(BigTableTitle.WHAT3, what);
        }

        private void SetFloor(DataRow row)
        {
            string[] strArr = new string[] {
                GetString(row, FormTitle.층),
                GetString(row, FormTitle.부호),
            };

            if (IsEmpty(strArr))
            {
                ThrowException(formTable, rowIdx, titles, ERROR_ROW);
            }

            SetData(BigTableTitle.WHERE2, strArr[0]);
            SetData(BigTableTitle.WHAT4, strArr[1]);
        }

        private void SetWork(DataRow row)
        {
            string[] strArr = new string[] {
                GetString(row, FormTitle.명칭),
                GetString(row, FormTitle.규격),
                GetString(row, FormTitle.산출식),
                GetString(row, FormTitle.결과값),
            };

            if (IsEmpty(strArr))
            {
                ThrowException(formTable, rowIdx, titles, ERROR_ROW);
            }

            SetData(BigTableTitle.HOW4, strArr[0]);
            SetData(BigTableTitle.HOW5, strArr[1]);
            SetData(BigTableTitle.RESULT1, strArr[2]);
            SetData(BigTableTitle.RESULT2, strArr[3]);
        }
    }
}