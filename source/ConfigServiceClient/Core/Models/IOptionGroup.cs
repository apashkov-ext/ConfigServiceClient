namespace ConfigServiceClient.Core.Models
{
    public interface IOptionGroup
    {
        string Name { get; }
        Option FindOption(string name);
        IOptionGroup FindNested(string name);
        bool Equals(IOptionGroup group);
    }
}