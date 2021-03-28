namespace ConfigServiceClient.Persistence.LocalCaching
{
    /// <summary>
    /// Cached data storage.
    /// </summary>
    public interface ICache<T>
    {
        /// <summary>
        /// Adds or updates cached data by key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="content">Data.</param>
        void Put(string key, T content);

        /// <summary>
        /// Returns cached entry by key.
        /// </summary>
        /// <param name="key">Key.</param>
        CacheEntry<T> Get(string key);
    }
}
