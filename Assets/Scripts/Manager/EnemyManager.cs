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
	// 現在生成されているアイテム
	List<Enemy> enemyList = new List<Enemy>();
	private int id = 0;
	private float createTime = 2.0f;
	private bool isCreating = false;

	private void Update()
	{
		if(PhotonNetwork.IsMasterClient)
		{
			// 一定間隔でエネミーを補充
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
		// アイテムを探す
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
		// 敵が湧く範囲内からランダムで位置を決定
		Vector3 pos = Vector3.zero;
		Bounds bounds = spawnArea.bounds;
		pos = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z));
		// 乱数で移動距離と移動方向を決定
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
		// IDをセット
		enemy.ID = id++;
		// 移動距離と方向をセット
		enemy.Speed = speed;
		enemy.MaxDistance = maxDistance;
		enemy.MoveVec = moveVec;
		enemy.EnemyManager= this;
		// リストに追加
		enemyList.Add(enemy);
	}
	public void Remove(Enemy enemy)
	{
		// スコアを加算
		int score = PlayerProperties.GetScore(PhotonNetwork.LocalPlayer);
		score += data.EnemyScore;
		PlayerProperties.SetScore(PhotonNetwork.LocalPlayer, score);
		// 敵を破壊
		photonView.RPC(nameof(DestroyEnemy), RpcTarget.All, enemy.ID);
	}
	[PunRPC]

	public void DestroyEnemy(int id)
	{
		// 同じIDの敵を探す
		Enemy enemy = FindEnemy(id);
		// 敵死亡時のエフェクトを作成
		enemy.PlayDeadEffect(enemy.transform.position);
		// リストからも削除
		enemyList.Remove(enemy);
		// その弾を破棄
		Destroy(enemy.gameObject);

	}
}
