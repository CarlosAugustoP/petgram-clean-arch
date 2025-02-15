@echo off
SETLOCAL

REM Set environment variables
SET DB_HOST=localhost
SET DB_PORT=5432
SET DB_DATABASE=postgres
SET DB_USERNAME=postgres
SET DB_PASSWORD=1234

REM Restore dependencies
dotnet restore

REM Start Docker containers
docker-compose up -d

REM Run the API
cd API
dotnet watch run

ENDLOCAL