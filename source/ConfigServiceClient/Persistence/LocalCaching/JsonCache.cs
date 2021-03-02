using System;
using LiteDB;

namespace ConfigServiceClient.Persistence.LocalCaching
{
    public class JsonCache : IJsonCache
    {
        private readonly string _project;

        public JsonCache(string project)
        {
            _project = project;
        }

        public void Put(string key, string content)
        {
            using (var db = new LiteDatabase("cfgStorage.db"))
            {
                var col = db.GetCollection<JsonCacheEntry>(_project);
                var entry = col.FindById(key) ?? new JsonCacheEntry { Id = key, Content = content };

                entry.Modified = DateTime.Now;
                col.Upsert(entry);
            }
        }

        public JsonCacheEntry Get(string key)
        {
            using (var db = new LiteDatabase("cfgStorage.db"))
            {
                return db.GetCollection<JsonCacheEntry>(_project).FindById(key);
            }
        }
    }
}
