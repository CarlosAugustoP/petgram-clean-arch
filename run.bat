@echo off
SETLOCAL

REM Set environment variables
SET DB_HOST=localhost
SET DB_PORT=5432
SET DB_DATABASE=postgres
SET DB_USERNAME=postgres
SET DB_PASSWORD=1234
SET SUPABASE_URL=https://fjarihwipfgwgttntmul.supabase.co
SET SUPABASE_KEY=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZqYXJpaHdpcGZnd2d0dG50bXVsIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDAwNjEwNDQsImV4cCI6MjA1NTYzNzA0NH0.mVSSpy7m4kNwK-pLJcGU3vxTNVnYMS1c58hA6vSiyhQ

REM Restore dependencies
dotnet restore

REM Start Docker containers
docker-compose up -d

REM Run the API
cd API
dotnet watch run

ENDLOCAL