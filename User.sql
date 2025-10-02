CREATE USER "astrix-api" FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER "astrix-api";
ALTER ROLE db_datawriter ADD MEMBER "astrix-api";	
ALTER ROLE db_ddladmin ADD MEMBER "astrix-api";

