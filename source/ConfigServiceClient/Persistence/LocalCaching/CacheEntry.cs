using System;

namespace ConfigServiceClient.Persistence.LocalCaching
{
    public class CacheEntry<T>
    {
        public string Id { get; set; }
        public DateTime Modified { get; set; }
        public T Content { get; set; }
    }
}
