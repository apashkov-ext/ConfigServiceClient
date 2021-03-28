namespace ConfigServiceClient.Core.Models
{
    /// <summary>
    /// Grouped options (nested options).
    /// </summary>
    public interface IOptionGroup
    {
        /// <summary>
        /// Group name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns contained option by name.
        /// </summary>
        /// <param name="name">Option name.</param>
        Option FindOption(string name);

        /// <summary>
        /// Returns nested option group by name.
        /// </summary>
        /// <param name="name">Nested option group name.</param>
        IOptionGroup FindNested(string name);

        /// <summary>
        /// Returns true if current option group equals with the specified option group.
        /// </summary>
        /// <param name="group">Specified option group.</param>
        bool Equals(IOptionGroup group);
    }
}