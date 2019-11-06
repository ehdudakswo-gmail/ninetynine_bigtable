using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary
{
    enum ScheduleTitle
    {
        ID,
        Floor,
        Description,
        T,
        Plan_Start,
        Plan_Finish,
        Plan_Dur,
        Actual_Start,
        Actual_Finish,
        Actual_Dur,
        Prog,
    }

    class BigTableDictionarySchedule : BigTableDictionary
    {
        private Enum[] keys = new Enum[]
        {
            ScheduleTitle.Floor,
            ScheduleTitle.Description,
        };

        private string FLOOR;

        internal override Dictionary<string, DataRow> Create(DataTable dataTable)
        {
            var rows = dataTable.Rows;
            int rowCount = rows.Count;

            for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
            {
                if (rowIdx < 4)
                {
                    //template check
                }
                else
                {
                    DataRow row = rows[rowIdx];
                    int floorColIdx = GetColumnIdx(ScheduleTitle.Floor);
                    int descriptionColIdx = GetColumnIdx(ScheduleTitle.Description);

                    string floorStr = row[floorColIdx].ToString();
                    string descriptionStr = row[descriptionColIdx].ToString();

                    bool isFloorEmpty = IsEmpty(floorStr);
                    bool isDescriptionEmpty = IsEmpty(descriptionStr);
                    BigTableErrorCell[] errorCells = GetErrorCells(rowIdx, keys);

                    if (isFloorEmpty && isDescriptionEmpty)
                    {
                        ThrowException(dataTable, errorCells, ERROR_FORMAT);
                    }

                    if (isFloorEmpty == false && isDescriptionEmpty == false)
                    {
                        ThrowException(dataTable, errorCells, ERROR_FORMAT);
                    }

                    if (isFloorEmpty == false)
                    {
                        FLOOR = floorStr;
                        continue;
                    }

                    string[] keyArr = new string[] { FLOOR, descriptionStr };
                    string key = GetKey(keyArr);
                    if (dictionary.ContainsKey(key))
                    {
                        ThrowException(dataTable, errorCells, ERROR_KEY_CONTAINS);
                    }

                    dictionary.Add(key, row);
                }
            }

            return dictionary;
        }

        internal override void SetTemplate(DataTable bigTable, DataTable dictionaryTable)
        {
        }
    }
}