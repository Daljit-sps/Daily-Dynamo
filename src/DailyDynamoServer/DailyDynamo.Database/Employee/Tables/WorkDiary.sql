CREATE TABLE [Employee].[WorkDiary] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [ReportDate]       DATE             NOT NULL,
    [TaskAccomplished] VARCHAR (2000)   NOT NULL,
    [ChallengesFaced]  VARCHAR (500)    NULL,
    [NextDayPlan]      VARCHAR (500)    NOT NULL,
    [IsActive]         BIT              CONSTRAINT [DF_Employee_WorkDiary_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]        UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]        DATETIME         CONSTRAINT [DF_Employee_WorkDiary_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [UpdatedBy]        UNIQUEIDENTIFIER NOT NULL,
    [UpdatedOn]        DATETIME         CONSTRAINT [DF_Employee_WorkDiary_UpdatedOn] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Employee_WorkDiary_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Employee_WorkDiary_CreatedBy_Employee_Profile_Id] FOREIGN KEY ([CreatedBy]) REFERENCES [Employee].[Profile] ([Id]),
    CONSTRAINT [FK_Employee_WorkDiary_UpdatedBy_Employee_Profile_Id] FOREIGN KEY ([UpdatedBy]) REFERENCES [Employee].[Profile] ([Id])
);

