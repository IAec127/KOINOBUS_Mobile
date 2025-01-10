using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class Stun : IState
{

	public Stun(PlayerMove playerMove) : base(playerMove)
	{
		state = PLAYER_STATE.STUN;
	}
	public override void Enter()
	{
		// ���x��0�ɂ���
		player.Speed = 0;
		rigidbody.linearVelocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		// ��e���̃J�����V�F�C�N���J�n
		var shaker = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineImpulseSource>();
		shaker.GenerateImpulse();
		player.photonView.RPC(nameof(player.Effect.EndBoost), RpcTarget.All);
		// �G�t�F�N�g���N��
		player.photonView.RPC(nameof(player.Effect.StartStun), RpcTarget.All);
		// ��]�R���[�`�����J�n
		player.photonView.RPC(nameof(player.StartRotate), RpcTarget.All);
	}

	public override void Exit()
	{
		// ���x���Œᑬ�x��
		player.Speed = player.Data.MinSpeed;
		// �G�t�F�N�g���~
		player.photonView.RPC(nameof(player.Effect.EndStun), RpcTarget.All);
	}

	public override void FixedUpdate()
	{
	}

	public override void Update()
	{
	}

}
