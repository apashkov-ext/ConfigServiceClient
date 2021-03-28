using System;
using System.IO;
using ConfigServiceClient.Options;
using LiteDB;

namespace ConfigServiceClient.Persistence.LocalCaching
{
    /// <summary>
    /// Json configuration cache.
    /// </summary>
    public class JsonCache : ICache<string>
    {
        private const string StorageName = "configurationCache.db";
        private readonly string _project;

        /// <summary>
        /// Creates new instance of Json configuration cache.
        /// </summary>
        /// <param name="options">Client options.</param>
        public JsonCache(ConfigClientOptions options)
        {
            _project = options.Project;
        }

        public void Put(string key, string content)
        {
            using (var db = new LiteDatabase(Path.Combine(CreateAndGetStorageDir(), StorageName)))
            {
                var col = db.GetCollection<CacheEntry<string>>(_project);
                var entry = col.FindById(key) ?? new CacheEntry<string> { Id = key, Content = content };

                entry.Modified = DateTime.Now;
                col.Upsert(entry);
            }
        }

        public CacheEntry<string> Get(string key)
        {
            using (var db = new LiteDatabase(Path.Combine(CreateAndGetStorageDir(), StorageName)))
            {
                return db.GetCollection<CacheEntry<string>>(_project).FindById(key);
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
