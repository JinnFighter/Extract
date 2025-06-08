using System;
using Logic.Components;

namespace Logic
{
    public abstract class BaseEntityProperty<T> : IEntityProperty
    {
        private T _value;
        public EPropertyType PropertyType { get; set; }

        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value)) return;
                _value = value;
                OnChange?.Invoke(value);
            }
        }

        public event Action<T> OnChange;
    }
}