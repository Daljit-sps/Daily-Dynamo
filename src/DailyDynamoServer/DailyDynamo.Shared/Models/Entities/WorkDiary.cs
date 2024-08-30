using System;
using System.Collections.Generic;

namespace DailyDynamo.Shared.Models.Entities;

public partial class WorkDiary
{
    public Guid Id { get; set; }

    public DateOnly ReportDate { get; set; }

    public string TaskAccomplished { get; set; } = null!;

    public string? ChallengesFaced { get; set; }

    public string NextDayPlan { get; set; } = null!;

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid UpdatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }

    public virtual Profile CreatedByNavigation { get; set; } = null!;

    public virtual Profile UpdatedByNavigation { get; set; } = null!;
}
