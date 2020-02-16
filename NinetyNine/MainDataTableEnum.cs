using NinetyNine.Template;
using NinetyNine.Template.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace NinetyNine
{
    enum MainDataTable
    {
        [Description("산출서")]
        Form,

        [Description("내역서")]
        Statement,

        [Description("공정표")]
        Schedule,

        [Description("조직도")]
        Organization,

        [Description("빅테이블")]
        BigTable,

        [Description("Work")]
        Work,

        [Description("Floor")]
        Floor,

        [Description("What")]
        What,

        [Description("WhatRef")]
        WhatRef,

        [Description("How")]
        How,

        [Description("HowRef")]
        HowRef,

        [Description("Who")]
        Who,
    }

    class MainDataTableEnum
    {
        private static Array values = Enum.GetValues(typeof(MainDataTable));

        internal static Array GetValues()
        {
            return values;
        }

        internal static string GetDescription(MainDataTable value)
        {
            return EnumManager.GetDescription(value);
        }

        internal static List<string> GetAllDescriptions()
        {
            return EnumManager.GetAllDescriptions(values);
        }

        internal static DataSet GetDataSetTemplate()
        {
            ExcelDataManager excelDataManager = ExcelDataManager.GetInstance();
            DataSet dataSet = new DataSet();
            var tables = dataSet.Tables;

            foreach (MainDataTable value in values)
            {
                string tableName = GetDescription(value);
                DataTable dataTable = excelDataManager.GetBasicDataTable(tableName);
                DataTableTemplate template = GetDataTableTemplate(value);
                template.Refresh(dataTable);
                tables.Add(dataTable);
            }

            return dataSet;
        }

        private static DataTableTemplate GetDataTableTemplate(MainDataTable value)
        {
            switch (value)
            {
                case MainDataTable.Form:
                    return new DataTableTemplateForm();
                case MainDataTable.Statement:
                    return new DataTableTemplateStatement();
                case MainDataTable.Schedule:
                    return new DataTableTemplateSchedule();
                case MainDataTable.Organization:
                    return new DataTableTemplateOrganization();
                case MainDataTable.BigTable:
                    return new DataTableTemplateBigTable();
                case MainDataTable.Work:
                    return new DataTableTemplateWork();
                case MainDataTable.Floor:
                    return new DataTableTemplateFloor();
                case MainDataTable.What:
                    return new DataTableTemplateWhat();
                case MainDataTable.WhatRef:
                    return new DataTableTemplateWhatRef();
                case MainDataTable.How:
                    return new DataTableTemplateHow();
                case MainDataTable.HowRef:
                    return new DataTableTemplateHowRef();
                case MainDataTable.Who:
                    return new DataTableTemplateWho();
                default:
                    throw new Exception("MainDataTableEnum GetDataTableTemplate Exception");
            }
        }

        internal static DataTable FindDataTable(DataSet dataSet, MainDataTable target)
        {
            var dataTables = dataSet.Tables;
            string targetTableName = GetDescription(target);

            foreach (DataTable dataTable in dataTables)
            {
                if (dataTable.TableName == targetTableName)
                {
                    return dataTable;
                }
            }

            throw new Exception("FindDataTable Exception");
        }
    }
}