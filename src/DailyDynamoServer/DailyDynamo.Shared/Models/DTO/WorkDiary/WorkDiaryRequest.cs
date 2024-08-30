
namespace DailyDynamo.Shared.Models.DTO.WorkDiary;

public class WorkDiaryRequest
{
    public string ReportDate { get; set; }

    public string TaskAccomplished { get; set; } = null!;

    public string? ChallengesFaced { get; set; }

    public string NextDayPlan { get; set; } = null!;

}
