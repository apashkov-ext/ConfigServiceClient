using System;

namespace ConfigServiceClient.Core.Exceptions
{
    public class InvalidPropertyTypeException : ApplicationException
    {
        private InvalidPropertyTypeException(string message) : base(message) { }

        public static InvalidPropertyTypeException Create(string target, string fullPath, Type requestedType)
        {
            return new InvalidPropertyTypeException($"Property \"{fullPath}\" of object \"{target}\" does not match requested type \"{requestedType.Name}\"");
        }
    }
}