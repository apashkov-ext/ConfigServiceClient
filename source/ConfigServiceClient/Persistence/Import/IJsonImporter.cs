namespace ConfigServiceClient.Persistence.Import
{
    public interface IJsonImporter<out TResult> where TResult : class
    {
        TResult ImportFromJson(string json);
    }
}