@ECHO OFF
SET CommandPath=%~dp0%

SET OutputPath=%CommandPath%..\Bin\Debug

SET WinFramePath=%CommandPath%..\..\..\..\01PKS.Library\PKS.Lib\WinFrame

robocopy "%WinFramePath%" "%OutputPath%" /S /SL

PAUSE
