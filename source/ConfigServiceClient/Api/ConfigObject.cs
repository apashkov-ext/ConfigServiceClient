using System;
using ConfigServiceClient.Abstractions;
using ConfigServiceClient.Core.Exceptions;
using ConfigServiceClient.Core.Models;

namespace ConfigServiceClient.Api
{
    public class ConfigObject : IConfigObject
    {
        private readonly OptionGroup _root;

        internal ConfigObject(OptionGroup root)
        {
            _root = root ?? throw new ArgumentNullException(nameof(root));
        }

        public object GetProperty(string path)
        {
            var splitted = SplitPath(path);
            var opt = new ElementFinder<Option>(x => x.FindOption).Find(_root, splitted) ?? throw InvalidPathException.Create(_root.Name, path);

            return opt.Value;
        }

        public object SafeGetProperty(string path)
        {
            try
            {
                return GetProperty(path);
            }
            catch
            {
                return null;
            }
        }

        public T GetProperty<T>(string path)
        {
            var value = GetProperty(path);
            var type = typeof(T);
            if (type != value.GetType())
            {
                throw InvalidPropertyTypeException.Create(_root.Name, path, type);
            }

            return (T) value;
        }

        public T SafeGetProperty<T>(string path)
        {
            try
            {
                return GetProperty<T>(path);
            }
            catch
            {
                return default;
            }
        }

        public IConfigObject GetNestedObject(string path)
        {
            var splitted = SplitPath(path);
            var group = new ElementFinder<OptionGroup>(x => x.FindNested).Find(_root, splitted) ?? throw InvalidPathException.Create(_root.Name, path);

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