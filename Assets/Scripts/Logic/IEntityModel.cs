using Logic.Components;

namespace Logic
{
    public interface IEntityModel
    {
        IEntityProperty Get(EPropertyType propertyType);
        void Set(EPropertyType type, int value);
    }
}
