using System.Text.Json;

namespace ConfigServiceClient.Persistence
{
    public interface IJsonImporter<out TResult> where TResult : class
    {
        TResult ImportFromJson(string json);
    }
}