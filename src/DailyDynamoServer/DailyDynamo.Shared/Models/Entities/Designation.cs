using System;
using System.Collections.Generic;

namespace DailyDynamo.Shared.Models.Entities;

public partial class Designation
{
    public Guid Id { get; set; }

    public string DesignationName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}
