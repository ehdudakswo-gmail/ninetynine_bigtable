using NinetyNine.BigTable.Dictionary;
using NinetyNine.Template;
using NinetyNine.Template.Mapping;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Mapper
{
    class BigTableMapperScheduleMonth : BigTableMapperSchedule
    {
        internal BigTableMapperScheduleMonth(DataTable bigTable) : base(bigTable)
        {
        }

        internal DateTime basicDateTime { get; set; }
        internal Dictionary<string, DataRow> scheduleDictionary { get; set; }
        internal Dictionary<string, DataRow> floorDictionary { get; set; }

        internal override void Mapping()
        {
            Enum[] bigTableKeys = new Enum[] { BigTableTitle.WHERE2 };
            Enum[] floorKeys = new Enum[] { bigTableKeys[0] };

            for (int rowIdx = 0; rowIdx < bigTableRowsCount; rowIdx++)
            {
                if (rowIdx < CONTENT_ROWIDX)
                {
                }
                else
                {
                    DataRow bigTableRow = bigTableRows[rowIdx];
                    BigTableErrorCell[] errorCells = GetErrorCells(rowIdx, bigTableKeys);

                    string floorKey = BigTableDictionary.GetKey(bigTableRow, floorKeys);
                    if (floorDictionary.ContainsKey(floorKey) == false)
                    {
                        ThrowException(bigTable, errorCells, ERROR_KEY_NONE);
                    }

                    DataRow floorRow = floorDictionary[floorKey];
                    string floorRowStr = GetString(floorRow, FloorTitle.Schedule_Floor);
                    string schedulekey = GetKey(new string[] { floorRowStr });

                    if (scheduleDictionary.ContainsKey(schedulekey) == false)
                    {
                        ThrowException(bigTable, errorCells, ERROR_KEY_NONE);
                    }

                    DataRow scheduleRow = scheduleDictionary[schedulekey];
                    DateTime dateTime = GetDateTime(scheduleRow, ScheduleTitle.Plan_Start);

                    Mapping(bigTableRow, BigTableTitle.WHEN1, GetYear(dateTime));
                    Mapping(bigTableRow, BigTableTitle.WHEN2, GetQuarter(dateTime));
                    Mapping(bigTableRow, BigTableTitle.WHEN3, GetMonth(dateTime));
                    Mapping(bigTableRow, BigTableTitle.WHEN4, GetWeekDiff(dateTime, basicDateTime));
                    Mapping(bigTableRow, BigTableTitle.WHEN5, scheduleRow, ScheduleTitle.Plan_Start);
                    Mapping(bigTableRow, BigTableTitle.WHEN6, scheduleRow, ScheduleTitle.Plan_Finish);
                    Mapping(bigTableRow, BigTableTitle.WHEN7, scheduleRow, ScheduleTitle.Actual_Start);
                    Mapping(bigTableRow, BigTableTitle.WHEN8, scheduleRow, ScheduleTitle.Actual_Finish);
                }
            }
        }
    }
}