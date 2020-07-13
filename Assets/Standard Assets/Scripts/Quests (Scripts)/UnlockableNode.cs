using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableNode : _Node
{
	public int unlockValue;
	// [HideInInspector]
	public int currentValue;
	[SaveAndLoadValue]
	public int CurrentValue
	{
		get
		{
			return currentValue;
		}
		set
		{
			currentValue = value;
			if (currentValue >= unlockValue)
				Unlock ();
		}
	}
	
	public virtual void Unlock ()
	{
	}
	
	public override void Traverse ()
	{
		base.Traverse();
		foreach (_Connection connection in connections)
		{
			UnlockableNode endNode = connection.end as UnlockableNode;
			if (endNode != null)
				endNode.CurrentValue += connection.weight;
		}
	}
}
