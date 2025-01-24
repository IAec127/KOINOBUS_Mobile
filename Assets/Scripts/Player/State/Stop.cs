using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Stop : IState
{
	private CinemachineVirtualCamera vCamera = null;
	private CinemachineComposer vCameraCompoer;
    private float countTime = 0.0f;//時間をはかる
    private float timeLimit = 4.0f;//制限時間

    public Stop(PlayerMove playerMove) : base(playerMove) 
	{
		vCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
		vCameraCompoer = vCamera.GetCinemachineComponent<CinemachineComposer>();
		state = PLAYER_STATE.STOP;
	}
	public override void Enter()
	{
	}

	public override void Exit()
	{
        player.Speed = (player.Data.MinSpeed + player.Data.MaxSpeed) * 0.5f;
    }

    public override void Update()
	{
		
	}


	public override void FixedUpdate()
	{
		player.Speed = 0.0f;
        countTime += Time.deltaTime;//マイフレーム事にかかった時間を足している


        if (countTime > timeLimit)
        {
            player.ChangeState(PLAYER_STATE.IDLE);
        }
    }

}
