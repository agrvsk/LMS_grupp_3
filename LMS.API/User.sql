/*
CREATE USER astrixApi FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER astrixApi;
ALTER ROLE db_datawriter ADD MEMBER astrixApi;	
ALTER ROLE db_ddladmin ADD MEMBER astrixApi;

CREATE USER astrixLms FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER astrixLms;
ALTER ROLE db_datawriter ADD MEMBER astrixLms;	
ALTER ROLE db_ddladmin ADD MEMBER astrixLms;
*/

/* TÖM Databasen */
DELETE FROM [dbo].[Activities] WHERE 1=1;
DELETE FROM [dbo].[ActivityTypes] WHERE 1=1;
DELETE FROM [dbo].[ApplicationUser] WHERE 1=1;
DELETE FROM [dbo].[ApplicationUserSubmission] WHERE 1=1;
DELETE FROM [dbo].[AspNetRoleClaims] WHERE 1=1;
DELETE FROM [dbo].[AspNetRoles] WHERE 1=1;
DELETE FROM [dbo].[AspNetUserClaims] WHERE 1=1;
DELETE FROM [dbo].[AspNetUserLogins] WHERE 1=1;
DELETE FROM [dbo].[AspNetUserRoles] WHERE 1=1;
DELETE FROM [dbo].[AspNetUserTokens] WHERE 1=1;

DELETE FROM [dbo].[Assignments] WHERE 1=1;
DELETE FROM [dbo].[Courses] WHERE 1=1;
DELETE FROM [dbo].[Documents] WHERE 1=1;
DELETE FROM [dbo].[Modules] WHERE 1=1;
DELETE FROM [dbo].[Submissions] WHERE 1=1;

