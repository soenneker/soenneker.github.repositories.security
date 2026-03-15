using Soenneker.GitHub.Repositories.Security.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.GitHub.Repositories.Security.Tests;

[Collection("Collection")]
public sealed class GitHubRepositoriesSecurityUtilTests : FixturedUnitTest
{
    private readonly IGitHubRepositoriesSecurityUtil _util;

    public GitHubRepositoriesSecurityUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IGitHubRepositoriesSecurityUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
