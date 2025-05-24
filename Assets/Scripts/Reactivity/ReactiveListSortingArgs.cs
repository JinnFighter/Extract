using System;

namespace Reactivity
{
    public class ReactiveListSortingArgs<TValue> : EventArgs
    {
        public int NewIndex;
        public int OldIndex;
        public TValue Value;

        public ReactiveListSortingArgs()
        {
        }

        public ReactiveListSortingArgs(int oldIndex, int newIndex, TValue value)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
            Value = value;
        }
    }
}