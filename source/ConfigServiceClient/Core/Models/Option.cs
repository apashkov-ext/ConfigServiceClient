namespace ConfigServiceClient.Core.Models
{
    /// <summary>
    /// Object that represents configuration parameter.
    /// </summary>
    public sealed class Option
    {
        /// <summary>
        /// Option name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Option value.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Creates an instance of the option.
        /// </summary>
        /// <param name="name">Option name.</param>
        /// <param name="value">Option value.</param>
        public Option(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
