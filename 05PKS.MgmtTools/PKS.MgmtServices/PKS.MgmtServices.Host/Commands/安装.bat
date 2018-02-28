@ECHO OFF
SET CommandPath=%~dp0%

SET OutputPath=%CommandPath%..\Bin\Debug

SET InstallPath=%CommandPath%..\..\..\..\PKS.MgmtServices.Host
IF NOT EXIST %InstallPath% MD %InstallPath%

net stop PKS_MgmtServices_Host

del "%InstallPath%\PKS.*"

robocopy "%OutputPath%" "%InstallPath%" /S /SL

"%InstallPath%\PKS.MgmtServices.Host.exe" install

net start PKS_MgmtServices_Host

PAUSE
