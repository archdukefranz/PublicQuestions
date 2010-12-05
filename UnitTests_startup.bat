@echo off
echo -------------------------------------------------
echo -    Public Question Unit Testing Startup
echo -------------------------------------------------
cd C:\Development\PublicQuestions
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild PublicQuestions.sln /p:Configuration=Debug
..\PublicQuestions-AddIns\NUnit-2.5.8\bin\net-2.0\nunit.exe PublicQuestions.UnitTests\bin\Debug\PublicQuestions.UnitTests.dll
