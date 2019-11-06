using NinetyNine.BigTable.Dictionary;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Mapper
{
    class BigTableMapperSchedule : BigTableMapper
    {
        internal DataTable bigTable { get; set; }
        internal Dictionary<string, DataRow> workDictionary { get; set; }
        internal Dictionary<string, DataRow> floorDictionary { get; set; }
        internal Dictionary<string, DataRow> scheduleDictionary { get; set; }

        internal override void Mapping()
        {
            foreach (var dictionary in scheduleDictionary)
            {
                string key = dictionary.Key;
                DataRow row = dictionary.Value;
                int colIdx = GetColumnIdx(ScheduleTitle.Actual_Start);

                Console.WriteLine("{0} | {1}", key, row[colIdx]);
            }
        }
    }
}