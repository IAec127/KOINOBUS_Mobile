using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private GameObject enemyPrefab = null;
	[SerializeField]
	private EnemyData data = null;
	[SerializeField]
	private BoxCollider spawnArea = null;
	[SerializeField]
	private GameObject parent = null;
	// ���ݐ�������Ă���A�C�e��
	List<Enemy> enemyList = new List<Enemy>();
	private int id = 0;
	private float createTime = 2.0f;
	private bool isCreating = false;

	private void Update()
	{
		if(PhotonNetwork.IsMasterClient)
		{
			// ���Ԋu�ŃG�l�~�[���[
			if (enemyList.Count < data.MaxEnemy && !isCreating)
			{
				StartCoroutine(SpawnEnemy());
			}
		}
	}

	private IEnumerator SpawnEnemy()
	{
		isCreating = true;
		yield return new WaitForSeconds(createTime);
		Create();
		isCreating = false;
		yield break;
	}

	public List<Enemy> GetEnemyList()
	{
		return enemyList;
	}

	public Enemy FindEnemy(int id)
	{
		// �A�C�e����T��
		Enemy findEnemy = null;
		foreach (var enemy in enemyList)
		{
			if (enemy.ID == id)
			{
				findEnemy = enemy;
			}
		}
		return findEnemy;
	}

	public void SpawnMaxEnemy()
	{
		for (int i = 0; i < data.MaxEnemy; i++)
		{
			Create();
		}
	}

	public void Create()
	{
		// �G���N���͈͓����烉���_���ňʒu������
		Vector3 pos = Vector3.zero;
		Bounds bounds = spawnArea.bounds;
		pos = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z));
		// �����ňړ������ƈړ�����������
		float speed = 0.0f;
		float maxDistance = 0.0f;
		Vector3 moveVec = Vector3.zero;
		speed = Random.Range(data.MinSpeed, data.MaxSpeed);
		maxDistance=Random.Range(data.MinDistance, data.MaxDistance);
		moveVec = new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized;

		photonView.RPC(nameof(CreateEnemy), RpcTarget.All, pos, speed,maxDistance, moveVec);
	}

	[PunRPC]
	public void CreateEnemy(Vector3 pos, float speed,float maxDistance, Vector3 moveVec)
	{
		GameObject enemyObj = Instantiate(enemyPrefab, pos, Quaternion.identity, parent.transform) ;
		Enemy enemy = enemyObj.GetComponent<Enemy>();
		// ID���Z�b�g
		enemy.ID = id++;
		// �ړ������ƕ������Z�b�g
		enemy.Speed = speed;
		enemy.MaxDistance = maxDistance;
		enemy.MoveVec = moveVec;
		enemy.EnemyManager= this;
		// ���X�g�ɒǉ�
		enemyList.Add(enemy);
	}
	public void Remove(Enemy enemy)
	{
		// �X�R�A�����Z
		int score = PlayerProperties.GetScore(PhotonNetwork.LocalPlayer);
		score += data.EnemyScore;
		PlayerProperties.SetScore(PhotonNetwork.LocalPlayer, score);
		// �G��j��
		photonView.RPC(nameof(DestroyEnemy), RpcTarget.All, enemy.ID);
	}
	[PunRPC]

	public void DestroyEnemy(int id)
	{
		// ����ID�̓G��T��
		Enemy enemy = FindEnemy(id);
		// �G���S���̃G�t�F�N�g���쐬
		enemy.PlayDeadEffect(enemy.transform.position);
		// ���X�g������폜
		enemyList.Remove(enemy);
		// ���̒e��j��
		Destroy(enemy.gameObject);

	}
}
