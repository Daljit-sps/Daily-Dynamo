CREATE TABLE [Admin].[Department] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [DepartmentCode] VARCHAR (10)     NOT NULL,
    [DepartmentName] VARCHAR (100)    NOT NULL,
    [IsActive]       BIT              CONSTRAINT [DF_Admin_Department_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [Pk_Admin_Department_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

