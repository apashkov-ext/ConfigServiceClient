namespace ConfigServiceClient.Api
{
    /// <summary>
    /// Hierarchycally structured configuration object.
    /// Wrapper over JSON-like object.
    /// </summary>
    public interface IConfigObject
    {
        /// <summary>
        /// Returns property value. Throws exception if property does not exist.
        /// </summary>
        /// <typeparam name="T">Type of property value.</typeparam>
        /// <param name="path">Property path in dot notation</param>
        T GetValue<T>(string path);

        /// <summary>
        /// Returns property value. Does not throw exception if property does not exist but returns null.
        /// </summary>
        /// <typeparam name="T">Type of property value.</typeparam>
        /// <param name="path">Property path in dot notation</param>
        T SafeGetValue<T>(string path);

        /// <summary>
        /// Returns property value as inner object. Throws exception if property does not exist.
        /// </summary>
        /// <param name="path">Property path in dot notation</param>
        IConfigObject GetNestedObject(string path);

        /// <summary>
        /// Returns property value as inner object. Does not throw exception if property does not exist but returns null.
        /// </summary>
        /// <param name="path">Property path in dot notation</param>
        IConfigObject SafeGetNestedObject(string path);
    }
}