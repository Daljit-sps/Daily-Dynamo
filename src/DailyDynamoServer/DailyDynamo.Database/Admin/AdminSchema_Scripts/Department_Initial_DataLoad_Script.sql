
/********************************************************************************************************************************
*    Date        |   Created By      |          Description/Work item
**********************************************************************************
*  27/02/2024    |   Daljit Kaur    |   Admin Schema: Insert initial data for Department Table if it doesn't exist
*  
*
*********************************************************************************************************************************/


IF NOT EXISTS (SELECT 1 FROM [Admin].Department WHERE Id = '52CDA349-943D-483C-B0EC-3715B9D8B3DB')
BEGIN
    INSERT INTO [Admin].Department (Id, DepartmentCode, DepartmentName, IsActive)
    VALUES ('52CDA349-943D-483C-B0EC-3715B9D8B3DB', 'DTX', 'Digital Transformation',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Department WHERE Id = '545853B0-D65A-4BB9-8B32-40FA1C851F50')
BEGIN
    INSERT INTO [Admin].Department (Id, DepartmentCode, DepartmentName, IsActive)
    VALUES ('545853B0-D65A-4BB9-8B32-40FA1C851F50', 'BD', 'Business Development',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Department WHERE Id = '9C4BC204-DD96-4D7B-8637-6F4334B2C098')
BEGIN
    INSERT INTO [Admin].Department (Id, DepartmentCode, DepartmentName, IsActive)
    VALUES ('9C4BC204-DD96-4D7B-8637-6F4334B2C098', 'HR', 'Human Resources',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Department WHERE Id = '8AC25351-300D-43DC-A987-917A0A279752')
BEGIN
    INSERT INTO [Admin].Department (Id, DepartmentCode, DepartmentName, IsActive)
    VALUES ('8AC25351-300D-43DC-A987-917A0A279752', 'IT', 'Information Technology',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Department WHERE Id = '495CF17D-48F1-4673-B40A-C466CD54923B')
BEGIN
    INSERT INTO [Admin].Department (Id, DepartmentCode, DepartmentName, IsActive)
    VALUES ('495CF17D-48F1-4673-B40A-C466CD54923B', 'QA', 'Testing',1);
END

IF NOT EXISTS (SELECT 1 FROM [Admin].Department WHERE DepartmentCode = 'D1F8230D-E661-4248-AB3F-CFC419617E19')
BEGIN
    INSERT INTO [Admin].Department (Id, DepartmentCode, DepartmentName, IsActive)
    VALUES ('D1F8230D-E661-4248-AB3F-CFC419617E19', 'JST', 'JavaScript',1);
END

