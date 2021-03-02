using System;

namespace ConfigServiceClient.Persistence.LocalCaching
{
    public class JsonCacheEntry
    {
        public string Id { get; set; }
        public DateTime Modified { get; set; }
        public string Content { get; set; }
    }
}
