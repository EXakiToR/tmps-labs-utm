# Laboratory 3
Requires the .NET 10 SDK. Run these commands from the repository root.

```powershell
dotnet build Lab3.Behavioral-design-patterns
dotnet run --project Lab3.Behavioral-design-patterns
dotnet run --project Lab3.Behavioral-design-patterns -- --self-test
```

The last command runs six dependency-free unit tests and returns a nonzero exit code on failure. See [REPORT.md](REPORT.md) for design details.
