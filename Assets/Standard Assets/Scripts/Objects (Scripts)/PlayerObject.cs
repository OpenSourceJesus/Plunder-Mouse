using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlunderMouse;
using Extensions;
using TMPro;
using UnityEngine.InputSystem;
using System;
using Unity.XR.Oculus.Input;

public class PlayerObject : SingletonMonoBehaviour<PlayerObject>, IDestructable, IUpdatable
{
	public Transform trs;
	public Rigidbody rigid;
	public float moveSpeed;
	[HideInInspector]
	public float hp;
	public float Hp
	{
		get
		{
			return hp;
		}
		set
		{
			hp = value;
		}
	}
	public int maxHp;
	public int MaxHp
	{
		get
		{
			return maxHp;
		}
		set
		{
			maxHp = value;
		}
	}
	public TMP_Text hpText;
	[HideInInspector]
	public bool dead;
	[HideInInspector]
	public Vector3 move;
	public bool active;
	public bool Active
	{
		get
		{
			return active;
		}
		set
		{
			active = value;
			if (active)
				currentActive = this;
		}
	}
	static PlayerObject currentActive;
	public static PlayerObject CurrentActive
	{
		get
		{
			return currentActive;
		}
		set
		{
			if (currentActive != null)
				currentActive.active = false;
			currentActive = value;
			currentActive.active = true;
		}
	}
	public float attackRate;
	[HideInInspector]
	public float attackTimer;
	public ColliderPresenceDetector switchIndicatorTrigger;
	public GameObject switchIndicator;
	[HideInInspector]
	public bool canSwitch;
	public Weapon weapon;
	
	public virtual void Start ()
	{
		Hp = MaxHp;
		if (active)
			CurrentActive = this;
		GameManager.updatables = GameManager.updatables.Add(this);
	}
	
	public virtual void Death ()
	{
		LoseableScenerio.Instance.Lose ();
	}
	
	public virtual void DoUpdate ()
	{
		if (dead || !active)
			return;
		Move ();
		Rotate ();
	}

	public virtual void TakeDamage (float amount, Hazard source)
	{
		hp = Mathf.Clamp(hp - amount, 0, MaxHp);
		hpText.text = "" + hp;
		if (hp == 0)
		{
			dead = true;
			Death ();
		}
	}
	
	public virtual Vector3 GetMoveInput ()
	{
		Vector3 moveInput = InputManager.MoveInput;
		moveInput = moveInput.XYToXZ();
		moveInput = Quaternion.Euler(Vector3.up * OVRCameraRig.Instance.eyesTrs.eulerAngles.y) * moveInput;
		moveInput.y = 0;
		return moveInput;
	}
	
	public virtual void Move ()
	{
		move = GetMoveInput();
		rigid.velocity += move.normalized * moveSpeed * Time.deltaTime;
	}

	public virtual void OnDestroy ()
	{
		GameManager.updatables = GameManager.updatables.Remove(this);
	}
	
	public virtual void Rotate ()
	{
		if (rigid.velocity != Vector3.zero)
			trs.forward = rigid.velocity;
	}
	
	[Serializable]
	public class AttackEntry
	{
		public float reloadRate;
		[HideInInspector]
		public float reloadTimer;
	}
}
