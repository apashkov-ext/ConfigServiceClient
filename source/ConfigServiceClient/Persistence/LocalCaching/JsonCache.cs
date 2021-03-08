using System;
using System.IO;
using LiteDB;

namespace ConfigServiceClient.Persistence.LocalCaching
{
    public class JsonCache : IJsonCache
    {
        private const string StorageName = "configurationCache.db";
        private readonly string _project;

        public JsonCache(string project)
        {
            _project = project;
        }

        public void Put(string key, string content)
        {
            using (var db = new LiteDatabase(GetStoragePath(StorageName)))
            {
                var col = db.GetCollection<JsonCacheEntry>(_project);
                var entry = col.FindById(key) ?? new JsonCacheEntry { Id = key, Content = content };

                entry.Modified = DateTime.Now;
                col.Upsert(entry);
            }
        }

        public JsonCacheEntry Get(string key)
        {
            using (var db = new LiteDatabase(GetStoragePath(StorageName)))
            {
                return db.GetCollection<JsonCacheEntry>(_project).FindById(key);
            }
        }

        private static string GetStoragePath(string fileName)
        {
            const string appDir = "ConfigServiceClient";
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.DoNotVerify);
            return Path.Combine(appData, appDir, fileName);
        }
    }
}
