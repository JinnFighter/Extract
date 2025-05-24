using System;

namespace Reactivity
{
    public class GenericEventArg<TValue> : EventArgs
    {
        public TValue Value;

        #region Constructors

        public GenericEventArg()
        {
        }

        public GenericEventArg(TValue value)
        {
            Value = value;
        }

        #endregion
    }
}