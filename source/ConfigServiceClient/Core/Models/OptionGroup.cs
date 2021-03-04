using System;
using System.Collections.Generic;
using System.Linq;

namespace ConfigServiceClient.Core.Models
{
    public class OptionGroup : IOptionGroup, IOptionGroupBuilder
    {
        protected readonly List<Option> Options = new List<Option>();
        protected readonly List<IOptionGroup> NestedGroups = new List<IOptionGroup>();

        public string Name { get; }

        public OptionGroup(string name)
        {
            Name = name;
        }

        protected OptionGroup(string name, IEnumerable<Option> options, IEnumerable<IOptionGroup> nestedGroups)
        {
            Name = name;
            Options = options.ToList();
            NestedGroups = nestedGroups.ToList();
        }

        public Option FindOption(string name)
        {
            return Options.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public IOptionGroup FindNested(string name)
        {
            return NestedGroups.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public bool Equals(IOptionGroup group)
        {
            if (group == null)
            {
                return false;
            }

            if (!Name.Equals(group.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            foreach (var o in Options)
            {
                if (group.FindOption(o.Name) == null)
                {
                    return false;
                }
            }

            foreach (var nested in NestedGroups)
            {
                var existed = group.FindNested(nested.Name);
                if (existed == null)
                {
                    return false;
                }

                if (!nested.Equals(existed))
                {
                    return false;
                }
            }

            return true;
        }

        public IOptionGroupBuilder AddNested(string name)
        {
            var group = new OptionGroup(name);
            NestedGroups.Add(group);
            return group;
        }

        public Option AddOption(string name, object value)
        {
            var opt = new Option(name, value);
            Options.Add(opt);
            return opt;
        }
    }
}