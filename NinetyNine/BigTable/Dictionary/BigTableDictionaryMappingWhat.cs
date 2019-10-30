using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary
{
    class BigTableDictionaryMappingWhat : BigTableDictionary
    {
        private Enum[] keys = new Enum[]
        {
            MappingWhatTitle.WHAT4
        };

        internal override void SetTemplate(DataTable bigTable, DataTable dictionaryTable)
        {
            RefreshTemplate(dictionaryTable, new DataTableTemplateMappingWHAT());
            SetKeyTemplate(bigTable, BigTableTitle.WHAT4, dictionaryTable, keys[0]);
        }

        internal override Dictionary<string, DataRow> Create(DataTable dataTable)
        {
            return dictionary;
        }
    }
}