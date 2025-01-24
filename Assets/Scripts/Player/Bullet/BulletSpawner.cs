using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BulletSpawner : MonoBehaviourPunCallbacks
{
    public enum TARGET_TYPE
    {
        ENEMY,
        PLAYER,
        NONE,
    }

    [SerializeField]
    BulletData data = null;
    [SerializeField]
    ParticleSystem shotEffect = null;
    private BulletManager bulletManager;
    private EnemyManager enemyManager;
    private LockOnUI lockOnUI;
    private PlayerMove player;
    public GameObject bulletPrefab; // 通常の弾のプレハブ
    public GameObject chargeBulletPrefab;
    public Transform firePoint;
    public float chargeTime = 0f; // チャージ時間
    [SerializeField]
    private int currentShots = 0; // 現在の弾数
    public int CurrentShots
    {
        get { return currentShots; }
        set { currentShots = value; }
    }
	float maxAngle = 10.0f;
    [SerializeField]
    TARGET_TYPE targetType;
    int targetID = -1;
    float maxLockOnTime = 4.0f;
    float lockonTime = 0.0f;

	private bool isReloading = false; // リロード中かどうか
    private bool isChargeComplete = false; // チャージ完了フラグ
    [SerializeField]
    private bool isLockedOn = false; // ロックオンしたかどうか

    private void Start()
	{
        bulletManager = GameObject.FindGameObjectWithTag("BulletManager").GetComponent<BulletManager>();
		enemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
        lockOnUI = GameObject.FindGameObjectWithTag("LockOnUI").GetComponent<LockOnUI>();
		player = GetComponent<PlayerMove>();
        currentShots = data.MaxBullet;
	}
	public void BulletUpdate()
    {
  //      //if(photonView.IsMine)
  //      {
  //          if (currentShots > 0)
  //          {
  //              LockOn();
  //          }

  //          // チャージ処理
  //          HandleCharge();

  //          // 発射処理
  //          if (player.IsShot)
		//	{
		//		Fire();
		//	}

  //          // リロード条件を確認
  //          if (currentShots < data.MaxBullet && isReloading == false)
		//	{
		//		StartCoroutine(Reload());
		//	}
		//}
	}

    void HandleCharge()
    {
     //   if (player.IsCharging)
     //   {
     //       chargeTime = Mathf.Min(chargeTime + Time.deltaTime, data.MaxChargeTime+ (player.GetItems[(int)Item.ITEM_EFFECT.CHARGE - 1] * player.Data.Item_charge));

     //       // チャージ時間分押し続けるか狂風イベント中ならチャージ完了
     //       if ((chargeTime >= data.MaxChargeTime + (player.GetItems[(int)Item.ITEM_EFFECT.CHARGE - 1] * player.Data.Item_charge) ||
					//player.EventType == EventManager.EVENT_TYPE.CRAZY_WIND) && !isChargeComplete)
     //       {
     //           Debug.Log("チャージ完了！");
     //           isChargeComplete = true;
     //       }
     //   }
    }

    void ResetCharge()
    {
        chargeTime = 0f;
        isChargeComplete = false;
    }

    private void LockOn()
    {
		//TARGET_TYPE beforeType = targetType;
		//int beforeID = targetID;
		//// ターゲットを探す
		//var targetTrans = FindClosestEnemy();
  //      // ターゲットを初めてロックオンしたとき
  //      if (beforeType == TARGET_TYPE.NONE && targetType != TARGET_TYPE.NONE)
  //      {
  //          // ロックオンUIを表示
  //          lockOnUI.ActiveUI();
  //          lockOnUI.Target = targetTrans;
  //      }
  //      // 同じターゲットをロックオンし続けている時
  //      else if (beforeType == targetType && beforeID == targetID && targetType != TARGET_TYPE.NONE)
  //      {
  //          // ロックオン時間を更新
  //          lockonTime += Time.deltaTime;
  //          if (lockonTime > maxLockOnTime)
  //          {
  //              Debug.Log("ロックオンしました");
  //              lockOnUI.LockedOn();
  //              lockonTime = 0.0f;
  //              isLockedOn = true;
  //          }
  //      }
  //      // ロックオンしているターゲットがいなくなった時
  //      else if (beforeType != TARGET_TYPE.NONE && targetType == TARGET_TYPE.NONE)
  //      {
  //          // ロックオンUIを非表示
  //          lockOnUI.InActiveUI();
  //          lockOnUI.LockOut();
  //          // ロックオン時間を初期化
  //          lockonTime = 0.0f;
  //          isLockedOn = false;
  //      }
  //      // 違うターゲットをロックオンした時
  //      else if (targetType != TARGET_TYPE.NONE && (targetType != beforeType || targetID != beforeID))
		//{
		//	lockOnUI.Target = targetTrans;
		//	lockOnUI.LockOut();
		//	// ロックオン時間を初期化
		//	lockonTime = 0.0f;
		//	isLockedOn = false;
  //      }
  //      else
  //      {
  //      }

	}

	void Fire()
    {
  //      // リロード中のチェックは不要になった
  //      int shotsRequired = isChargeComplete ? 3 : 1;

  //      if (currentShots - shotsRequired < 0)
  //      {
  //          Debug.Log("弾数が足りません！");
  //          ResetCharge();
  //          return;
  //      }

  //      shotEffect.Play();
  //      if(isLockedOn)
  //      {
  //          bulletManager.Fire(photonView.OwnerActorNr, firePoint.transform, isChargeComplete, targetType, targetID);
  //      }
  //      else
  //      {
		//	bulletManager.Fire(photonView.OwnerActorNr, firePoint.transform, isChargeComplete, TARGET_TYPE.NONE, -1);
		//}

		//// 狂風イベント中なら弾が減らない
		//if (player.EventType!=EventManager.EVENT_TYPE.CRAZY_WIND)
  //      {
  //          currentShots -= shotsRequired;
  //      }
  //      if(isLockedOn)
  //      {
		//	lockOnUI.LockOut();
		//	// ロックオン時間を初期化
		//	lockonTime = 0.0f;
		//	isLockedOn = false;
		//    ResetCharge();
  //      }
    }

	private Transform FindClosestEnemy()
	{
        // ターゲットの候補となる敵とプレイヤーを取得
		List<Enemy> enemies = enemyManager.GetEnemyList();
        List<PlayerMove> players = SceneController.instance.Players;
        List<Enemy> targetEnemyList = new List<Enemy>();
		List<PlayerMove> targetPlayerList = new List<PlayerMove>();

        Transform targetTrans = null;

		// プレイヤーの方向
		Vector3 playerForward = player.transform.forward;

		// 画面の中央付近にいる敵のみを入手
		for (int i = 0; i < enemies.Count; i++)
		{
			// オブジェクトへの方向
			Vector3 toObject = (enemies[i].transform.position - player.transform.position).normalized;

			// カメラの方向とオブジェクトの方向の角度を計算
			float angle = Vector3.Angle(playerForward, toObject);
			if (angle <= maxAngle)
			{
				targetEnemyList.Add(enemies[i]);
			}
		}

		// 画面の中央付近にいるプレイヤーのみを入手
		for (int i = 0; i < players.Count; i++)
		{
			if (players[i].photonView.ControllerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                continue;
            }
            // プレイヤーがすでに被弾しているか無敵状態ならターゲットリストに入れない
            if (player.NowState.State == PLAYER_STATE.STUN || player.NowState.State == PLAYER_STATE.DEAD)
            {
                continue;
            }
            if (player.IsInvincible)
            {
                continue;
            }
			// オブジェクトへの方向
			Vector3 toObject = (players[i].transform.position - player.transform.position).normalized;

			// カメラの方向とオブジェクトの方向の角度を計算
			float angle = Vector3.Angle(playerForward, toObject);
			if (angle <= maxAngle)
			{
				targetPlayerList.Add(players[i]);
			}
		}

		float closestDistance = Mathf.Infinity;

        targetID = -1;
        targetType = TARGET_TYPE.NONE;
        // ターゲットリストの中から一番近いプレイヤーをターゲットに
		foreach (PlayerMove target in targetPlayerList)
		{
			Vector3 directionToPlayer = (target.transform.position - transform.position).normalized;
			if (Vector3.Dot(transform.forward, directionToPlayer) > 0)
			{
				float distance = Vector3.Distance(transform.position, target.transform.position);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					targetType = TARGET_TYPE.PLAYER;
					targetID = target.photonView.ControllerActorNr;
					targetTrans = target.transform;
				}
			}
		}
		// ターゲットリストの中から一番近い敵をターゲットに
		foreach (Enemy target in targetEnemyList)
		{
			Vector3 directionToEnemy = (target.transform.position - transform.position).normalized;
			if (Vector3.Dot(transform.forward, directionToEnemy) > 0)
			{
				float distance = Vector3.Distance(transform.position, target.transform.position);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					targetType = TARGET_TYPE.ENEMY;
                    targetID = target.ID;
                    targetTrans = target.transform;
				}
			}
		}
        return targetTrans;
	}

    public Transform FindTarget(TARGET_TYPE type,int  targetID)
    {
        Transform target = null;

        if (type == TARGET_TYPE.PLAYER)
        {
            var player = SceneController.instance.FindPlayer(targetID);
            if(player != null)
            {
			    target = player.transform;
            }

        }
        else
        {
            var enemy = enemyManager.FindEnemy(targetID);
            if (enemy != null)
            {
                target = enemy.transform;
                
            }

        }

        if(target==null)
        {
            Debug.Log("弾のターゲットが見つかりませんでした");
        }
        return target;
    }

	IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(data.ReloadTime);
        currentShots++;
        if(currentShots>data.MaxBullet)
        {
            currentShots = data.MaxBullet;
        }
        isReloading = false;
    }
}