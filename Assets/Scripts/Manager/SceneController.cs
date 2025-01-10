using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviourPunCallbacks
{
	public static SceneController instance; // �C���X�^���X�̒�`
	public CountDownUI CountDownUI { get; set; } = null;
    public List<PlayerMove> Players { get; set; } = new List<PlayerMove>();
	public bool IsStart { get; set; } = false;

    [SerializeField]
	private int createEnemyPlayerNum = 2;

	private void Awake()
	{
		// �V���O���g���̎���
		if (instance == null)
		{
			// ���g���C���X�^���X�Ƃ���
			instance = this;
		}
		else
		{
			// �C���X�^���X���������݂��Ȃ��悤�ɁA���ɑ��݂��Ă����玩�g����������
			Destroy(gameObject);
		}
	}

	private void Update()
	{
	}

	private IEnumerator Init()
	{
		GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>().SpawnMaxEnemy();
		GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>().SpawnMaxItem();
		yield return new WaitForSeconds(1.0f);
		photonView.RPC(nameof(CountDown), RpcTarget.AllViaServer);
		yield break;
	}

	public void StartGame()
	{
		IsStart = true;
		// ���[�����쐬�����v���C���[�́A���݂̃T�[�o�[�������Q�[���̊J�n�����ɐݒ肷��
		//if (PhotonNetwork.IsMasterClient)
		//{
		//	PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
		//}

	}

	[PunRPC]
	private void CountDown()
	{
		if(CountDownUI != null)
		{
			StartCoroutine(CountDownUI.CountDown());
		}
	}

	public void AddPlayer(PlayerMove playerMove)
	{
		Players.Add(playerMove);
		if (!IsStart && PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == createEnemyPlayerNum)
		{
			StartCoroutine(Init());
		}
	}

	public PlayerMove FindPlayer(int findId)
	{
		PlayerMove findPlayer = null;
		//foreach (PlayerMove playerMove in Players)
		//{
		//	int id = playerMove.gameObject.GetPhotonView().CreatorActorNr;
		//	if (findId == id)
		//	{
		//		findPlayer = playerMove;
		//	}
		//}

		return findPlayer;
	}
}
