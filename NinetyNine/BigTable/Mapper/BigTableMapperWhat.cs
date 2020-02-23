using NinetyNine.BigTable.Dictionary;
using NinetyNine.Template;
using NinetyNine.Template.Mapping;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Mapper
{
    class BigTableMapperWhat : BigTableMapper
    {
        internal BigTableMapperWhat(DataTable bigTable) : base(bigTable)
        {
        }

        internal Dictionary<string, DataRow> whatDictionary { get; set; }

        internal override void Mapping()
        {
            Enum[] bigTableKeys = new Enum[] { BigTableTitle.WHERE2 };
            Enum[] whatKeys = new Enum[] { bigTableKeys[0] };

            for (int rowIdx = 0; rowIdx < bigTableRowsCount; rowIdx++)
            {
                if (rowIdx < CONTENT_ROWIDX)
                {
                }
                else
                {
                    DataRow bigTableRow = bigTableRows[rowIdx];
                    BigTableErrorCell[] errorCells = GetErrorCells(rowIdx, bigTableKeys);

                    string whatKey = BigTableDictionary.GetKey(bigTableRow, whatKeys);
                    if (whatDictionary.ContainsKey(whatKey) == false)
                    {
                        ThrowException(bigTable, errorCells, ERROR_KEY_NONE);
                    }

                    DataRow whatRow = whatDictionary[whatKey];
                    Mapping(bigTableRow, BigTableTitle.WHAT1, whatRow, WhatTitle.BigTable_Structure);
                    Mapping(bigTableRow, BigTableTitle.WHAT2, whatRow, WhatTitle.BigTable_StructureSeparation);
                }
            }
        }
    }
}