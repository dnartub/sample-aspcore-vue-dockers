@echo off
cd %~dp0

for /f %%i in ('docker-machine ip') do set docker-machine-ip=%%i
echo docker-machine-ip is %docker-machine-ip%

call docker-compose down
call docker-compose up -d

echo Waiting webhost controllers up...
TIMEOUT /T 3
call chrome %docker-machine-ip%