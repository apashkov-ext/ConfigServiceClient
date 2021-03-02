namespace ConfigServiceClient.Core.Models
{
    public interface IOptionGroupBuilder
    {
        IOptionGroupBuilder AddNested(string name);
        Option AddOption(string name, object value);
    }
}