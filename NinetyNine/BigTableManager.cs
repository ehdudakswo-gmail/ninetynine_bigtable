using System;
using System.Data;

namespace NinetyNine
{
    internal class BigTableManager
    {
        private readonly string ERROR_EMPTY_SHEET = "{0} SHEET 내용이 필요합니다.";
        private readonly string[] titles = {
            "WHERE 1: 프로젝트",
            "WHERE 2: 동",
            "WHERE 3: 타입",
            "WHERE 4: 층 ",
            "WHAT 1: 구조체",
            "WHAT 2: 부재명",
            "WHAT 3: 부재세부명",
            "WHEN 1: 년",
            "WHEN 2: 분기",
            "WHEN 3: 월(Month)",
            "WHEN 4: 주(Week)",
            "WHEN 5: EST",
            "WHEN 5: EFT",
            "HOW 1: 대공종",
            "HOW 2: 중공종",
            "HOW 3: 세공종",
            "HOW 4: 작업",
            "HOW 5: 작업규격",
            "WHO 1: 하도급",
            "WHO 2: 하도급 세부",
            "Result 1: 산출식",
            "Result 2: 결과값",
            "Result 3: 재료비",
            "Result 4: 노무비",
            "Result 5: 경비",
            "Result 6: 단가",
            "Result 7: 금액",
        };

        private ExcelDataManager excelDataManager = new ExcelDataManager();
        private DataSet dataSet;
        private DataTable bigTable;

        internal void Set(DataSet dataSet, DataTable bigTable)
        {
            this.dataSet = dataSet;
            this.bigTable = bigTable;
        }

        internal void Check()
        {
            foreach (DataTable dataTable in dataSet.Tables)
            {
                if (dataTable == bigTable)
                {
                    continue;
                }

                if (isEmpty(dataTable))
                {
                    string tableName = dataTable.TableName;
                    string errorMessage = string.Format(ERROR_EMPTY_SHEET, tableName);
                    throw new Exception(errorMessage);
                }
            }
        }

        private bool isEmpty(DataTable dataTable)
        {
            return (dataTable.Rows.Count == 0);
        }

        internal void Refresh()
        {
            bigTable.Clear();
            SetBigTableTitle();
        }

        private void SetBigTableTitle()
        {
            int columnCnt = titles.Length;
            DataRow titleRow = bigTable.NewRow();

            for (int i = 0; i < columnCnt; i++)
            {
                titleRow[i] = titles[i];
            }
            bigTable.Rows.Add(titleRow);
        }
    }
}