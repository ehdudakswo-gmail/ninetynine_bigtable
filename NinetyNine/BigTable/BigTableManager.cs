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

        internal Task<string> Refresh(DataSet dataSet, BigTableManagerState state)
        {
            return Task.Run(() =>
            {
                this.dataSet = dataSet;
                this.state = state;

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

        private void Parsing()
        {
            DataTable bigTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.BigTable);
            DataTable formTable = MainDataTableEnum.FindDataTable(dataSet, MainDataTable.Form);
            BigTableParser formParser = new BigTableParserForm();

            bigTable.Clear();
            formParser.SetTables(bigTable, formTable);
            formParser.SetBigTableTitles();
            formParser.Parse();
        }

        private void SetMappingKeys()
        {

        }

        private void Mapping()
        {

        }
    }
}