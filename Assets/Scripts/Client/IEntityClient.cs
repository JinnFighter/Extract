using Logic.Components;

namespace Client
{
    public interface IEntityClient
    {
        IEntityPropertyClient Get(EPropertyType propertyType);
        void Set(EPropertyType type, int value);
    }
}