CREATE TABLE [List].[LookupHeader] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [KeyName]     VARCHAR (100)    NOT NULL,
    [Description] VARCHAR (500)    NULL,
    [IsActive]    BIT              CONSTRAINT [DF_List_LookupHeader_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]   UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]   DATETIME         CONSTRAINT [DF_List_LookupHeader_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [UpdatedBy]   UNIQUEIDENTIFIER NOT NULL,
    [UpdatedOn]   DATETIME         CONSTRAINT [DF_List_LookupHeader_UpdatedOn] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_List_LookupHeader_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_List_LookupHeader_CreatedBy_Employee_Profile_Id] FOREIGN KEY ([CreatedBy]) REFERENCES [Employee].[Profile] ([Id]),
    CONSTRAINT [FK_List_LookupHeader_UpdatedBy_Employee_Profile_Id] FOREIGN KEY ([UpdatedBy]) REFERENCES [Employee].[Profile] ([Id])
);

