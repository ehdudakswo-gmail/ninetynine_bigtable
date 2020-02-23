using NinetyNine.BigTable.Dictionary;
using NinetyNine.Template;
using NinetyNine.Template.Mapping;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Mapper
{
    class BigTableMapperWho : BigTableMapper
    {
        internal BigTableMapperWho(DataTable bigTable) : base(bigTable)
        {
        }

        internal Dictionary<string, DataRow> whoDictionary { get; set; }

        internal override void Mapping()
        {
            Enum[] bigTableKeys = new Enum[] { BigTableTitle.HOW3 };
            Enum[] whoKeys = new Enum[] { bigTableKeys[0] };

            for (int rowIdx = 0; rowIdx < bigTableRowsCount; rowIdx++)
            {
                if (rowIdx < CONTENT_ROWIDX)
                {
                }
                else
                {
                    DataRow bigTableRow = bigTableRows[rowIdx];
                    BigTableErrorCell[] errorCells = GetErrorCells(rowIdx, bigTableKeys);

                    string whoKey = BigTableDictionary.GetKey(bigTableRow, whoKeys);
                    if (whoDictionary.ContainsKey(whoKey) == false)
                    {
                        ThrowException(bigTable, errorCells, ERROR_KEY_NONE);
                    }

                    DataRow whoRow = whoDictionary[whoKey];
                    Mapping(bigTableRow, BigTableTitle.WHO1, whoRow, WhoTitle.Organization_WHO1);
                    Mapping(bigTableRow, BigTableTitle.WHO2, whoRow, WhoTitle.Organization_WHO2);
                    Mapping(bigTableRow, BigTableTitle.WHO3, whoRow, WhoTitle.Organization_WHO3);
                    Mapping(bigTableRow, BigTableTitle.WHO4, whoRow, WhoTitle.Organization_WHO4);
                    Mapping(bigTableRow, BigTableTitle.WHO5, whoRow, WhoTitle.Organization_WHO5);
                }
            }
        }
    }
}
