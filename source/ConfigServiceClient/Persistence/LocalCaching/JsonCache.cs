using System;
using System.IO;
using ConfigServiceClient.Options;
using LiteDB;

namespace ConfigServiceClient.Persistence.LocalCaching
{
    public class JsonCache : IJsonCache
    {
        private const string StorageName = "configurationCache.db";
        private readonly string _project;

        public JsonCache(ConfigClientOptions options)
        {
            _project = options.Project;
        }

        public void Put(string key, string content)
        {
            using (var db = new LiteDatabase(Path.Combine(CreateAndGetStorageDir(), StorageName)))
            {
                var col = db.GetCollection<JsonCacheEntry>(_project);
                var entry = col.FindById(key) ?? new JsonCacheEntry { Id = key, Content = content };

                entry.Modified = DateTime.Now;
                col.Upsert(entry);
            }
        }

        public JsonCacheEntry Get(string key)
        {
            using (var db = new LiteDatabase(Path.Combine(CreateAndGetStorageDir(), StorageName)))
            {
                return db.GetCollection<JsonCacheEntry>(_project).FindById(key);
            }
        }

        private static string CreateAndGetStorageDir()
        {
            const string appDir = "ConfigServiceClient";
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.DoNotVerify);
            var path = Path.Combine(appData, appDir);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}
