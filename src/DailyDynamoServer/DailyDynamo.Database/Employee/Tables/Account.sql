CREATE TABLE [Employee].[Account] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [UserName]     VARCHAR (100)    NOT NULL,
    [PasswordHash] VARCHAR (500)    NOT NULL,
    [IsVerified]   BIT              CONSTRAINT [DF_Employee_Account_IsVerified] DEFAULT ((0)) NOT NULL,
    [IsActive]     BIT              CONSTRAINT [DF_Employee_Account_IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]    UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]    DATETIME         CONSTRAINT [DF_Employee_Account_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [UpdatedBy]    UNIQUEIDENTIFIER NOT NULL,
    [UpdatedOn]    DATETIME         CONSTRAINT [DF_Employee_Account_UpdatedOn] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Employee_Account_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);









