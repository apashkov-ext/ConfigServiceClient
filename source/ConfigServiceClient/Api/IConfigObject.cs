namespace ConfigServiceClient.Api
{
    public interface IConfigObject
    {
        object GetProperty(string path);
        object SafeGetProperty(string path);
        T GetProperty<T>(string path);
        T SafeGetProperty<T>(string path);
        IConfigObject GetNestedObject(string path);
        IConfigObject SafeGetNestedObject(string path);
    }
}