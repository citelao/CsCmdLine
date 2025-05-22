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

### Measuring Defender impact

In **admin** PowerShell:

```pwsh
# You need a fresh build of your exe, one that Defender doesn't remember:
# dotnet publish

# Admin!
New-MpPerformanceRecording -RecordTo native.etl

# In separate pwsh:
measure-command { .\bin\Release\net9.0\win-x64\publish\CsCmdLine.exe }
# OR
# measure-command { .\bin\Release\net9.0\win-x64\native\CsCmdLine.exe }

# In admin:
# end recording (press enter)

Get-MpPerformanceReport -path .\native.etl -topfiles 10
# Alt:
# Get-MpPerformanceReport -path .\native.etl -topscansperfile 10 -topfiles 1
# Get-MpPerformanceReport -path .\native.etl -topfiles 1 -TopProcessesPerFile 10 -TopScansPerProcessPerFile 10

```

An example run of the native version, showing **1852.1384ms** clock execution time, and a scan duration of **1822.5269ms** (so actual execution was only about 30ms).

![Defender analysis](./Defender%20analysis.png)

https://learn.microsoft.com/en-us/defender-endpoint/tune-performance-defender-antivirus
https://learn.microsoft.com/en-us/powershell/module/defenderperformance/new-mpperformancerecording?view=windowsserver2025-ps
https://learn.microsoft.com/en-us/powershell/module/defenderperformance/get-mpperformancereport?view=windowsserver2025-ps

## Results

| Mode | Trial # | Clock time | Defender time | ⇒ Execution time |
|-|-|-:|-:|-:|
| Native | 1 (fresh build) | 1852.1384ms | 1822.5269ms (real-time 1111.8026ms) | 29.6115ms (740.3358ms) |
| Native | 2 | 42.9371ms | (not measured) | 42.9371ms |
| Native | 3 | 16.9444ms | (not measured) | 16.9444ms |
| Native | 4 (a few min later) | 95.0381ms | 1065.7168ms (real-time 46.2020ms) | ??? (48.8361ms) |
| Publish | 1 (fresh build) | 1951.9085ms | 1915.4627ms (real-time 1877.1030ms) | 36.4458ms (74.8055ms) |
| Publish | 2 | 44.3055ms | (not measured) | 44.3055ms |
| Publish | 3 | 15.9743ms | (not measured) | 15.9743ms |
| Publish | 4 (a few min later) | 44.2909ms | 44.1827ms (real-time 0ms) | 0.1082ms (44.2909ms) |

Clearly, total scan time is not always useful, but neither is real-time scan time. It's some mixture.

But it does seem clear that Defender makes the first launch slower.
