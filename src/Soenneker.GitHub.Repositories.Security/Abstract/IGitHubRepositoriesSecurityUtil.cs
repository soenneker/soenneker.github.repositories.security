using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.GitHub.OpenApiClient.Models;

namespace Soenneker.GitHub.Repositories.Security.Abstract;

/// <summary>
/// A utility library for GitHub Repository Security related operations
/// </summary>
public interface IGitHubRepositoriesSecurityUtil
{
    /// <summary>
    /// Retrieves all Dependabot alerts for the specified repository.
    /// </summary>
    /// <param name="owner">The owner of the repository.</param>
    /// <param name="name">The name of the repository.</param>
    /// <param name="state">Optional state filter (e.g. "open"). If null, returns all states.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of Dependabot alerts.</returns>
    ValueTask<List<DependabotAlert>> GetDependabotAlerts(string owner, string name, string? state = "open",
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all code scanning alerts for the specified repository.
    /// </summary>
    /// <param name="owner">The owner of the repository.</param>
    /// <param name="name">The name of the repository.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of code scanning alert items.</returns>
    ValueTask<List<CodeScanningAlertItems>> GetCodeScanningAlerts(string owner, string name,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all secret scanning alerts for the specified repository.
    /// </summary>
    /// <param name="owner">The owner of the repository.</param>
    /// <param name="name">The name of the repository.</param>
    /// <param name="state">Optional state filter ("open" or "resolved"). If null, returns all states.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of secret scanning alerts.</returns>
    ValueTask<List<SecretScanningAlert>> GetSecretScanningAlerts(string owner, string name, string? state = "open",
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs all security alerts (Dependabot, code scanning, secret scanning) for the specified repository.
    /// </summary>
    /// <param name="owner">The owner of the repository.</param>
    /// <param name="name">The name of the repository.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    ValueTask LogAllSecurityAlerts(string owner, string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs all security alerts for all repositories owned by the specified user or organization.
    /// </summary>
    /// <param name="owner">The owner of the repositories.</param>
    /// <param name="startAt">Optional filter to restrict to repositories created after this time.</param>
    /// <param name="endAt">Optional filter to restrict to repositories created before this time.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    ValueTask LogAllSecurityAlertsForAllRepos(string owner, DateTimeOffset? startAt = null, DateTimeOffset? endAt = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs Dependabot alerts for all repositories owned by the specified user or organization.
    /// </summary>
    /// <param name="owner">The owner of the repositories.</param>
    /// <param name="startAt">Optional filter to restrict to repositories created after this time.</param>
    /// <param name="endAt">Optional filter to restrict to repositories created before this time.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    ValueTask LogDependabotAlertsForAllRepos(string owner, DateTimeOffset? startAt = null, DateTimeOffset? endAt = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs code scanning alerts for all repositories owned by the specified user or organization.
    /// </summary>
    /// <param name="owner">The owner of the repositories.</param>
    /// <param name="startAt">Optional filter to restrict to repositories created after this time.</param>
    /// <param name="endAt">Optional filter to restrict to repositories created before this time.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    ValueTask LogCodeScanningAlertsForAllRepos(string owner, DateTimeOffset? startAt = null, DateTimeOffset? endAt = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs secret scanning alerts for all repositories owned by the specified user or organization.
    /// </summary>
    /// <param name="owner">The owner of the repositories.</param>
    /// <param name="startAt">Optional filter to restrict to repositories created after this time.</param>
    /// <param name="endAt">Optional filter to restrict to repositories created before this time.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    ValueTask LogSecretScanningAlertsForAllRepos(string owner, DateTimeOffset? startAt = null, DateTimeOffset? endAt = null,
        CancellationToken cancellationToken = default);
}
