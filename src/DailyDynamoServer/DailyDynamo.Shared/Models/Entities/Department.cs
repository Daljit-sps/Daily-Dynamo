using System;
using System.Collections.Generic;

namespace DailyDynamo.Shared.Models.Entities;

public partial class Department
{
    public Guid Id { get; set; }

    public string DepartmentCode { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Profile> Profiles { get; set; } = new List<Profile>();
}
