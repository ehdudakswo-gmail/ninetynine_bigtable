using NinetyNine.BigTable;
using NinetyNine.BigTable.Dictionary;
using NinetyNine.BigTable.Dictionary.Mapping;
using NinetyNine.BigTable.Mapper;
using NinetyNine.BigTable.Parser;
using NinetyNine.Template;
using NinetyNine.Template.Mapping;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace NinetyNine
{
    enum BigTableManagerState
    {
        None,
        Unknown,
        Parsing,
        Mapping,
    }

    internal class BigTableManager
    {
        private readonly string ERROR_STATE_DEFUALT = "BigTableManagerState default";
        private readonly string ERROR_EMPTY_SHEET = "{0} SHEET 내용이 필요합니다.";

        private BigTableManagerState state = BigTableManagerState.None;
        private DataSet dataSet;

        private DataTable bigTable;
        private DataTable formTable;
        private DataTable statementTable;
        private DataTable scheduleTable;
        private DataTable workTable;
        private DataTable floorTable;
        private DataTable whatTable;
        private DataTable howTable;
        private DataTable whoTable;

        private BigTableDictionary statementBigTableDictionary;
        private BigTableDictionarySchedule scheduleBigTableDictionary;
        private BigTableDictionary workBigTableDictionary;
        private BigTableDictionary floorBigTableDictionary;
        private BigTableDictionary whatBigTableDictionary;
        private BigTableDictionary howBigTableDictionary;
        private BigTableDictionary whoBigTableDictionary;

        internal Task<string> Refresh(DataSet dataSet, BigTableManagerState state)
        {
            return Task.Run(() =>
            {
                this.dataSet = dataSet;
                this.state = state;

                SetDataTable();
                SetDictionary();
                HandleState();

                return dataSet.ToString();
            });
        }

        private void SetDataTable()
        {
            bigTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.BigTable);
            formTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Form);
            statementTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Statement);
            scheduleTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Schedule);
            workTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Work);
            floorTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Floor);
            whatTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.What);
            howTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.How);
            whoTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Who);
        }

        private void SetDictionary()
        {
            statementBigTableDictionary = new BigTableDictionaryStatement(statementTable, new DataTableTemplateStatement());
            scheduleBigTableDictionary = new BigTableDictionaryScheduleWeek(scheduleTable, new DataTableTemplateSchedule());
            workBigTableDictionary = new BigTableDictionaryWork(workTable, new DataTableTemplateWork());
            floorBigTableDictionary = new BigTableDictionaryFloor(floorTable, new DataTableTemplateFloor());
            whatBigTableDictionary = new BigTableDictionaryWhat(whatTable, new DataTableTemplateWhat());
            howBigTableDictionary = new BigTableDictionaryHow(howTable, new DataTableTemplateHow());
            whoBigTableDictionary = new BigTableDictionaryWho(whoTable, new DataTableTemplateWho());
        }

        private void HandleState()
        {
            switch (state)
            {
                case BigTableManagerState.Parsing:
                    CheckDataSet();
                    SetBigTableTemplate();
                    Parsing();
                    SetMappingKeys();
                    break;
                case BigTableManagerState.Mapping:
                    Mapping();
                    break;
                default:
                    throw new Exception(ERROR_STATE_DEFUALT);
            }
        }

        private void CheckDataSet()
        {
            foreach (DataTable dataTable in dataSet.Tables)
            {
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

        private void SetBigTableTemplate()
        {
            DataTableTemplate template = new DataTableTemplateBigTable();
            template.Refresh(bigTable);
        }

        private void Parsing()
        {
            BigTableParser formParser = new BigTableParserForm(bigTable, formTable);
            formParser.Parse();
        }

        private void SetMappingKeys()
        {
            DataTableTemplate template = new DataTableTemplateBigTable();
            DataTable templateDataTable = template.GetTemplateDataTable();
            int templateRowsCount = templateDataTable.Rows.Count;
            int bigTableRowsCount = bigTable.Rows.Count;

            IComparer<string[]> workComparer = new BigTableMappingKeyComparer.WorkComparer();
            IComparer<string[]> floorComparer = new BigTableMappingKeyComparer.FloorComparer();
            IComparer<string[]> whatComparer = floorComparer;
            IComparer<string[]> howComparer = workComparer;

            SortedSet<string[]> workSortedKeys = new SortedSet<string[]>(workComparer);
            SortedSet<string[]> floorSortedKeys = new SortedSet<string[]>(floorComparer);
            SortedSet<string[]> whatSortedKeys = new SortedSet<string[]>(whatComparer);
            SortedSet<string[]> howSortedKeys = new SortedSet<string[]>(howComparer);

            for (int rowIdx = 0; rowIdx < bigTableRowsCount; rowIdx++)
            {
                if (rowIdx < templateRowsCount)
                {
                }
                else
                {
                    DataRow row = bigTable.Rows[rowIdx];
                    string workName = GetString(row, BigTableTitle.HOW4);
                    string workStandard = GetString(row, BigTableTitle.HOW5);
                    string floor = GetString(row, BigTableTitle.WHERE2);

                    workSortedKeys.Add(new string[] { workName, workStandard });
                    floorSortedKeys.Add(new string[] { floor });
                    whatSortedKeys.Add(new string[] { floor });
                    howSortedKeys.Add(new string[] { workName, workStandard });
                }
            }

            workBigTableDictionary.SetMappingKeys(workSortedKeys);
            floorBigTableDictionary.SetMappingKeys(floorSortedKeys);
            whatBigTableDictionary.SetMappingKeys(whatSortedKeys);
            howBigTableDictionary.SetMappingKeys(howSortedKeys);
        }

        private string GetString(DataRow row, Enum title)
        {
            int idx = EnumManager.GetIndex(title);
            string str = row[idx].ToString();

            return str;
        }

        private void Mapping()
        {
            Dictionary<string, DataRow> statementDictionary = statementBigTableDictionary.Create();
            Dictionary<string, DataRow> scheduleDictionary = scheduleBigTableDictionary.Create();
            DateTime basicDateTime = scheduleBigTableDictionary.GetBasicDateTime();

            Dictionary<string, DataRow> workDictionary = workBigTableDictionary.Create();
            Dictionary<string, DataRow> floorDictionary = floorBigTableDictionary.Create();
            Dictionary<string, DataRow> whatDictionary = whatBigTableDictionary.Create();
            Dictionary<string, DataRow> howDictionary = howBigTableDictionary.Create();
            Dictionary<string, DataRow> whoDictionary = whoBigTableDictionary.Create();

            BigTableMapper statementMapper = new BigTableMapperStatement
            {
                bigTable = bigTable,
                statementDictionary = statementDictionary,
                workDictionary = workDictionary,
            };
            BigTableMapper scheduleMapper = new BigTableMapperScheduleWeek
            {
                bigTable = bigTable,
                basicDateTime = basicDateTime,
                scheduleDictionary = scheduleDictionary,
                floorDictionary = floorDictionary,
                workDictionary = workDictionary,
            };
            //BigTableMapper scheduleMapper = new BigTableMapperScheduleMonth
            //{
            //    bigTable = bigTable,
            //    basicDateTime = basicDateTime,
            //    scheduleDictionary = scheduleDictionary,
            //    floorDictionary = floorDictionary,
            //};
            BigTableMapper whatMapper = new BigTableMapperWhat
            {
                bigTable = bigTable,
                whatDictionary = whatDictionary,
            };
            BigTableMapper howMapper = new BigTableMapperHow
            {
                bigTable = bigTable,
                howDictionary = howDictionary,
            };
            BigTableMapper whoMapper = new BigTableMapperWho
            {
                bigTable = bigTable,
                whoDictionary = whoDictionary,
            };

            statementMapper.Mapping();
            scheduleMapper.Mapping();
            whatMapper.Mapping();
            howMapper.Mapping();
            whoMapper.Mapping();
        }
    }
}