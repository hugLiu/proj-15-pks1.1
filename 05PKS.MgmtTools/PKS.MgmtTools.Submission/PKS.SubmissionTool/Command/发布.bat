@ECHO OFF
ECHO 编译
SET CompileProgram=D:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe

SET CurrentPath=%~dp0

SET SolutionPath=%CurrentPath%..\..\..\..\PKS.MgmtTools.sln
SET Project=PKS.SubmissionTool.csproj

SET CompileLog=%CurrentPath%..\bin\Compile.Log
REM "%CompileProgram%" "%SolutionPath%" /Build debug /Out "%CompileLog%" /Log
robocopy "%CurrentPath%..\bin\debug" "%CurrentPath%..\bin\Release" /S /SL

ECHO 打包
SET ZipProgram=D:\Program Files\7-Zip\7z.exe
SET ZipFile=%CurrentPath%..\bin\Excel批量提交工具.7z
SET ZipPath=%CurrentPath%..\bin\Release
DEL /Q "%ZipFile%"
DEL /Q "%ZipPath%\Logs\*.*"
rename "%ZipPath%\ProfileCatalog.xml" "ProfileCatalog.config"
DEL /Q "%ZipPath%\*.xml"
rename "%ZipPath%\ProfileCatalog.config" "ProfileCatalog.xml"
DEL /Q "%ZipPath%\*.dll.config"
DEL /Q "%ZipPath%\Index*.config"

"%ZipProgram%" a "%ZipFile%" "%ZipPath%\*"
ECHO 完成

pause
