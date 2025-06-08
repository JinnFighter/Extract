namespace Logic.Components
{
    public interface IPropertyComponent
    {
        public EPropertyType EventType { get; }
        public int Value { get; set; }
    }
}