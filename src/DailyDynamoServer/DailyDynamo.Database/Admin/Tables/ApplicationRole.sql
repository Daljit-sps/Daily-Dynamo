CREATE TABLE [Admin].[ApplicationRole] (
    [Id]       INT          NOT NULL,
    [RoleName] VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_Admin_ApplicationRole_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Admin_ApplicationRole_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

