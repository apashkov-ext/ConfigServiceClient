using System.Collections.Generic;
using ConfigServiceClient.Core.Models;

namespace ConfigServiceClient.Tests.Fixtures
{
    internal class TestableOptionGroup : OptionGroup
    {
        public TestableOptionGroup(string name, IEnumerable<Option> options, IEnumerable<IOptionGroup> nestedGroups) : base(name, options, nestedGroups)
        {
        }
    }
}
