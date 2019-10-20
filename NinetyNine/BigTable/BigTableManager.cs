using NinetyNine.BigTable.Dictionary;
using NinetyNine.BigTable.Mapper;
using NinetyNine.BigTable.Parser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace NinetyNine
{
    internal class BigTableManager
    {
        private readonly string ERROR_EMPTY_SHEET = "{0} SHEET 내용이 필요합니다.";

        private DataSet dataSet;
        private DataTable bigTable;

        internal Task<string> Refresh(DataSet dataSet)
        {
            return Task.Run(() =>
            {
                this.dataSet = dataSet;
                bigTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.BigTable);
                Check();

                bigTable.Clear();
                CreateBigTable();

                return dataSet.ToString();
            });
        }

        private void Check()
        {
            foreach (DataTable dataTable in dataSet.Tables)
            {
                if (dataTable == bigTable)
                {
                    continue;
                }

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

        private void CreateBigTable()
        {
            DataTable formTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Form);
            BigTableParserForm formParser = new BigTableParserForm(bigTable, formTable);
            formParser.Parse();

            DataTable statementMappingTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Mapping_Statement);
            DataTable statementTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Statement);
            BigtableDictionary BigtableDictionaryStatementMapping = new BigtableDictionaryStatementMapping();
            BigtableDictionary BigtableDictionaryStatement = new BigtableDictionaryStatement();
            Dictionary<string, DataRow> statementMappingDictionary = BigtableDictionaryStatementMapping.Create(statementMappingTable);
            Dictionary<string, DataRow> statementDictionary = BigtableDictionaryStatement.Create(statementTable);
            BigTableMapper statementMapper = new BigTableMapperStatement
            {
                bigTable = bigTable,
                statementMappingTable = statementMappingTable,
                statementTable = statementTable,
                statementMappingDictionary = statementMappingDictionary,
                statementDictionary = statementDictionary,
            };
            statementMapper.Mapping();
        }
    }
}