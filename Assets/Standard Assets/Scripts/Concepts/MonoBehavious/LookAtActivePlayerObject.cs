using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using GameDevJourney;

public class LookAtActivePlayerObject : UpdateWhileEnabled, IUpdatable
{
	public Transform trs;
	
	public override void DoUpdate ()
	{
		trs.forward = (PlayerShip.CurrentActive.trs.position - trs.position).GetXZ();
	}
}
