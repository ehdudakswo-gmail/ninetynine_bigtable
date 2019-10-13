using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine
{
    internal class BigTableAutoComplete
    {
        private readonly int WHAT_TABLE_COLUMN_COUNT = 3;
        private readonly int HOW_TABLE_COLUMN_COUNT = 5;

        private DataTable bigTable;
        private DataTable whatTable;
        private DataTable howTable;

        private Dictionary<string, DataRow> whatDictionary = new Dictionary<string, DataRow>();
        private Dictionary<string, DataRow> howDictionary = new Dictionary<string, DataRow>();

        internal BigTableAutoComplete(DataTable bigTable, DataTable whatTable, DataTable howTable)
        {
            this.bigTable = bigTable;
            this.whatTable = whatTable;
            this.howTable = howTable;

            CheckError();
            SetDictionary(whatTable, whatDictionary, WHAT_TABLE_COLUMN_COUNT - 1);
            SetDictionary(howTable, howDictionary, HOW_TABLE_COLUMN_COUNT - 1);
        }

        private void CheckError()
        {

        }

        private void SetDictionary(DataTable dataTable, Dictionary<string, DataRow> dictionary, int lastIdx)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                string lastValue = row[lastIdx].ToString();
                dictionary.Add(lastValue, row);
            }
        }

        internal void Complete()
        {
            for (int idx = 0; idx < bigTable.Rows.Count; idx++)
            {
                Complete(idx, BigTableTitle.WHAT3, whatDictionary, WHAT_TABLE_COLUMN_COUNT);
                Complete(idx, BigTableTitle.HOW5, howDictionary, HOW_TABLE_COLUMN_COUNT);
            }
        }

        private void Complete(int idx, BigTableTitle lastColumnTitle, Dictionary<string, DataRow> dictionary, int columnCnt)
        {
            DataRow bigTableRow = bigTable.Rows[idx];
            int lastIdx = BigTableTitleEnum.GetIndex(lastColumnTitle);
            string lastValue = bigTableRow[lastIdx].ToString();

            if (!dictionary.ContainsKey(lastValue))
            {
                throw new Exception("AUTO COMPLETE ERROR LINE " + (idx + 1));
            }

            DataRow completeRow = dictionary[lastValue];
            int firstIdx = lastIdx - columnCnt + 1;

            for (int i = firstIdx; i <= lastIdx; i++)
            {
                int completeIdx = i - firstIdx;
                string completeValue = completeRow[completeIdx].ToString();
                if (completeValue == null || completeValue == "")
                {
                    continue;
                }

                bigTableRow[i] = completeRow[completeIdx];
            }
        }
    }
}