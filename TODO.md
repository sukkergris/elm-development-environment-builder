## Validation Checklist

- ✅ dotnet --list-runtimes (skal inkludere Microsoft.NETCore.App 8.0.18)
- ✅ dotnet --info (RID bør være linux-arm64)
- ✅ node --version (24.11.1)
- ✅ elm --version (0.19.1)

## Results

dotnet --info ░▒▓ ✔  at 12:00:02  
.NET SDK:
Version: 10.0.100
Commit: b0f34d51fc
Workload version: 10.0.100-manifests.4c0ca8ba
MSBuild version: 18.0.2+b0f34d51f

Runtime Environment:
OS Name: debian
OS Version: 12
OS Platform: Linux
RID: linux-arm64
Base Path: /opt/dotnet/sdk/10.0.100/

.NET workloads installed:
There are no installed workloads to display.
Configured to use workload sets when installing new manifests.
No workload sets are installed. Run "dotnet workload restore" to install a workload set.

Host:
Version: 10.0.0
Architecture: arm64
Commit: b0f34d51fc

.NET SDKs installed:
10.0.100 [/opt/dotnet/sdk]

.NET runtimes installed:
Microsoft.AspNetCore.App 9.0.14 [/opt/dotnet/shared/Microsoft.AspNetCore.App]
Microsoft.AspNetCore.App 10.0.0 [/opt/dotnet/shared/Microsoft.AspNetCore.App]
Microsoft.NETCore.App 8.0.18 [/opt/dotnet/shared/Microsoft.NETCore.App]
Microsoft.NETCore.App 9.0.14 [/opt/dotnet/shared/Microsoft.NETCore.App]
Microsoft.NETCore.App 10.0.0 [/opt/dotnet/shared/Microsoft.NETCore.App]

Other architectures found:
None

Environment variables:
DOTNET_ROOT [/opt/dotnet]

global.json file:
Not found

Learn more:
<https://aka.ms/dotnet/info>

Download .NET:
<https://aka.ms/dotnet/download>

    /workspace  on   main !5 ?2 ▓▒░ node --version ░▒▓ ✔  at 12:00:09  
v24.11.1

    /workspace  on   main !5 ?2 ▓▒░ elm --version ░▒▓ ✔  at 12:00:18  
0.19.1
