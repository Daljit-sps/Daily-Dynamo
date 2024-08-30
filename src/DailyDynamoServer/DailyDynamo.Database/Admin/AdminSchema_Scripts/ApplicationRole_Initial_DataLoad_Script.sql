/********************************************************************************************************************************
*    Date        |   Created By      |          Description/Work item
**********************************************************************************
*  27/02/2024    |   Daljit Kaur    |   Admin Schema: Insert initial data for ApplicationRole Table if it doesn't exist
*  
*
*********************************************************************************************************************************/


IF NOT EXISTS (SELECT 1 FROM [Admin].ApplicationRole WHERE Id = 1)
BEGIN
    INSERT INTO [Admin].ApplicationRole (Id, RoleName, IsActive)
    VALUES (1, 'Admin',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].ApplicationRole WHERE Id = 2)
BEGIN
    INSERT INTO [Admin].ApplicationRole (Id, RoleName, IsActive)
    VALUES (2, 'Employee',1);
END