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

            return dataTable;
        }
    }
}