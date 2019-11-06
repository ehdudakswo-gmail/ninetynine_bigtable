using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Dictionary
{
    class BigTableDictionaryWork : BigTableDictionary
    {
        internal override Dictionary<string, DataRow> Create(DataTable dataTable)
        {
            return dictionary;
        }

        internal override void SetTemplate(DataTable bigTable, DataTable dictionaryTable)
        {
        }
    }
}
