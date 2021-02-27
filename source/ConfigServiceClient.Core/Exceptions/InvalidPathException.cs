using System;

namespace ConfigServiceClient.Core.Exceptions
{
    public class InvalidPathException : ApplicationException
    {
        private InvalidPathException(string message) : base(message) { }

        public static InvalidPathException Create(string target, string path)
        {
            return new InvalidPathException($"Path \"{path}\" does not exist for object \"{target}\"");
        }
    }
}
