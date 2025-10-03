/*
CREATE USER astrixApi FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER astrixApi;
ALTER ROLE db_datawriter ADD MEMBER astrixApi;	
ALTER ROLE db_ddladmin ADD MEMBER astrixApi;
*/
CREATE USER astrixLms FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER astrixLms;
ALTER ROLE db_datawriter ADD MEMBER astrixLms;	
ALTER ROLE db_ddladmin ADD MEMBER astrixLms;
