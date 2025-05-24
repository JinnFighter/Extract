using System;
using System.Collections.Generic;

public class NotifierListRemoveRangeArgs< TValue > : EventArgs
{
	#region Public Fields
	public int Index;
	public int Count;
	public IEnumerable< TValue > removedValues;
	#endregion

	#region Constructors
	public NotifierListRemoveRangeArgs()
	{
		removedValues = new List< TValue >();
	}

	public NotifierListRemoveRangeArgs( int index, int count, IEnumerable< TValue > removed )
	{
		Index = index;
		Count = count;
		removedValues = removed;
	}
	#endregion
}
