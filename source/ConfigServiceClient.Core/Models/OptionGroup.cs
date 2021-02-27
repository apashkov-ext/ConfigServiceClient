using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigServiceClient.Core.Models
{
    public sealed class OptionGroup
    {
        public string Name { get; set; }
        public IEnumerable<Option> Options { get; set; }
        public IEnumerable<OptionGroup> NestedGroups { get; set; }

        public Option FindOption(string name)
        {
            return Options.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public OptionGroup FindNested(string name)
        {
            return NestedGroups.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}