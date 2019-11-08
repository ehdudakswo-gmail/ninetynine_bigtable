using System;
using System.ComponentModel;
using System.Data;

namespace NinetyNine.Template
{
    enum ScheduleTitle
    {
        [Description("ID")]
        ID,

        [Description("구분")]
        Floor,

        [Description("Description")]
        Description,

        [Description("T")]
        T,

        [Description("Plan_Start")]
        Plan_Start,

        [Description("Plan_Finish")]
        Plan_Finish,

        [Description("Plan_Dur")]
        Plan_Dur,

        [Description("Actual_Start")]
        Actual_Start,

        [Description("Actual_Finish")]
        Actual_Finish,

        [Description("Actual_Dur")]
        Actual_Dur,

        [Description("Actual_Prog")]
        Actual_Prog,
    }

    internal class DataTableTemplateSchedule : DataTableTemplate
    {
        internal override DataTable GetTemplateDataTable()
        {
            Init();

            DataRow row0 = dataTable.NewRow();
            SetRow0(row0);
            rows.Add(row0);

            return dataTable;
        }

        private void SetRow0(DataRow row)
        {
            Array values = Enum.GetValues(typeof(ScheduleTitle));
            SetTitleDescriptions(row, values);
        }
    }
}