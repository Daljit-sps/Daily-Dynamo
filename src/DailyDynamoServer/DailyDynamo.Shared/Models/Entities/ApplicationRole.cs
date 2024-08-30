using System;
using System.Collections.Generic;

namespace DailyDynamo.Shared.Models.Entities;

public partial class ApplicationRole
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}
