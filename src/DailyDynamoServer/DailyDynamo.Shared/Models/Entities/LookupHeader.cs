using System;
using System.Collections.Generic;

namespace DailyDynamo.Shared.Models.Entities;

public partial class LookupHeader
{
    public Guid Id { get; set; }

    public string KeyName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid UpdatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }

    public virtual Profile CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<LookupElement> LookupElements { get; set; } = new List<LookupElement>();

    public virtual Profile UpdatedByNavigation { get; set; } = null!;
}
