using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary
{
    class BigTableDictionarySchedule : BigTableDictionary
    {
        internal BigTableDictionarySchedule(DataTable dataTable, DataTableTemplate template) : base(dataTable, template)
        {
        }

        internal override void SetMappingKeys(SortedSet<string[]> sortedKeys)
        {
        }

        internal override Dictionary<string, DataRow> Create()
        {
            Enum[] keys = new Enum[] { ScheduleTitle.Floor, ScheduleTitle.Description };
            string FLOOR = "";

            for (int rowIdx = 0; rowIdx < rows.Count; rowIdx++)
            {
                if (rowIdx < 5)
                {
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

                    string key = GetKey(new string[] { FLOOR, descriptionStr });
                    if (dictionary.ContainsKey(key))
                    {
                        ThrowException(dataTable, errorCells, ERROR_KEY_CONTAIN);
                    }

                    dictionary.Add(key, row);
                }
            }

            return dictionary;
        }

        internal DateTime GetBasicDateTime()
        {
            int rowIdx = 2;
            int colIdx = 0;
            string str = rows[rowIdx][colIdx].ToString();

            DateTime dateTime;
            bool IsParse = DateTime.TryParse(str, out dateTime);
            if (IsParse == false)
            {
                BigTableErrorCell[] errorCells = GetErrorCells(rowIdx, colIdx);
                ThrowException(dataTable, errorCells, ERROR_FORMAT_DATETIME);
            }

            return dateTime;
        }
    }
}