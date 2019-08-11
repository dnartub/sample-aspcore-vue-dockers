@echo off
cd %~dp0

call %~dp0/build-images.bat

call %~dp0/run-docker-compose.bat