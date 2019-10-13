using System;
using System.Data;

namespace NinetyNine.DataTableTemplate
{
    internal class DataTableTemplateManager
    {
        internal DataSet CreateDataSet()
        {
            DataSet dataSet = new DataSet();

            foreach (MainTabPage value in MainTabPageEnum.GetValues())
            {
                var tables = dataSet.Tables;
                DataTable dataTable = CreateDataTable(value);
                tables.Add(dataTable);
            }

            return dataSet;
        }

        private DataTable CreateDataTable(MainTabPage value)
        {
            DataTableTemplate template = GetTemplate(value);
            string tableName = MainTabPageEnum.GetDescription(value);
            DataTable dataTable = template.Create(tableName);

            return dataTable;
        }

        private DataTableTemplate GetTemplate(MainTabPage value)
        {
            switch (value)
            {
                case MainTabPage.Form:
                    return new DataTableTemplateForm();
                case MainTabPage.Statement:
                    return new DataTableTemplateStatement();
                case MainTabPage.Schedule:
                    return new DataTableTemplateSchedule();
                case MainTabPage.Organization:
                    return new DataTableTemplateOrganization();
                case MainTabPage.BigTable:
                    return new DataTableTemplateBigTable();
                default:
                    throw new Exception("GetTemplate Exception");
            }
        }
    }
}