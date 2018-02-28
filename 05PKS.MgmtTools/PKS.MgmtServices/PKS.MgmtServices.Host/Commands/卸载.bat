@ECHO OFF
SET CommandPath=%~dp0%

REM SET OutputPath=%CommandPath%..\Bin\Debug

SET InstallPath=%CommandPath%..\..\..\..\PKS.MgmtServices.Host
REM IF NOT EXIST %InstallPath% MD %InstallPath%

net stop PKS_MgmtServices_Host

REM robocopy "%OutputPath%" "%InstallPath%" /S /SL

"%InstallPath%\PKS.MgmtServices.Host.exe" uninstall

REM net start PKS_MgmtServices_Host

PAUSE
