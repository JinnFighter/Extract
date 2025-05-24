using System;

namespace Reactivity
{
    public interface IReactiveProperty<T>
    {
        T Value { get; }

        event EventHandler<GenericEventArg<T>> OnValueChanged;
        event EventHandler<PropertyEventArgs<T>> OnValueChangedExtended;
    }
}