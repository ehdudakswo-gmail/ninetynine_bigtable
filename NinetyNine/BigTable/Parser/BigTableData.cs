using System;

namespace NinetyNine
{
    class BigTableData
    {
        private string[] values = new string[BigTableTitleEnum.GetLength()];

        internal void Set(BigTableTitle title, string value)
        {
            int idx = BigTableTitleEnum.GetIndex(title);
            values[idx] = value;
        }

        internal string[] GetValues()
        {
            return values;
        }
    }
}