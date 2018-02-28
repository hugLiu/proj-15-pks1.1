@ECHO OFF
SET CommandPath=%~dp0%

SET OutputPath=%CommandPath%..\Bin\Debug

SET InstallPath=%CommandPath%..\..\..\..\PKS.MgmtServices.Host
REM IF NOT EXIST %InstallPath% MD %InstallPath%

net stop PKS_MgmtServices_Host

REM del "%InstallPath%\PKS.*"

robocopy "%OutputPath%" "%InstallPath%" /S /SL

REM "%InstallPath%\PKS.MgmtServices.Host.exe" install

net start PKS_MgmtServices_Host

PAUSE
