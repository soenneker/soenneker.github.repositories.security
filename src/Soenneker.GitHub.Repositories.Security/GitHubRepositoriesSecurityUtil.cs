using Microsoft.Extensions.Logging;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.GitHub.ClientUtil.Abstract;
using Soenneker.GitHub.OpenApiClient;
using Soenneker.GitHub.OpenApiClient.Models;
using Soenneker.GitHub.OpenApiClient.Repos.Item.Item.SecretScanning.Alerts;
using Soenneker.GitHub.Repositories.Abstract;
using Soenneker.GitHub.Repositories.Security.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.GitHub.Repositories.Security;

/// <inheritdoc cref="IGitHubRepositoriesSecurityUtil"/>
public sealed class GitHubRepositoriesSecurityUtil : IGitHubRepositoriesSecurityUtil
{
    private readonly ILogger<GitHubRepositoriesSecurityUtil> _logger;
    private readonly IGitHubOpenApiClientUtil _gitHubClientUtil;
    private readonly IGitHubRepositoriesUtil _gitHubRepositoriesUtil;

    public GitHubRepositoriesSecurityUtil(ILogger<GitHubRepositoriesSecurityUtil> logger, IGitHubOpenApiClientUtil gitHubClientUtil,
        IGitHubRepositoriesUtil gitHubRepositoriesUtil)
    {
        _logger = logger;
        _gitHubClientUtil = gitHubClientUtil;
        _gitHubRepositoriesUtil = gitHubRepositoriesUtil;
    }

    public async ValueTask<List<DependabotAlert>> GetDependabotAlerts(string owner, string name, string? state = "open",
        CancellationToken cancellationToken = default)
    {
        GitHubOpenApiClient client = await _gitHubClientUtil.Get(cancellationToken)
                                                            .NoSync();

        List<DependabotAlert>? response = await client.Repos[owner][name]
                                                      .Dependabot.Alerts.GetAsync(config =>
                                                      {
                                                          config.QueryParameters.PerPage = 100;
                                                          if (!string.IsNullOrEmpty(state))
                                                              config.QueryParameters.State = state;
                                                      }, cancellationToken)
                                                      .NoSync();

        return response?.ToList() ?? [];
    }

    public async ValueTask<List<CodeScanningAlertItems>> GetCodeScanningAlerts(string owner, string name, CancellationToken cancellationToken = default)
    {
        GitHubOpenApiClient client = await _gitHubClientUtil.Get(cancellationToken)
                                                            .NoSync();

        List<CodeScanningAlertItems>? response = await client.Repos[owner][name]
                                                             .CodeScanning.Alerts.GetAsync(config =>
                                                             {
                                                                 config.QueryParameters.PerPage = 100;
                                                                 config.QueryParameters.State = CodeScanningAlertStateQuery.Open;
                                                             }, cancellationToken)
                                                             .NoSync();

        return response?.ToList() ?? [];
    }

    public async ValueTask<List<SecretScanningAlert>> GetSecretScanningAlerts(string owner, string name, string? state = "open",
        CancellationToken cancellationToken = default)
    {
        GitHubOpenApiClient client = await _gitHubClientUtil.Get(cancellationToken)
                                                            .NoSync();

        List<SecretScanningAlert>? response = await client.Repos[owner][name]
                                                          .SecretScanning.Alerts.GetAsync(config =>
                                                          {
                                                              config.QueryParameters.PerPage = 100;
                                                              if (!string.IsNullOrEmpty(state))
                                                                  config.QueryParameters.State = state == "resolved"
                                                                      ? GetStateQueryParameterType.Resolved
                                                                      : GetStateQueryParameterType.Open;
                                                          }, cancellationToken)
                                                          .NoSync();

        return response?.ToList() ?? [];
    }

    public async ValueTask LogAllSecurityAlerts(string owner, string name, CancellationToken cancellationToken = default)
    {
        await LogDependabotAlerts(owner, name, cancellationToken)
            .NoSync();
        await LogCodeScanningAlerts(owner, name, cancellationToken)
            .NoSync();
        await LogSecretScanningAlerts(owner, name, cancellationToken)
            .NoSync();
    }

    public async ValueTask LogAllSecurityAlertsForAllRepos(string owner, DateTimeOffset? startAt = null, DateTimeOffset? endAt = null,
        CancellationToken cancellationToken = default)
    {
        List<MinimalRepository> repositories = await _gitHubRepositoriesUtil.GetAllForOwner(owner, startAt, endAt, cancellationToken)
                                                                            .NoSync();

        if (repositories.Count == 0)
            return;

        foreach (MinimalRepository repo in repositories)
        {
            if (string.IsNullOrEmpty(repo.Name))
                continue;
            await LogAllSecurityAlerts(owner, repo.Name, cancellationToken)
                .NoSync();
        }
    }

