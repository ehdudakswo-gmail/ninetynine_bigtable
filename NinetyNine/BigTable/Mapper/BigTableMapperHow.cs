using NinetyNine.BigTable.Dictionary;
using NinetyNine.Template;
using NinetyNine.Template.Mapping;
using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.BigTable.Mapper
{
    class BigTableMapperHow : BigTableMapper
    {
        internal BigTableMapperHow(DataTable bigTable) : base(bigTable)
        {
        }

        internal Dictionary<string, DataRow> howDictionary { get; set; }

        internal override void Mapping()
        {
            Enum[] bigTableKeys = new Enum[] { BigTableTitle.HOW4, BigTableTitle.HOW5 };
            Enum[] howKeys = new Enum[] { bigTableKeys[0], bigTableKeys[1] };

            for (int rowIdx = 0; rowIdx < bigTableRowsCount; rowIdx++)
            {
                if (rowIdx < CONTENT_ROWIDX)
                {
                }
                else
                {
                    DataRow bigTableRow = bigTableRows[rowIdx];
                    BigTableErrorCell[] errorCells = GetErrorCells(rowIdx, bigTableKeys);

                    string howKey = BigTableDictionary.GetKey(bigTableRow, howKeys);
                    if (howDictionary.ContainsKey(howKey) == false)
                    {
                        ThrowException(bigTable, errorCells, ERROR_KEY_NONE);
                    }

                    DataRow howRow = howDictionary[howKey];
                    Mapping(bigTableRow, BigTableTitle.HOW1, howRow, HowTitle.BigTable_WorkLarge);
                    Mapping(bigTableRow, BigTableTitle.HOW2, howRow, HowTitle.BigTable_WorkMedium);
                    Mapping(bigTableRow, BigTableTitle.HOW3, howRow, HowTitle.BigTable_WorkSmall);
                }
            }
        }
    }
}