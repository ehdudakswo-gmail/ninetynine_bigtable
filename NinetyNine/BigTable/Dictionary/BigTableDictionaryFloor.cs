using NinetyNine.Template;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary
{
    class BigTableDictionaryFloor : BigTableDictionary
    {
        private Enum[] keys = new Enum[]
        {
            MappingFloorTitle.Form_Floor,
        };

        internal override void SetTemplate(DataTable bigTable, DataTable dictionaryTable)
        {
            RefreshTemplate(dictionaryTable, new DataTableTemplateMappingFloor());
            SetKeyTemplate(bigTable, BigTableTitle.WHERE4, dictionaryTable, keys[0]);
        }

        internal override Dictionary<string, DataRow> Create(DataTable dataTable)
        {
            return dictionary;
        }
    }
}