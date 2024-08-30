/********************************************************************************************************************************
*    Date        |   Created By      |          Description/Work item
**********************************************************************************
*  27/02/2024    |   Daljit Kaur    |   Admin Schema: Insert initial data for Designation Table if it doesn't exist
*  
*
*********************************************************************************************************************************/

IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = 'BC7A64C8-9A53-4298-A057-120A8FB4D4EB')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName, IsActive)
    VALUES ('BC7A64C8-9A53-4298-A057-120A8FB4D4EB', 'Associate Software Engineer',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = '3C42B0F7-C84B-4A98-B01C-1BA25BA680C1')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName, IsActive)
    VALUES ('3C42B0F7-C84B-4A98-B01C-1BA25BA680C1', 'Digital Marketing Executive',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = '52A77712-7024-4761-AAB0-1DF1FCC53573')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName, IsActive)
    VALUES ('52A77712-7024-4761-AAB0-1DF1FCC53573', 'Assistant Manager',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = 'B38247CE-3DB7-4334-B3DB-379F69E46B27')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName,IsActive)
    VALUES ('B38247CE-3DB7-4334-B3DB-379F69E46B27', 'Project Lead',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = '6185BB2E-755C-4AB1-8D4D-54D7778C0C2D')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName, IsActive)
    VALUES ('6185BB2E-755C-4AB1-8D4D-54D7778C0C2D', 'Network Engineer',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = '3F084B0D-E795-47EF-9E49-6F0120710DCE')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName, IsActive)
    VALUES ('3F084B0D-E795-47EF-9E49-6F0120710DCE', 'Intern',1);
END


IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = '3C7F20F3-0F37-4068-B004-808D3A9013E3')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName, IsActive)
    VALUES ('3C7F20F3-0F37-4068-B004-808D3A9013E3', 'Network Administrator',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = '6A3BA01F-8D03-4CE5-8958-87FE23F410F1')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName, IsActive)
    VALUES ('6A3BA01F-8D03-4CE5-8958-87FE23F410F1', 'Sr. Solution Architect',1);
END


IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = '39AF52FA-B9A3-4823-A9DB-8A10FF9E9E54')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName,IsActive)
    VALUES ('39AF52FA-B9A3-4823-A9DB-8A10FF9E9E54', 'Talent Acquisition Specialist',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = '0BAF3C57-F49C-4428-87A1-8AF25C1750BA')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName, IsActive)
    VALUES ('0BAF3C57-F49C-4428-87A1-8AF25C1750BA', 'Associate HR Executive',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = '662152E8-9C13-41BA-BA10-9C01FDA60A67')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName, IsActive)
    VALUES ('662152E8-9C13-41BA-BA10-9C01FDA60A67', 'Quality Assurance Lead',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = '752D1FFB-BDB5-4548-9819-BBFE4562ECF2')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName, IsActive)
    VALUES ('752D1FFB-BDB5-4548-9819-BBFE4562ECF2', 'Quality Analyst',1);
END


IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = 'B5F11399-222C-472B-8723-CCE4AD3E9F54')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName, IsActive)
    VALUES ('B5F11399-222C-472B-8723-CCE4AD3E9F54', 'Business Development Manager',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = '8B1C565E-F65A-418E-A30B-DD6A0EDFB02C')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName,IsActive)
    VALUES ('8B1C565E-F65A-418E-A30B-DD6A0EDFB02C', 'Sr. Software Engineer',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = '3A47AE53-801F-4D38-BBB2-DF7456920200')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName, IsActive)
    VALUES ('3A47AE53-801F-4D38-BBB2-DF7456920200', 'Technology Analyst',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Designation WHERE Id = '02211A6D-7271-4BBF-B024-F8EC5A67DB03')
BEGIN
    INSERT INTO [Admin].Designation (Id, DesignationName, IsActive)
    VALUES ('02211A6D-7271-4BBF-B024-F8EC5A67DB03', 'Graphic Designer',1);
END

    
