@echo off

set client-host-image-name=local/astral-sample3-proxy-front:1.0

cd %~dp0
mkdir app-front
cd ./../../src-front

		echo build front files to /app-front . . . 
		call npm i --loglevel=error
		call npm run build
		
		xcopy /y /e dist %~dp0\app-front\

cd %~dp0

		echo build docker image "%client-host-image-name%" . . .
		docker build -t %client-host-image-name% -f app-proxy-front.Dockerfile .

for /f %%i in ('docker-machine ip') do set docker-machine-ip=%%i
echo docker-machine-ip is %docker-machine-ip%

mkdir images

echo save docker image "%client-host-image-name%" to /images/client-host-image.tar . . .
docker image save -o ./images/client-host-image.tar %client-host-image-name%

echo "%client-host-image-name%" build in %docker-machine-ip%