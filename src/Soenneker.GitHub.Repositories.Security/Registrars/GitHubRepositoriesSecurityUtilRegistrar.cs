using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.GitHub.Repositories.Registrars;
using Soenneker.GitHub.Repositories.Security.Abstract;

namespace Soenneker.GitHub.Repositories.Security.Registrars;

/// <summary>
/// A utility library for GitHub Repository Security related operations
/// </summary>
public static class GitHubRepositoriesSecurityUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IGitHubRepositoriesSecurityUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddGitHubRepositoriesSecurityUtilAsSingleton(this IServiceCollection services)
    {
        services.AddGitHubRepositoriesUtilAsSingleton().TryAddSingleton<IGitHubRepositoriesSecurityUtil, GitHubRepositoriesSecurityUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IGitHubRepositoriesSecurityUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddGitHubRepositoriesSecurityUtilAsScoped(this IServiceCollection services)
    {
        services.AddGitHubRepositoriesUtilAsScoped().TryAddScoped<IGitHubRepositoriesSecurityUtil, GitHubRepositoriesSecurityUtil>();

        return services;
    }
}
