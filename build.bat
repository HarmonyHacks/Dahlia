@echo off

pushd "%~dp0"

powershell -NoProfile -ExecutionPolicy unrestricted -Command "& { Import-Module .\lib\psake\psake.psm1; Invoke-psake .\build.ps1 -t Build }"

popd
