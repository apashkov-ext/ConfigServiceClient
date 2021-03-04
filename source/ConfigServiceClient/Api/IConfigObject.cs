namespace ConfigServiceClient.Api
{
    /// <summary>
    /// Hierarchycally structured configuration object.
    /// </summary>
    public interface IConfigObject
    {
        /// <summary>
        /// Returns property value.
        /// </summary>
        /// <typeparam name="T">Type of property value.</typeparam>
        /// <param name="path">Property path in dot notation</param>
        /// <example>
        /// <code>
        /// GetValue{bool}("options.logging.enabled")
        /// </code>
        /// </example>
        T GetValue<T>(string path);
        T SafeGetValue<T>(string path);
        IConfigObject GetNestedObject(string path);
        IConfigObject SafeGetNestedObject(string path);
    }
}