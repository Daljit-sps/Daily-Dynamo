namespace DailyDynamo.Shared.Models.DTO.WorkDiary;

public class WorkDiaryGetResponse
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public DateOnly ReportDate { get; set; }

    public string TaskAccomplished { get; set; } = null!;

    public string? ChallengesFaced { get; set; }

    public string NextDayPlan { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }


}
