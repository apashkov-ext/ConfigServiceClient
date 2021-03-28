namespace ConfigServiceClient.Persistence.Import
{
    /// <summary>
    /// Json parser.
    /// </summary>
    /// <typeparam name="TResult">Parsed object type.</typeparam>
    public interface IJsonParser<out TResult> where TResult : class
    {
        /// <summary>
        /// Returns parsed object.
        /// </summary>
        /// <param name="json">Json string.</param>
        TResult Parse(string json);
    }
}