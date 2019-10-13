using System;
using System.Data;
using System.Text;

namespace NinetyNine.BigTable.Parser
{
    abstract class BigTableParser
    {
        abstract internal void Parse();

        protected string Trim(string str)
        {
            return str.Replace(" ", "").Trim();
        }

        protected string[] Split(string str, int idx)
        {
            string[] ret = new string[2];

            ret[0] = str.Substring(0, idx).Trim();
            ret[1] = str.Substring(idx + 1).Trim();

            return ret;
        }

        protected string Remove(string str, string removes)
        {
            StringBuilder sb = new StringBuilder(str.Length);

            foreach (char ch in str)
            {
                if (isContain(removes, ch))
                {
                    continue;
                }

                sb.Append(ch);
            }

            return sb.ToString();
        }

        protected bool isContain(string str, char target)
        {
            foreach (char ch in str)
            {
                if (ch == target)
                {
                    return true;
                }
            }

            return false;
        }

        protected bool isLeastOneEmpty(DataRow row, int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                string str = row[i].ToString();
                if (str == "")
                {
                    return true;
                }
            }

            return false;
        }

        protected string[] GetStringValues(DataRow row, int from, int to)
        {
            string[] values = new string[to - from + 1];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = row[from + i].ToString();
            }

            return values;
        }

        protected bool isEmpty(DataRow row, int columnCount)
        {
            for (int i = 0; i < columnCount; i++)
            {
                if (isEmpty(row[i].ToString()))
                {
                    return false;
                }
            }

            return true;
        }

        protected bool isEmpty(string str)
        {
            return (str == null || str == "");
        }
    }
}