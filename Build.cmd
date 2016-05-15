@echo off

IF NOT "%VS140COMNTOOLS%" == "" (call "%VS140COMNTOOLS%vsvars32.bat")

@echo on

.paket\paket.bootstrapper.exe
.paket\paket.exe update

msbuild.exe "src\KeyboardNavigationHackSample.sln" /p:configuration=Debug /p:platform="Any CPU" /m /t:Clean,Rebuild
