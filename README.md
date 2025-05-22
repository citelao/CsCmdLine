# CsCmdLine

Testing load time for different compilation methods.

## Usage

```pwsh
dotnet publish

.\bin\Release\net9.0\win-x64\publish\CsCmdLine.exe
.\bin\Release\net9.0\win-x64\native\CsCmdLine.exe

measure-command { .\bin\Release\net9.0\win-x64\publish\CsCmdLine.exe }
measure-command { .\bin\Release\net9.0\win-x64\native\CsCmdLine.exe }
```