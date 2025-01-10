using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PLAYER_STATE
{
	IDLE,
	KNOCK_BACK,
	STUN,
	DEAD,
}

public abstract class IState
{
	protected PlayerMove player = null;
	protected Rigidbody rigidbody = null;

	protected PLAYER_STATE state = PLAYER_STATE.IDLE;
	public PLAYER_STATE State
	{
		get { return state; }
		set { state = value; }
	}


	public IState(PlayerMove player)
	{
		this.player = player;
		rigidbody = player.GetComponent<Rigidbody>();
	}
	public abstract void Enter();

	public abstract void Update();

	public abstract void FixedUpdate();

	public abstract void Exit();
}

