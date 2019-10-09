using System;

namespace NinetyNine
{
    class BigTableData
    {
        private string[] values;

        internal BigTableData(int len)
        {
            values = new string[len];
        }

        internal void Set(int idx, string value)
        {
            values[idx] = value;
        }

        internal string[] GetValues()
        {
            return values;
        }
    }
}