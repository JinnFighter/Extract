namespace Reactivity
{
    public class GenericPairEventArgs<TKey, TValue> : GenericEventArg<TValue>
    {
        public TKey Key;

        public GenericPairEventArgs()
        {
        }

        public GenericPairEventArgs(TKey key, TValue value) : base(value)
        {
            Key = key;
        }
    }
}