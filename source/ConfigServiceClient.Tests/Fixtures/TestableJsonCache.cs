using System;
using System.Collections.Generic;
using System.Linq;
using ConfigServiceClient.Persistence.LocalCaching;

namespace ConfigServiceClient.Tests.Fixtures
{
    internal class TestableJsonCache : ICache<string>
    {
        private readonly Dictionary<string, CacheEntry<string>> _cache = new Dictionary<string, CacheEntry<string>>();

        public TestableJsonCache() { }

        public TestableJsonCache(Dictionary<string, CacheEntry<string>> entries)
        {
            _cache = entries;
        }

        public void Put(string key, string content)
        {
            _cache[key] = new CacheEntry<string> { Id = key, Modified = DateTime.Now, Content = content };
        }

        public CacheEntry<string> Get(string key)
        {
            return _cache.ContainsKey(key) ? _cache[key] : null;
        }

        public CacheEntry<string> Last()
        {
            return _cache.Last().Value;
        }
    }
}
