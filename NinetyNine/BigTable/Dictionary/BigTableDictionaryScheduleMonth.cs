using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary
{
    class BigTableDictionaryScheduleMonth : BigTableDictionarySchedule
    {
        internal BigTableDictionaryScheduleMonth(DataTable dataTable, DataTableTemplate template) : base(dataTable, template)
        {
        }

        internal override void SetMappingKeys(SortedSet<string[]> sortedKeys)
        {
        }

        internal override Dictionary<string, DataRow> Create()
        {
            Dictionary<string, DataRow> dictionary = new Dictionary<string, DataRow>();
            Enum[] keys = new Enum[] { ScheduleTitle.Floor, ScheduleTitle.Description };

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

                    if (isDescriptionEmpty)
                    {
                        ThrowException(dataTable, errorCells, ERROR_FORMAT);
                    }

                    if (isFloorEmpty)
                    {
                        continue;
                    }

                    string key = GetKey(new string[] { floorStr });
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
