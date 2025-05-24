using System;

namespace Reactivity
{
    public class PropertyEventArgs<TProperty> : EventArgs
    {
        public TProperty NewValue;
        public TProperty OldValue;

        public PropertyEventArgs()
        {
        }

        public PropertyEventArgs(TProperty oldValue, TProperty newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}