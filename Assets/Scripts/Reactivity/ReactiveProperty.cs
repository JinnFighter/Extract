using System;

namespace Reactivity
{
    public class ReactiveProperty<T> : IReactiveProperty<T>
    {
        private readonly GenericEventArg<T> _args;
        private readonly EventDispatchMode _eventDispatchMode;
        private readonly PropertyEventArgs<T> _extendedArgs;
        private readonly object _sender;

        private T _value;

        public ReactiveProperty(EventDispatchMode eventDispatchMode = EventDispatchMode.OnValueChanged)
        {
            _args = new GenericEventArg<T>(default);
            _extendedArgs = new PropertyEventArgs<T>(default, default);
            _eventDispatchMode = eventDispatchMode;
        }

        public ReactiveProperty(T value, EventDispatchMode eventDispatchMode = EventDispatchMode.OnValueChanged) :
            this()
        {
            _value = value;
            _eventDispatchMode = eventDispatchMode;
        }

        public ReactiveProperty(T value, object sender,
            EventDispatchMode eventDispatchMode = EventDispatchMode.OnValueChanged) : this(value)
        {
            _sender = sender;
            _eventDispatchMode = eventDispatchMode;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (_value == null || !_value.Equals(value) || _eventDispatchMode == EventDispatchMode.Always)
                {
                    _extendedArgs.OldValue = _value;
                    _value = value;
                    _extendedArgs.NewValue = _value;
                    _args.Value = _value;

                    OnValueChanged?.Invoke(_sender ?? this, _args);
                    OnValueChangedExtended?.Invoke(_sender ?? this, _extendedArgs);
                }
            }
        }

        public event EventHandler<GenericEventArg<T>> OnValueChanged;
        public event EventHandler<PropertyEventArgs<T>> OnValueChangedExtended;


        private T GetValue()
        {
            return _value;
        }

        public override string ToString()
        {
            return _value != null ? _value.ToString() : "NULL value";
        }

        public static implicit operator T(ReactiveProperty<T> value)
        {
            return value.GetValue();
        }
    }
}