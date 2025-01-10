using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : IState
{

    public Vector3 KnockbackDir { get; set; }
	public bool isKnockbacking {  get; set; }

    public Knockback(PlayerMove playerMove) : base(playerMove) 
	{
		state = PLAYER_STATE.KNOCK_BACK;
	}
	public override void Enter()
	{
		// ���x���Œᑬ�x��
		player.Speed = player.Data.MinSpeed;
		// ��x��]�����x��0�ɂ���
		rigidbody.angularVelocity = Vector3.zero;
		rigidbody.linearVelocity = Vector3.zero;
		// ��e���̃J�����V�F�C�N���J�n
		var shaker = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineImpulseSource>();
		shaker.GenerateImpulse();
		player.photonView.RPC(nameof(player.Effect.EndBoost), RpcTarget.All);
		// �m�b�N�o�b�N������
		if (!isKnockbacking)
		{
			rigidbody.AddForce(KnockbackDir * player.Data.KnockbackPower, ForceMode.Impulse);
		}
		else
		{
			// �m�b�N�o�b�N���Ȃ�m�b�N�o�b�N�ł͂Ȃ������x������������
			rigidbody.linearVelocity = KnockbackDir.normalized;
		}
	}

	public override void Exit()
	{
		Debug.Log("�m�b�N�o�b�N�I��");
		// ���x���Œᑬ�x��
		player.Speed = player.Data.MinSpeed;
	}

	public override void FixedUpdate()
	{
		rigidbody.AddForce(player.transform.forward * player.Data.ReturnSpeed, ForceMode.Force);
		// ���@���O�ɐi��ł���̂��`�F�b�N
		float dot = Vector3.Dot(player.transform.forward, rigidbody.linearVelocity.normalized);
		if (dot > 0.0f)
		{
			player.photonView.RPC(nameof(player.ChangeState), RpcTarget.All, PLAYER_STATE.IDLE);
		}
	}

	public override void Update()
	{
	}
}
