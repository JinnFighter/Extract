using System;
using Logic;

namespace Client
{
    public abstract class BaseEntityPropertyClient<T> : IEntityPropertyClient
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