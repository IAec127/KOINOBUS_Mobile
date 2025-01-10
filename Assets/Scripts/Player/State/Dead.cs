using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : IState
{
	private float nowTime = 0.0f;
	private Vector3 startRot = Vector3.zero;
	private float rotationY = 0.0f;
	private BoxCollider respawnArea = null;
	private float respawnRadius = 40.0f;

	public Dead(PlayerMove playerMove) : base(playerMove)
	{
		state = PLAYER_STATE.DEAD;
		respawnArea = GameObject.FindGameObjectWithTag("RespawnArea").GetComponent<BoxCollider>();
	}
	public override void Enter()
	{
		// ���x�A�p���x��0�ɂ���
		player.Speed = 0;
		rigidbody.linearVelocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		// ��e���̃J�����V�F�C�N���J�n
		var shaker = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineImpulseSource>();
		shaker.GenerateImpulse();
		player.photonView.RPC(nameof(player.Effect.EndBoost), RpcTarget.All);
		// ���G��Ԃ�
		player.StartCoroutine(Init());
	}

	public override void Exit()
	{

	}

	public override void FixedUpdate()
	{
	}

	public override void Update()
	{
	}

	private void Respawn()
	{
		Vector3 offset = Vector3.zero;
		Bounds bounds = respawnArea.bounds;
		offset = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z));
		// ���̋@�̂Ƃ̋���������
		player.transform.position = offset;
		float y = Random.Range(0.0f, 360.0f);
		player.transform.eulerAngles = new Vector3(0.0f, y, 0.0f);
	}

	private void DropItems()
	{
		// �A�C�e���̍��v���v�Z
		int totalItems = 0;
		List<Item.ITEM_EFFECT> items = new List<Item.ITEM_EFFECT>();
		for (int i = 0; i < player.GetItems.Length; i++)
		{ 
			for(int j=0;j< player.GetItems[i]; j++)
			{
				items.Add((Item.ITEM_EFFECT)i + 1);
			}
			totalItems += player.GetItems[i];
		}
		// ���Ƃ��A�C�e�������v�Z
		int dropItems = (int)(totalItems * 0.2f);
		// ����ɃA�C�e���𐶐�
		for (int i = 0; i < dropItems; i++)
		{
			// ��������A�C�e���̎�ނ�����
			int rand = Random.Range(0, items.Count);
			Item.ITEM_EFFECT effect = items[rand];
			items.RemoveAt(rand);
			// �����ʒu������
			Vector3 randPos = GetRandomCirclePosition();
			randPos += player.transform.position;
			// �A�C�e���𐶐�
			player.ItemManager.Create(effect, Item.ITEM_TYPE.PLUS, randPos);
		}
	}

	private Vector3 GetRandomCirclePosition()
	{
		// �����_���Ȋp�x�𐶐��i���W�A���j
		float randomAngle = Random.Range(0f, Mathf.PI * 2);

		// �~����̍��W���v�Z
		float x = Mathf.Cos(randomAngle) * respawnRadius;
		float z = Mathf.Sin(randomAngle) * respawnRadius;

		// �v���C���[�̈ʒu����ɃI�t�Z�b�g
		Vector3 randomPosition = new Vector3(x, 0f, z);

		return randomPosition;
	}

	IEnumerator Init()
	{
		player.photonView.RPC(nameof(player.Effect.StartDead), RpcTarget.All);
		// �����Ă���A�C�e����2���𗎂Ƃ�
		DropItems();
		// �v���C���[���\��
		player.photonView.RPC(nameof(player.InActivePlayerModel), RpcTarget.All);
		// �����ҋ@
		yield return new WaitForSeconds(2.0f);
		// ���X�|�[��
		Respawn();
		// �v���C���[��\��
		player.photonView.RPC(nameof(player.ActivePlayerModel), RpcTarget.All);
		// �Œᑬ�x�ɂ���
		player.Speed = player.Data.MinSpeed;
		// �̗͂��ő��
		int hp = PlayerProperties.GetHp(player.photonView.Controller);
		hp = player.Data.MaxHp;
		PlayerProperties.SetHp(player.photonView.Controller, hp);

		// ���G��Ԃ�
		player.photonView.RPC(nameof(player.StartInvincible), RpcTarget.All);
		player.photonView.RPC(nameof(player.ChangeState), RpcTarget.All, PLAYER_STATE.IDLE);
	}


}
