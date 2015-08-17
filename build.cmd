@echo off
if "%DOCKER_HOST%"=="" (
  echo "You must set docker environment vars using boot2docker shellinit or docker-machine env before running this script"
  exit /b -1
)
  
if not exist "packages\FAKE\tools\Fake.exe" ".nuget\NuGet.exe" "Install" "FAKE" "-OutputDirectory" "packages" "-ExcludeVersion"

"packages\FAKE\tools\Fake.exe" "%1" build.fsx
exit /b %errorlevel%