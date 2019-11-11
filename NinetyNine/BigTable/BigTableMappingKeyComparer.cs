using System.Collections.Generic;

namespace NinetyNine.BigTable
{
    class BigTableMappingKeyComparer
    {
        internal class WorkComparer : IComparer<string[]>
        {
            public int Compare(string[] x, string[] y)
            {
                string x0 = x[0];
                string y0 = y[0];
                string x1 = x[1];
                string y1 = y[1];

                if (x0 != y0)
                {
                    return x0.CompareTo(y0);
                }
                else
                {
                    return x1.CompareTo(y1);
                }
            }
        }

        internal class FloorComparer : IComparer<string[]>
        {
            public int Compare(string[] x, string[] y)
            {
                string x0 = x[0];
                string y0 = y[0];

                int x0Num;
                int y0Num;

                bool isX0Num = int.TryParse(x0, out x0Num);
                bool isY0Num = int.TryParse(y0, out y0Num);

                if (isX0Num && isY0Num)
                {
                    return x0Num - y0Num;
                }
                else
                {
                    return x0.CompareTo(y0);
                }
            }
        }

        internal class HowComparer : IComparer<string[]>
        {
            public int Compare(string[] x, string[] y)
            {
                string x0 = x[0];
                string y0 = y[0];

                return x0.CompareTo(y0);
            }
        }
    }
}
