CREATE TABLE [List].[LookupElement] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [LookupHeaderId] UNIQUEIDENTIFIER NOT NULL,
    [Name]           VARCHAR (100)    NOT NULL,
    [GroupName]      VARCHAR (300)    NULL,
    [SortOrder]      INT              NOT NULL,
    [IsActive]       BIT              CONSTRAINT [DF_List_LookupElement_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]      DATETIME         CONSTRAINT [DF_List_LookupElement_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [UpdatedBy]      UNIQUEIDENTIFIER NOT NULL,
    [UpdatedOn]      DATETIME         CONSTRAINT [DF_List_LookupElement_UpdatedOn] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_List_LookupElement_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_List_LookupElement_CreatedBy_Employee_Profile_Id] FOREIGN KEY ([CreatedBy]) REFERENCES [Employee].[Profile] ([Id]),
    CONSTRAINT [FK_List_LookupElement_LookupHeaderId_List_LookupHeader_Id] FOREIGN KEY ([LookupHeaderId]) REFERENCES [List].[LookupHeader] ([Id]),
    CONSTRAINT [FK_List_LookupElement_UpdatedBy_Employee_Profile_Id] FOREIGN KEY ([UpdatedBy]) REFERENCES [Employee].[Profile] ([Id])
);

