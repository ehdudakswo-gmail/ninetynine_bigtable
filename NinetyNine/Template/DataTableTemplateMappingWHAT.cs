using System;
using System.Collections.Generic;
using System.Data;

namespace NinetyNine.Template
{
    enum MappingWhatTitle
    {
        WHAT1,
        WHAT2,
        WHAT3,
        WHAT4,
    }

    class DataTableTemplateMappingWHAT : DataTableTemplate
    {
        internal override void Set(DataTable dataTable)
        {
            var rows = dataTable.Rows;
            DataRow row0 = dataTable.NewRow();

            rows.Add(row0);

            SetRow0(row0);
        }

        private void SetRow0(DataRow row)
        {
            List<Enum> bigTableValues = GetBigTableValues();

            for (int i = 0; i < bigTableValues.Count; i++)
            {
                Enum bigTableValue = bigTableValues[i];
                string bigTableText = EnumManager.GetText(bigTableValue);
                row[i] = bigTableText;
            }
        }

        private List<Enum> GetBigTableValues()
        {
            Array values = Enum.GetValues(typeof(MappingWhatTitle));
            List<Enum> bigTableValues = new List<Enum>();

            foreach (MappingWhatTitle value in values)
            {
                Enum bigTableValue = GetMatchedBigTableValue(value);
                bigTableValues.Add(bigTableValue);
            }

            return bigTableValues;
        }

        private Enum GetMatchedBigTableValue(MappingWhatTitle value)
        {
            switch (value)
            {
                case MappingWhatTitle.WHAT1:
                    return BigTableTitle.WHAT1;
                case MappingWhatTitle.WHAT2:
                    return BigTableTitle.WHAT2;
                case MappingWhatTitle.WHAT3:
                    return BigTableTitle.WHAT3;
                case MappingWhatTitle.WHAT4:
                    return BigTableTitle.WHAT4;
                default:
                    return null;
            }
        }
    }
}