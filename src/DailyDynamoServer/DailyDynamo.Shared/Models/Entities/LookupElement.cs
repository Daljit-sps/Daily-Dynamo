using System;
using System.Collections.Generic;

namespace DailyDynamo.Shared.Models.Entities;

public partial class LookupElement
{
    public Guid Id { get; set; }

    public Guid LookupHeaderId { get; set; }

    public string Name { get; set; } = null!;

    public string? GroupName { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid UpdatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }

    public virtual Profile CreatedByNavigation { get; set; } = null!;

    public virtual LookupHeader LookupHeader { get; set; } = null!;

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();

    public virtual Profile UpdatedByNavigation { get; set; } = null!;
}
