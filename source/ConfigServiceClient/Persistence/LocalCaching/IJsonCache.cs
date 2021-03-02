namespace ConfigServiceClient.Persistence.LocalCaching
{
    public interface IJsonCache
    {
        void Put(string key, string content);
        JsonCacheEntry Get(string key);
    }
}
