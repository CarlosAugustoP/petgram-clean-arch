@echo off
SETLOCAL

REM Set environment variables for database configuration
SET DB_HOST=localhost
SET DB_PORT=5432
SET DB_DATABASE=postgres
SET DB_USERNAME=postgres
SET DB_PASSWORD=1234

REM Restore the .NET Core project
dotnet restore

REM Change directory to API
cd API

REM Set environment variables and run the project with watch
SET DB_HOST=%DB_HOST%
SET DB_PORT=%DB_PORT%
SET DB_DATABASE=%DB_DATABASE%
SET DB_USERNAME=%DB_USERNAME%
SET DB_PASSWORD=%DB_PASSWORD%

dotnet watch run

ENDLOCAL