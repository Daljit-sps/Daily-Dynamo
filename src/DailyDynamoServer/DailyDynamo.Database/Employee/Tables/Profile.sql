CREATE TABLE [Employee].[Profile] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [AccountId]       UNIQUEIDENTIFIER NOT NULL,
    [RoleId]          INT              NOT NULL,
    [DepartmentId]    UNIQUEIDENTIFIER NULL,
    [DesignationId]   UNIQUEIDENTIFIER NULL,
    [ManagerId]       UNIQUEIDENTIFIER NULL,
    [FirstName]       VARCHAR (50)     NOT NULL,
    [LastName]        VARCHAR (50)     NOT NULL,
    [EmailID]         VARCHAR (100)    NOT NULL,
    [ProfileImageUrl] NVARCHAR (200)   NULL,
    [GenderId]        UNIQUEIDENTIFIER NULL,
    [DateOfBirth]     DATE             NULL,
    [Address]         VARCHAR (200)    NULL,
    [MobileNo]        VARCHAR (10)     NULL,
    [IsActive]        BIT              CONSTRAINT [DF_Employee_Profile_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]       UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]       DATETIME         CONSTRAINT [DF_Employee_Profile_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [UpdatedBy]       UNIQUEIDENTIFIER NOT NULL,
    [UpdatedOn]       DATETIME         CONSTRAINT [DF_Employee_Profile_UpdatedOn] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Employee_Profile_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Employee_Profile_AccountId_Employee_Account_Id] FOREIGN KEY ([AccountId]) REFERENCES [Employee].[Account] ([Id]),
    CONSTRAINT [FK_Employee_Profile_DepartmentId_Admin_Department_Id] FOREIGN KEY ([DepartmentId]) REFERENCES [Admin].[Department] ([Id]),
    CONSTRAINT [FK_Employee_Profile_DesignationId_Admin_Designation_Id] FOREIGN KEY ([DesignationId]) REFERENCES [Admin].[Designation] ([Id]),
    CONSTRAINT [FK_Employee_Profile_GenderId_List_LookupElement_Id] FOREIGN KEY ([GenderId]) REFERENCES [List].[LookupElement] ([Id]),
    CONSTRAINT [FK_Employee_Profile_ManagerId_Employee_Profile_Id] FOREIGN KEY ([ManagerId]) REFERENCES [Employee].[Profile] ([Id]),
    CONSTRAINT [FK_Employee_Profile_RoleId_Admin_Role_Id] FOREIGN KEY ([RoleId]) REFERENCES [Admin].[ApplicationRole] ([Id]),
    CONSTRAINT [UQ_Employee_Profile_EmailID] UNIQUE NONCLUSTERED ([EmailID] ASC),
    CONSTRAINT [UQ_Employee_Profile_MobileNo] UNIQUE NONCLUSTERED ([MobileNo] ASC),
    CONSTRAINT [UQ_Employee_Profile_AccountId] UNIQUE NONCLUSTERED ([AccountId] ASC)
);



