/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/



/********************************************************************************************************************************
*    Date        |   Created By      |          Description/Work item
**********************************************************************************
*  27/02/2024    |   Daljit Kaur    |   Admin Schema: call the Admin Schema Scripts
*  
*
*********************************************************************************************************************************/




-- Call each script within the AdminSchema_Scripts folder
:r .\Admin\AdminSchema_Scripts\ApplicationRole_Initial_DataLoad_Script.sql
:r .\Admin\AdminSchema_Scripts\Department_Initial_DataLoad_Script.sql
:r .\Admin\AdminSchema_Scripts\Designation_Initial_DataLoad_Script.sql
