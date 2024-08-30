using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDynamo.Shared.Models.DTO.WorkDiary
{
    public class WorkDiaryDashboardResponse
    {
        public Guid DSRId { get; set; }
        public string? Name { get; set; }

        public string? Status { get; set; }

        public string? TaskAccomplished { get; set; }

        public string? NextDayPlan { get; set; }

        public DateOnly? ReportDate { get; set; }


    }
}
