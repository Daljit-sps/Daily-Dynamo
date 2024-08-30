CREATE TABLE [Admin].[Designation] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [DesignationName] VARCHAR (100)    NOT NULL,
    [IsActive]        BIT              CONSTRAINT [DF_Admin_Designation_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Admin_Designation_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

