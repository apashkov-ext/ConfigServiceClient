using System;
using System.Collections.Generic;
using ConfigServiceClient.Api;
using ConfigServiceClient.Core.Models;
using ConfigServiceClient.Tests.Fixtures;
using Xunit;

namespace ConfigServiceClient.Tests.UnitTests
{
    public class OptionGroupChildElementFinderTests
    {
        [Fact]
        public void FindOption_NotContains_ReturnsNull()
        {
            var parent = new TestableOptionGroup("", new List<Option>(), new List<IOptionGroup>());
            var finder = new OptionGroupChildElementFinder<Option>(x => x.FindOption);

            var res = finder.Find(parent, "property");

            Assert.Null(res);
        }

        [Fact]
        public void FindNested_NotContains_ReturnsNull()
        {
            var parent = new TestableOptionGroup("", new List<Option>(), new List<IOptionGroup>());
            var finder = new OptionGroupChildElementFinder<IOptionGroup>(x => x.FindNested);

            var res = finder.Find(parent, "nested");

            Assert.Null(res);
        }

        [Fact]
        public void FindOption_ContainsInRoot_ReturnsOption()
        {
            var expected = new Option("enabled", true);
            var parent = new TestableOptionGroup("", new List<Option>{ expected }, new List<IOptionGroup>());
            var finder = new OptionGroupChildElementFinder<Option>(x => x.FindOption);

            var res = finder.Find(parent, "enabled");

            Assert.Equal(expected, res);
        }

        [Fact]
        public void FindOption_ContainsInNested_ReturnsOption()
        {
            var expected = new Option("enabled", true);
            var parent = new TestableOptionGroup("", new List<Option>(),
                new List<IOptionGroup>
                {
                    new TestableOptionGroup("nested", new List<Option>(), new List<IOptionGroup>
                        {
                            new TestableOptionGroup("logging", new List<Option> {expected}, new List<IOptionGroup>())
                        })
                });
            var finder = new OptionGroupChildElementFinder<Option>(x => x.FindOption);

            var res = finder.Find(parent, "nested", "logging", "enabled");

            Assert.Equal(expected, res);
        }

        [Fact]
        public void FindNested_ContainsInNested_ReturnsOption()
        {
            var expected = new TestableOptionGroup("logging", new List<Option> { new Option("enabled", true) }, new List<IOptionGroup>());
            var parent = new TestableOptionGroup("", new List<Option>(),
                new List<IOptionGroup>
                {
                    new TestableOptionGroup("nested", new List<Option>(), new List<IOptionGroup> { expected })
                });
            var finder = new OptionGroupChildElementFinder<IOptionGroup>(x => x.FindNested);

            var res = finder.Find(parent, "nested", "logging");

            Assert.Equal(expected, res);
        }

        [Fact]
        public void Find_InvalidPathSegment_ThrowsEx()
        {
            var parent = new TestableOptionGroup("", new List<Option>(), new List<IOptionGroup>());
            var finder = new OptionGroupChildElementFinder<Option>(x => x.FindOption);

            Assert.Throws<ApplicationException>(() => finder.Find(parent, "property"));
        }
    }
}
