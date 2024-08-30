using System;
using System.Collections.Generic;

namespace DailyDynamo.Shared.Models.Entities;

public partial class Profile
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public int RoleId { get; set; }

    public Guid? DepartmentId { get; set; }

    public Guid? DesignationId { get; set; }

    public Guid? ManagerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public string? ProfileImageUrl { get; set; }

    public Guid? GenderId { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? MobileNo { get; set; }

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid UpdatedBy { get; set; }

    public DateTime UpdatedOn { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Department? Department { get; set; }

    public virtual Designation? Designation { get; set; }

    public virtual LookupElement? Gender { get; set; }

    public virtual ICollection<Profile> InverseManager { get; set; } = new List<Profile>();

    public virtual ICollection<LookupElement> LookupElementCreatedByNavigations { get; set; } = new List<LookupElement>();

    public virtual ICollection<LookupElement> LookupElementUpdatedByNavigations { get; set; } = new List<LookupElement>();

    public virtual ICollection<LookupHeader> LookupHeaderCreatedByNavigations { get; set; } = new List<LookupHeader>();

    public virtual ICollection<LookupHeader> LookupHeaderUpdatedByNavigations { get; set; } = new List<LookupHeader>();

    public virtual Profile? Manager { get; set; }

    public virtual ApplicationRole Role { get; set; } = null!;

    public virtual ICollection<WorkDiary> WorkDiaryCreatedByNavigations { get; set; } = new List<WorkDiary>();

    public virtual ICollection<WorkDiary> WorkDiaryUpdatedByNavigations { get; set; } = new List<WorkDiary>();
}