    public async ValueTask LogDependabotAlertsForAllRepos(string owner, DateTimeOffset? startAt = null, DateTimeOffset? endAt = null,
        CancellationToken cancellationToken = default)
    {
        List<MinimalRepository> repositories = await _gitHubRepositoriesUtil.GetAllForOwner(owner, startAt, endAt, cancellationToken)
                                                                            .NoSync();

        if (repositories.Count == 0)
            return;

        foreach (MinimalRepository repo in repositories)
        {
            if (string.IsNullOrEmpty(repo.Name))
                continue;
            await LogDependabotAlerts(owner, repo.Name, cancellationToken)
                .NoSync();
        }
    }

    public async ValueTask LogCodeScanningAlertsForAllRepos(string owner, DateTimeOffset? startAt = null, DateTimeOffset? endAt = null,
        CancellationToken cancellationToken = default)
    {
        List<MinimalRepository> repositories = await _gitHubRepositoriesUtil.GetAllForOwner(owner, startAt, endAt, cancellationToken)
                                                                            .NoSync();

        if (repositories.Count == 0)
            return;

        foreach (MinimalRepository repo in repositories)
        {
            if (string.IsNullOrEmpty(repo.Name))
                continue;
            await LogCodeScanningAlerts(owner, repo.Name, cancellationToken)
                .NoSync();
        }
    }

    public async ValueTask LogSecretScanningAlertsForAllRepos(string owner, DateTimeOffset? startAt = null, DateTimeOffset? endAt = null,
        CancellationToken cancellationToken = default)
    {
        List<MinimalRepository> repositories = await _gitHubRepositoriesUtil.GetAllForOwner(owner, startAt, endAt, cancellationToken)
                                                                            .NoSync();

        if (repositories.Count == 0)
            return;

        foreach (MinimalRepository repo in repositories)
        {
            if (string.IsNullOrEmpty(repo.Name))
                continue;
            await LogSecretScanningAlerts(owner, repo.Name, cancellationToken)
                .NoSync();
        }
    }

    private async ValueTask LogDependabotAlerts(string owner, string name, CancellationToken cancellationToken)
    {
        try
        {
            List<DependabotAlert> alerts = await GetDependabotAlerts(owner, name, "open", cancellationToken)
                .NoSync();

            if (alerts.Count == 0)
                return;

            foreach (DependabotAlert alert in alerts)
            {
                string? summary = alert.SecurityAdvisory?.Summary;
                string? severity = alert.SecurityVulnerability?.Severity?.ToString();
                _logger.LogInformation("{Repo}: [Dependabot] #{Number} severity: {Severity}, summary: {Summary}, updated: {UpdatedAt}", name, alert.Number,
                    severity, summary ?? "(none)", alert.UpdatedAt);
            }
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "{Repo}: Could not list Dependabot alerts (security features may be disabled)", name);
        }
    }

    private async ValueTask LogCodeScanningAlerts(string owner, string name, CancellationToken cancellationToken)
    {
        try
        {
            GitHubOpenApiClient client = await _gitHubClientUtil.Get(cancellationToken).NoSync();
            var page = 1;
            List<CodeScanningAlertItems>? alerts;

            do
            {
                alerts = await client.Repos[owner][name]
                    .CodeScanning.Alerts.GetAsync(config =>
                    {
                        config.QueryParameters.PerPage = 100;
                        config.QueryParameters.Page = page;
                        config.QueryParameters.State = CodeScanningAlertStateQuery.Open;
                    }, cancellationToken)
                    .NoSync();

                if (alerts is not { Count: > 0 })
                    break;

                foreach (CodeScanningAlertItems alert in alerts)
                {
                    _logger.LogInformation("{Repo}: [CodeScanning] #{Number} rule: {RuleId}, severity: {Severity}, created: {CreatedAt}", name, alert.Number,
                        alert.Rule?.Id, alert.Rule?.SecuritySeverityLevel?.ToString(), alert.CreatedAt);
                }

                page++;
            } while (alerts.Count == 100 && !cancellationToken.IsCancellationRequested);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "{Repo}: Could not list code scanning alerts (code scanning may be disabled)", name);
        }
    }

    private async ValueTask LogSecretScanningAlerts(string owner, string name, CancellationToken cancellationToken)
    {
        try
        {
            GitHubOpenApiClient client = await _gitHubClientUtil.Get(cancellationToken).NoSync();
            var page = 1;
            List<SecretScanningAlert>? alerts;

            do
            {
                alerts = await client.Repos[owner][name]
                    .SecretScanning.Alerts.GetAsync(config =>
                    {
                        config.QueryParameters.PerPage = 100;
                        config.QueryParameters.Page = page;
                        config.QueryParameters.State = GetStateQueryParameterType.Open;
                    }, cancellationToken)
                    .NoSync();

                if (alerts is not { Count: > 0 })
                    break;

                foreach (SecretScanningAlert alert in alerts)
                {
                    _logger.LogInformation("{Repo}: [SecretScanning] #{Number} secret type: {SecretType}, created: {CreatedAt}", name, alert.Number,
                        alert.SecretType, alert.CreatedAt);
                }

                page++;
            } while (alerts.Count == 100 && !cancellationToken.IsCancellationRequested);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "{Repo}: Could not list secret scanning alerts (secret scanning may be disabled)", name);
        }
    }
}