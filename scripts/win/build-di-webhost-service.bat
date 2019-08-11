@echo off

set web-host-image-name=local/astral-sample3:1.0

cd %~dp0
cd ./../../

echo publish Web.Host.Service.csproj to /app-webhost . . . 
dotnet publish src-back/Web.Host.Service/Web.Host.Service.csproj -c Release -o %~dp0/app-webhost

cd %~dp0

echo build docker image "%web-host-image-name%" . . .
docker build -t %web-host-image-name% -f app-webhost.Dockerfile .

for /f %%i in ('docker-machine ip') do set docker-machine-ip=%%i
echo docker-machine-ip is %docker-machine-ip%

mkdir images

echo save docker image "%web-host-image-name%" to /images/web-host-image.tar . . .
docker image save -o ./images/web-host-image.tar %web-host-image-name%

echo "%web-host-image-name%" build in %docker-machine-ip%


