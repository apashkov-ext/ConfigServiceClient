using System;

namespace ConfigServiceClient.Core.Exceptions
{
    public class ConfigNotFoundException : ApplicationException
    {
        private ConfigNotFoundException(string message) : base(message) { }

        public static ConfigNotFoundException Create(string message)
        {
            return new ConfigNotFoundException(message);
        }
    }
}
