using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FunctionalTests
{
    [CollectionDefinition("server")]
    public class HostCollectionFixture
       : ICollectionFixture<HostFixture>
    {
    }
}
