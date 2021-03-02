using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigServiceClient.Core.Models
{
    public sealed class OptionGroup : IOptionGroup, IOptionGroupBuilder
    {
        private readonly List<Option> _options = new List<Option>();
        private readonly List<IOptionGroup> _nestedGroups = new List<IOptionGroup>();

        public string Name { get; }

        public OptionGroup(string name)
        {
            Name = name;
        }

        public Option FindOption(string name)
        {
            return _options.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public IOptionGroup FindNested(string name)
        {
            return _nestedGroups.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public IOptionGroupBuilder AddNested(string name)
        {
            var group = new OptionGroup(name);
            _nestedGroups.Add(group);
            return group;
        }

        public Option AddOption(string name, object value)
        {
            var opt = new Option(name, value);
            _options.Add(opt);
            return opt;
        }
    }
}