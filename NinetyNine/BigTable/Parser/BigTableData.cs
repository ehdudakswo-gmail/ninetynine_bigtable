using NinetyNine.Template;
using System;

namespace NinetyNine
{
    class BigTableData
    {
        private string[] values;

        internal BigTableData()
        {
            Array titles = Enum.GetValues(typeof(BigTableTitle));
            int len = EnumManager.GetLength(titles);
            values = new string[len];
        }

        internal void Set(BigTableTitle title, string value)
        {
            int idx = EnumManager.GetIndex(title);
            values[idx] = value;
        }

        internal string[] GetValues()
        {
            return values;
        }
    }
}