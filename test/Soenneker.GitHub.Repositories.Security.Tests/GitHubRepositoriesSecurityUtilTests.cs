using Soenneker.GitHub.Repositories.Security.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.GitHub.Repositories.Security.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class GitHubRepositoriesSecurityUtilTests : HostedUnitTest
{
    private readonly IGitHubRepositoriesSecurityUtil _util;

    public GitHubRepositoriesSecurityUtilTests(Host host) : base(host)
    {
        _util = Resolve<IGitHubRepositoriesSecurityUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}
