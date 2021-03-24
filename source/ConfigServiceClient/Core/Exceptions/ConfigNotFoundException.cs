using System;

namespace ConfigServiceClient.Core.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the configuration for the specified environment was not found.
    /// </summary>
    public class ConfigNotFoundException : ApplicationException
    {
        private ConfigNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Creates the instance of exception.
        /// </summary>
        /// <param name="message">Exception message</param>
        public static ConfigNotFoundException Create(string message)
        {
            return new ConfigNotFoundException(message);
        }
    }
}
