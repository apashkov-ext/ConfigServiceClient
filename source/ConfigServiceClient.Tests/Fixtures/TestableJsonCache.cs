using System;
using System.Collections.Generic;
using System.Linq;
using ConfigServiceClient.Persistence.LocalCaching;

namespace ConfigServiceClient.Tests.Fixtures
{
    internal class TestableJsonCache : IJsonCache
    {
        private readonly Dictionary<string, JsonCacheEntry> _cache = new Dictionary<string, JsonCacheEntry>();

        public TestableJsonCache() { }

        public TestableJsonCache(Dictionary<string, JsonCacheEntry> entries)
        {
            _cache = entries;
        }

        public void Put(string key, string content)
        {
            _cache[key] = new JsonCacheEntry { Id = key, Modified = DateTime.Now, Content = content };
        }

        public JsonCacheEntry Get(string key)
        {
            return _cache.ContainsKey(key) ? _cache[key] : null;
        }

        public JsonCacheEntry Last()
        {
            return _cache.Last().Value;
        }
    }
}
