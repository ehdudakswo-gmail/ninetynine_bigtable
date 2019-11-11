using NinetyNine.BigTable.Dictionary;
using NinetyNine.Template;
using NinetyNine.Template.Mapping;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Mapper
{
    class BigTableMapperSchedule : BigTableMapper
    {
        internal DataTable bigTable { get; set; }
        internal Dictionary<string, DataRow> scheduleDictionary { get; set; }
        internal Dictionary<string, DataRow> floorDictionary { get; set; }
        internal Dictionary<string, DataRow> workDictionary { get; set; }

        internal override void Mapping()
        {
            Enum[] bigTableKeys = new Enum[] { BigTableTitle.WHERE2, BigTableTitle.HOW4, BigTableTitle.HOW5 };
            Enum[] floorKeys = new Enum[] { bigTableKeys[0] };
            Enum[] workKeys = new Enum[] { bigTableKeys[1], bigTableKeys[2] };

            var bigTableRows = bigTable.Rows;
            int bigTableRowsCount = bigTableRows.Count;

            for (int bigTableRowIdx = 0; bigTableRowIdx < bigTableRowsCount; bigTableRowIdx++)
            {
                if (bigTableRowIdx < templateRowsCount)
                {
                }
                else
                {
                    DataRow bigTableRow = bigTableRows[bigTableRowIdx];
                    BigTableErrorCell[] errorCells = GetErrorCells(bigTableRowIdx, bigTableKeys);

                    string floorKey = BigTableDictionary.GetKey(bigTableRow, floorKeys);
                    if (floorDictionary.ContainsKey(floorKey) == false)
                    {
                        ThrowException(bigTable, errorCells, ERROR_KEY_NONE);
                    }

                    string workKey = BigTableDictionary.GetKey(bigTableRow, workKeys);
                    if (workDictionary.ContainsKey(workKey) == false)
                    {
                        ThrowException(bigTable, errorCells, ERROR_KEY_NONE);
                    }

                    DataRow floorRow = floorDictionary[floorKey];
                    DataRow workRow = workDictionary[workKey];

                    string floorRowStr = GetString(floorRow, BigTableFloorTitle.Schedule_Floor);
                    string workRowStr = GetString(workRow, BigTableWorkTitle.Schedule_Description);
                    string schedulekey = GetKey(new string[] { floorRowStr, workRowStr });

                    if (scheduleDictionary.ContainsKey(schedulekey) == false)
                    {
                        ThrowException(bigTable, errorCells, ERROR_KEY_NONE);
                    }

                    DataRow scheduleRow = scheduleDictionary[schedulekey];
                    Mapping(bigTableRow, BigTableTitle.WHEN5, scheduleRow, ScheduleTitle.Plan_Start);
                    Mapping(bigTableRow, BigTableTitle.WHEN6, scheduleRow, ScheduleTitle.Plan_Finish);
                    Mapping(bigTableRow, BigTableTitle.WHEN7, scheduleRow, ScheduleTitle.Actual_Start);
                    Mapping(bigTableRow, BigTableTitle.WHEN8, scheduleRow, ScheduleTitle.Actual_Finish);
                }
            }
        }
    }
}