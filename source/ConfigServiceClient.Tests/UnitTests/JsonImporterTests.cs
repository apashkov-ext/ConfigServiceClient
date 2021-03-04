using System.Collections.Generic;
using ConfigServiceClient.Core.Models;
using ConfigServiceClient.Persistence.Import;
using ConfigServiceClient.Tests.Fixtures;
using Xunit;

namespace ConfigServiceClient.Tests.UnitTests
{
    public class JsonImporterTests
    {
        [Fact]
        public void ImportFromJson_RootOnly_ReturnsCorrectResult()
        {
            const string json = "{}";
            var importer = new JsonImporter();

            var res = importer.ImportFromJson(json);

            Assert.NotNull(res);
        }

        [Fact]
        public void ImportFromJson_RootOnly2_ReturnsCorrectResult()
        {
            const string json = "{}";
            var importer = new JsonImporter();

            var res = importer.ImportFromJson(json);

            Assert.IsAssignableFrom<IOptionGroup>(res);
        }

        [Fact]
        public void ImportFromJson_WithProperty_ReturnsNotNullOption()
        {
            const string name = "prop";
            const string json = "{\"" + name + "\":\"test\"}";
            var importer = new JsonImporter();

            var res = importer.ImportFromJson(json);
            var opt = res.FindOption(name);

            Assert.NotNull(opt);
        }

        [Fact]
        public void ImportFromJson_WithProperty_ReturnsExistedOption()
        {
            const string name = "prop";
            const string json = "{\"" + name + "\":\"test\"}";
            var importer = new JsonImporter();

            var res = importer.ImportFromJson(json);
            var opt = res.FindOption(name);

            Assert.Equal(opt.Name, name);
        }

        [Fact]
        public void ImportFromJson_WithEmptyNested_ReturnsNotNullNestedObj()
        {
            const string name = "nested";
            const string json = "{\"" + name + "\":{}}";
            var importer = new JsonImporter();

            var res = importer.ImportFromJson(json);
            var nested = res.FindNested(name);

            Assert.NotNull(nested);
        }

        [Fact]
        public void ImportFromJson_WithEmptyNested_ReturnsNestedByName()
        {
            const string name = "nested";
            const string json = "{\"" + name + "\":{}}";
            var importer = new JsonImporter();

            var res = importer.ImportFromJson(json);
            var nested = res.FindNested(name);

            Assert.Equal(name, nested.Name);
        }

        [Fact]
        public void ImportFromJson_MultiLevelStructure_ReturnsObjectWithTheSameStructure()
        {
            const string json = "{\"name\":\"prop\", \"nested\":{\"logging\":{\"enabled\":true}}}";

            var expected = new TestableOptionGroup("", new List<Option>
                {
                    new Option("name", "prop")
                },
                new List<IOptionGroup>
                {
                    new TestableOptionGroup("nested", new List<Option>(), new List<IOptionGroup>
                    {
                        new TestableOptionGroup("logging", new List<Option> {new Option("enabled", true)}, new List<IOptionGroup>())
                    })
                }
            );

            var importer = new JsonImporter();
            var res = importer.ImportFromJson(json);

            Assert.Equal(expected, res, new OptionGroupEqualityComparer());
        }
    }

    internal class OptionGroupEqualityComparer : IEqualityComparer<IOptionGroup>
    {
        public bool Equals(IOptionGroup x, IOptionGroup y)
        {
            if (x == null && y == null)
            {
                return false;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public int GetHashCode(IOptionGroup obj)
        {
            return obj.GetHashCode();
        }
    }
}
