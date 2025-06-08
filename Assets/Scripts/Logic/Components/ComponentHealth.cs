namespace Logic.Components
{
    public struct ComponentHealth : IPropertyComponent
    {
        public EPropertyType EventType => EPropertyType.Health;
        public int Value { get; set; }
    }
}