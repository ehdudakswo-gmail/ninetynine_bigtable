using NinetyNine.BigTable.Dictionary;
using NinetyNine.BigTable.Mapper;
using NinetyNine.BigTable.Parser;
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

        private DataSet dataSet;
        private BigTableManagerState state = BigTableManagerState.None;
        private BigTableParser parser;

        internal Task<string> Refresh(DataSet dataSet, BigTableManagerState state, BigTableParser parser)
        {
            return Task.Run(() =>
            {
                this.dataSet = dataSet;
                this.state = state;
                this.parser = parser;

                HandleState();
                return dataSet.ToString();
            });
        }

        private void HandleState()
        {
            switch (state)
            {
                case BigTableManagerState.Parsing:
                    CheckDataSet();
                    Parsing();
                    SetMappingTemplate();
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

        private void Parsing()
        {
            DataTable bigTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.BigTable);
            DataTable formTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Form);

            bigTable.Clear();
            parser.SetTables(bigTable, formTable);
            parser.SetBigTableTitles();
            parser.Parse();
        }

        private void SetMappingTemplate()
        {
            DataTable bigTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.BigTable);
            DataTable workTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Mapping_Work);
            DataTable floorTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Mapping_Floor);
            DataTable whatTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Mapping_WHAT);

            BigTableDictionary mappingStatementBigTableDictionary = new BigTableDictionaryMappingStatement();
            BigTableDictionary floorBigTableDictionary = new BigTableDictionaryFloor();
            BigTableDictionary whatBigTableDictionary = new BigTableDictionaryMappingWhat();

            mappingStatementBigTableDictionary.SetTemplate(bigTable, workTable);
            floorBigTableDictionary.SetTemplate(bigTable, floorTable);
            whatBigTableDictionary.SetTemplate(bigTable, whatTable);
        }

        private void Mapping()
        {
            DataTable bigTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.BigTable);
            DataTable statementTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Statement);
            DataTable scheduleTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Schedule);
            DataTable mappingStatementTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Mapping_Work);
            DataTable workTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Mapping_Work);

            BigTableDictionary mappingStatementBigTableDictionary = new BigTableDictionaryMappingStatement();
            BigTableDictionary statementBigTableDictionary = new BigTableDictionaryStatement();
            BigTableDictionary workBigTableDictionary = new BigTableDictionaryWork();
            BigTableDictionary floorBigTableDictionary = new BigTableDictionaryFloor();
            BigTableDictionary scheduleBigTableDictionary = new BigTableDictionarySchedule();

            Dictionary<string, DataRow> mappingStatementDictionary = mappingStatementBigTableDictionary.Create(mappingStatementTable);
            Dictionary<string, DataRow> statementDictionary = statementBigTableDictionary.Create(statementTable);
            Dictionary<string, DataRow> workDictionary = workBigTableDictionary.Create(workTable);
            Dictionary<string, DataRow> floorDictionary = floorBigTableDictionary.Create(workTable);
            Dictionary<string, DataRow> scheduleDictionary = scheduleBigTableDictionary.Create(scheduleTable);

            BigTableMapper statementMapper = new BigTableMapperStatement
            {
                bigTable = bigTable,
                mappingStatementTable = mappingStatementTable,
                statementTable = statementTable,
                mappingStatementDictionary = mappingStatementDictionary,
                statementDictionary = statementDictionary,
            };

            BigTableMapper scheduleMapper = new BigTableMapperSchedule
            {
                bigTable = bigTable,
                workDictionary = workDictionary,
                floorDictionary = floorDictionary,
                scheduleDictionary = scheduleDictionary,
            };

            statementMapper.Mapping();
            scheduleMapper.Mapping();
        }
    }
}