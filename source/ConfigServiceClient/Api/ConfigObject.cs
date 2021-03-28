using System;
using ConfigServiceClient.Core.Exceptions;
using ConfigServiceClient.Core.Models;

namespace ConfigServiceClient.Api
{
    /// <inheritdoc cref="IConfigObject"/>
    public class ConfigObject : IConfigObject
    {
        private readonly IOptionGroup _root;

        internal ConfigObject(IOptionGroup root)
        {
            _root = root ?? throw new ArgumentNullException(nameof(root));
        }

        public T GetValue<T>(string path)
        {
            var value = GetProperty(path);
            var type = typeof(T);
            if (type != value.GetType())
            {
                throw InvalidPropertyTypeException.Create(_root.Name, path, type);
            }

            return (T) value;
        }

        private object GetProperty(string path)
        {
            var splitted = SplitPath(path);
            var opt = new OptionGroupChildElementFinder<Option>(x => x.FindOption).Find(_root, splitted) ?? throw InvalidPathException.Create(_root.Name, path);

            return opt.Value;
        }

        public T SafeGetValue<T>(string path)
        {
            try
            {
                return GetValue<T>(path);
            }
            catch
            {
                return default;
            }
        }

        public IConfigObject GetNestedObject(string path)
        {
            var splitted = SplitPath(path);
            var group = new OptionGroupChildElementFinder<IOptionGroup>(x => x.FindNested).Find(_root, splitted) ?? throw InvalidPathException.Create(_root.Name, path);

            return new ConfigObject(group);
        }

        public IConfigObject SafeGetNestedObject(string path)
        {
            try
            {
                return SafeGetNestedObject(path);
            }
            catch
            {
                return null;
            }
        }

        private string[] SplitPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw InvalidPathException.Create(_root.Name, path);
            }

            return path.Split('.');
        }
    }
}