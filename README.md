[![](https://img.shields.io/nuget/v/soenneker.github.repositories.security.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.github.repositories.security/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.github.repositories.security/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.github.repositories.security/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.github.repositories.security.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.github.repositories.security/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.github.repositories.security/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.github.repositories.security/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.GitHub.Repositories.Security

Retrieve and log open Dependabot, code-scanning, and secret-scanning alerts for one repository or every repository owned by an account.

## Installation

```bash
dotnet add package Soenneker.GitHub.Repositories.Security
```

## Configuration

```json
{
  "GH": {
    "Token": "github-token"
  }
}
```

The token needs read access to each security-alert feature being queried. GitHub may deny a feature when it is disabled for the repository or unavailable to the account.

## Registration

```csharp
services.AddGitHubRepositoriesSecurityUtilAsSingleton();
```

Use `AddGitHubRepositoriesSecurityUtilAsScoped()` for a scoped consumer.

## Retrieve alerts

```csharp
List<DependabotAlert> dependabot = await security.GetDependabotAlerts(
    "soenneker",
    "example-repository",
    cancellationToken: cancellationToken);

List<CodeScanningAlertItems> codeScanning = await security.GetCodeScanningAlerts(
    "soenneker",
    "example-repository",
    cancellationToken);

List<SecretScanningAlert> secretScanning = await security.GetSecretScanningAlerts(
    "soenneker",
    "example-repository",
    cancellationToken: cancellationToken);
```

Code-scanning and secret-scanning retrieval follows pagination. Dependabot retrieval requests GitHub's maximum page size of 100 alerts. Dependabot and secret-scanning methods default to open alerts; pass `state: null` for all states, or `state: "resolved"` for resolved secret-scanning alerts. Code scanning currently returns open alerts.

## Log alert summaries

```csharp
await security.LogAllSecurityAlerts(
    "soenneker",
    "example-repository",
    cancellationToken);

await security.LogAllSecurityAlertsForAllRepos(
    "soenneker",
    cancellationToken: cancellationToken);
```

Logging methods write alert summaries through `ILogger`. If a security feature is disabled or inaccessible, that feature is skipped and the API error is written at debug level; requested cancellation still stops the scan.
