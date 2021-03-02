namespace ConfigServiceClient.Core.Models
{
    public sealed class Option
    {
        public string Name { get; }
        public object Value { get; }

        public Option(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
