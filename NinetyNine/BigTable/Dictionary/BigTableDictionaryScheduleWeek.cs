using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary
{
    class BigTableDictionaryScheduleWeek : BigTableDictionarySchedule
    {
        private const int CONTENT_ROWIDX = 5;
        private Enum[] keys = new Enum[] { ScheduleTitle.Floor, ScheduleTitle.Description };

        internal BigTableDictionaryScheduleWeek(DataTable dataTable, DataTableTemplate template) : base(dataTable, template)
        {
        }

        internal override void SetMappingKeys(SortedSet<string[]> sortedKeys)
        {
        }

        internal override Dictionary<string, DataRow> Create()
        {
            Dictionary<string, DataRow> dictionary = new Dictionary<string, DataRow>();
            string FLOOR = "";

            for (int rowIdx = 0; rowIdx < rows.Count; rowIdx++)
            {
                if (rowIdx < CONTENT_ROWIDX)
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
    }
}