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
        Unknown,
        Parsing,
        Mapping,
    }

    internal class BigTableManager
    {
        private readonly string ERROR_STATE_DEFUALT = "BigTableManagerState default";
        private readonly string ERROR_EMPTY_SHEET = "{0} SHEET 내용이 필요합니다.";

        private BigTableManagerState state = BigTableManagerState.Unknown;
        private DataSet dataSet;
        private DataTable bigTable;

        internal Task<string> Refresh(BigTableManagerState state, DataSet dataSet)
        {
            return Task.Run(() =>
            {
                this.state = state;
                this.dataSet = dataSet;
                bigTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.BigTable);

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
                    bigTable.Clear();
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
            DataTable formTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Form);
            BigTableParserForm formParser = new BigTableParserForm(bigTable, formTable);
            formParser.Parse();
        }

        private void SetMappingTemplate()
        {
            DataTable mappingStatementTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Mapping_Statement);
            DataTable mappingWhatTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Mapping_WHAT);

            BigTableDictionary mappingStatementBigTableDictionary = new BigTableDictionaryMappingStatement();
            BigTableDictionary mappingWhatBigTableDictionary = new BigTableDictionaryMappingWhat();

            mappingStatementBigTableDictionary.SetTemplate(bigTable, mappingStatementTable);
            mappingWhatBigTableDictionary.SetTemplate(bigTable, mappingWhatTable);
        }

        private void Mapping()
        {
            DataTable mappingStatementTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Mapping_Statement);
            DataTable statementTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Statement);

            BigTableDictionary mappingStatementBigTableDictionary = new BigTableDictionaryMappingStatement();
            BigTableDictionary statementBigTableDictionary = new BigTableDictionaryStatement();

            Dictionary<string, DataRow> mappingStatementDictionary = mappingStatementBigTableDictionary.Create(mappingStatementTable);
            Dictionary<string, DataRow> statementDictionary = statementBigTableDictionary.Create(statementTable);

            BigTableMapper statementMapper = new BigTableMapperStatement
            {
                bigTable = bigTable,
                mappingStatementTable = mappingStatementTable,
                statementTable = statementTable,
                mappingStatementDictionary = mappingStatementDictionary,
                statementDictionary = statementDictionary,
            };
            statementMapper.Mapping();
        }
    }
}