
using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary
{
    abstract class BigTableDictionary : BigTableFunction
    {
        protected DataTable dataTable;
        protected DataTableTemplate template;
        protected DataRowCollection rows;

        abstract internal void SetMappingKeys(SortedSet<string[]> sortedKeys);
        abstract internal Dictionary<string, DataRow> Create();

        internal BigTableDictionary(DataTable dataTable, DataTableTemplate template)
        {
            this.dataTable = dataTable;
            this.template = template;
            this.rows = dataTable.Rows;
        }

        protected void RefreshTemplate()
        {
            template.Refresh(dataTable);
        }

        protected void SetMappingKeys(SortedSet<string[]> sortedKeys, Enum[] keys)
        {
            foreach (string[] sortedKey in sortedKeys)
            {
                DataRow row = dataTable.NewRow();
                dataTable.Rows.Add(row);

                int keyLen = keys.Length;
                for (int i = 0; i < keyLen; i++)
                {
                    string keyStr = sortedKey[i];
                    int keyColIdx = GetColumnIdx(keys[i]);
                    row[keyColIdx] = keyStr;
                }
            }
        }
    }
}
