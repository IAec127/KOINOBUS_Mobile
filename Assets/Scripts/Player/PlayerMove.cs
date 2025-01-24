using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Collections.Generic;
using Photon.Realtime;
using System.Collections;
using Unity.VisualScripting;
using System.Globalization;


//MonoBehaviour->MonoBehaviourPunCallbacksに変更
public class PlayerMove : MonoBehaviourPunCallbacks, IPunObservable
{
	// プレイヤーの入力スクリプトの配列
	[SerializeField]
	private PlayerInput[] inputs = null;	

	public PlayerInput[] Inputs
	{
		get { return inputs; }
		set { inputs = value; }
	}

	[SerializeField]
	private GameObject koiModel = null;

	[SerializeField]
	private SkinnedMeshRenderer playerRenderer = null;

	public SkinnedMeshRenderer PlayerRenderer
	{
		get { return playerRenderer; }
		set { playerRenderer = value; }
	}

	[SerializeField]
	private BulletSpawner bulletSpawner = null;
	public BulletSpawner BulletSpawner
	{
		get { return bulletSpawner; }
		set { bulletSpawner = value; }
	}


	[SerializeField]
	private List<Material> materials = null;


	// プレイヤーのデータ
	[SerializeField]
	private PlayerData data = null;		
	public PlayerData Data
	{
		get { return data; }
		set { data = value; }
	}

	[SerializeField]
	private PlayerEffect effect;
	public PlayerEffect Effect
	{
		get { return effect; }
		set { effect = value; }
	}


	// プレイヤーのモデルデータ
	[SerializeField]
	private GameObject modelObj = null;
	public GameObject ModelObj
	{
		get { return modelObj; }
		set { modelObj = value; }
	}

	// カメラのデータ
	[SerializeField]
	private CameraData cameraData = null;
	public CameraData CameraData
	{
		get { return cameraData; }
		set { cameraData = value; }
	}

	// プレイヤーの状態
	private IState nowState = null;

	public IState NowState
	{
		get { return nowState; }
		private set { nowState = value; }
	}

    // --------------------------
    // 各種管理用クラス
    // --------------------------
    public ItemManager ItemManager { get; set; }
    public BulletManager BulletManager { get; set; }
    public EnemyManager EnemyManager { get; set; }

    // --------------------------
    // UI関連クラス
    // --------------------------
    private ScoreUI scoreUI = null;
	private HpUI hpUI = null;


    // --------------------------
    // 各状態の実体
    //---------------------------
    // 各状態の実体のリスト
    List<IState> stateList = new List<IState>();
	[SerializeField]
	private PLAYER_STATE s;

    // 通常状態
    private Idle idleState;
	public Idle IdleState
	{
		get { return idleState; }
		private set { idleState = value; }
	}
	// ノックバック状態
	private Knockback knockbackState;
	public Knockback KnockbackState
	{
		get { return knockbackState; }
		private set { knockbackState = value; }
	}
	// 気絶（被弾）状態
	private Stun stunState;
	public Stun StanState
	{
		get { return stunState; }
		private set { stunState = value; }
	}
	// 死亡状態
	private Dead deadState;
	public Dead DeadState
	{
		get { return deadState; }
		set { deadState = value; }
	}
	//初期状態
	private Stop stopState;
	public Stop StopState
	{
		get { return stopState; }
		set { stopState = value; }
	}

	//------------------------
	// 基本ステータス
	//------------------------

	// 現在のスピード
	[SerializeField]
	private float speed;
	public float Speed
	{
		get { return speed; }
		set { speed = value; }
	}

	// 最高速度(アイテムの効果,イベントの影響を含めた最終的な最高速度)
	public float MaxSpeed
	{
		get 
		{
			return data.MaxSpeed + (getItems[(int)Item.ITEM_EFFECT.MAX_SPEED - 1] * data.Item_maxSpeed) + EventSpeed; 
		}
	}


	// アクセルを踏んでいるかのフラグ
	private bool isAccel;
	public bool IsAccel
	{
		get { return isAccel; }
		set { isAccel = value; }
	}

	// ブレーキを踏んでいるかのフラグ
	private bool isBrake;
	public bool IsBrake
	{
		get { return isBrake; }
		set { isBrake = value; }
	}

	// このフレームで回転しているかを表すフラグ
	[SerializeField]
	private bool isRotate = false;
	public bool IsRotate
	{
		get { return isRotate; }
		set { isRotate = value; }
	}

	// このフレームでブースト（アクセル）しているかを表すフラグ
	[SerializeField]
	private bool isBoost = false;
	public bool IsBoost
	{
		get { return isBoost; }
		set { isBoost = value; }
	}

	public bool IsShot { get; set; } = false;
	public bool IsCharging { get; set; } = false;


    // 無敵かどうか
    [SerializeField]
	private bool isInvincible = false;
	public bool IsInvincible
	{
		get { return isInvincible; }
		set { isInvincible = value; }
	}


	// 接地判定をする4方向のray
	private GroundingRay[] rays= new GroundingRay[4];
	public GroundingRay[] Rays
	{
		get { return rays; }
		private set { rays = value; }
	}

	[SerializeField]
	private bool isFollowWall;
	public bool IsFollowWall
	{
		get { return isFollowWall; }
		set { isFollowWall = value; }
	}

	//---------------------
	// ブースト系
	//---------------------
	[SerializeField]
	private float totalBoostPower = 0.0f;

	public float TotalBoostPower
	{
		get { return totalBoostPower; }
		set { totalBoostPower = value; }
	}



	//---------------------
	// アイテム系
	//---------------------

	// アイテムの種類ごとの所持している数
	private int[] getItems = new int[Item.ITEM_COUNT];
	public int[] GetItems
	{
		get { return  getItems; }
		set {  getItems = value; }
	}

	//-----------------------
	// イベント系
	//-----------------------
	// 現在のイベントの種類
	[SerializeField]
	private EventManager.EVENT_TYPE eventType = EventManager.EVENT_TYPE.NONE;
	public EventManager.EVENT_TYPE EventType
	{
		get { return eventType; }
		set 
		{ 
			var beforType = eventType;
			eventType = value;
			// イベントが終了したならエフェクトを止める
			if (beforType != EventManager.EVENT_TYPE.NONE)
			{
			 	effect.BuffEffects[(int)beforType].Stop();
				effect.BuffEffects[(int)beforType].gameObject.SetActive(false);
			}
			if (eventType != EventManager.EVENT_TYPE.NONE)
			{
				effect.BuffEffects[(int)eventType].gameObject.SetActive(true);
				effect.BuffEffects[(int)eventType].Play();
			}
		}
	}

	// スピードに影響があるイベントの速度
    public float EventSpeed { get; set; }
    // 気流系イベントの気流の方向
    public Vector3 burstVec { get; set; } = Vector3.zero;
    // 竜巻イベント時の旋回加算値
    public Vector3 tornadoSens { get; set; }

    //-----------------------------
    // テスト用
    //-----------------------------

    private float invincibleTime = 5.0f;

	//テスト用　仮のパーティクルシステム
	[SerializeField]
    private GameObject particleObject;

    [SerializeField]
    private GameObject UI;

    private PlayerUI uiScript;

    public bool effectFlag;

    //仮HP
    private int life;

    [SerializeField,Header("遷移先のシーン")]
    private string nextScene;

    private void Awake()
	{
		idleState = new Idle(this);
		stateList.Add(idleState);
		knockbackState= new Knockback(this);
		stateList.Add(knockbackState);
		stunState = new Stun(this);
		stateList.Add(stunState);
		deadState = new Dead(this);
		stateList.Add(DeadState);
		stopState = new Stop(this);
		stateList.Add(stopState);

        //if(photonView.IsMine)
        {
			//UIを生成
			if (UI != null)
			{
				GameObject pUI = Instantiate(UI);
				var canvas=pUI.GetComponent<Canvas>();
				pUI.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
				uiScript = pUI.GetComponent<PlayerUI>();
				// レティクルクラスにローカルプレイヤーを渡す
				//var reticleUI = GameObject.FindGameObjectWithTag("Reticle").GetComponent<Reticle>();
				//reticleUI.localPlayer = gameObject;
				//reticleUI.bulletSpawner = gameObject.GetComponent<BulletSpawner>();
				//hpUI = GameObject.FindWithTag("HpUI").GetComponent<HpUI>();
				scoreUI = GameObject.FindWithTag("ScoreUI").GetComponent<ScoreUI>();
			}
			else
			{
				Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
			}

			for (int i = 0; i < rays.Length; i++)
			{
				rays[i] = new GroundingRay(gameObject.transform, (RAY_DIRECTION)i, data.RayOffset, data.RayDistance, data.MinHeight);
			}
		}
		// シーン管理用クラスに自分を登録
		//SceneController.instance.AddPlayer(this);
		nowState = stopState;

	}

	private void Start()
	{
		// 各種管理用クラスを取得
		ItemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
		BulletManager = GameObject.FindGameObjectWithTag("BulletManager").GetComponent<BulletManager>();
		EnemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
		// プレイヤーのマテリアルを変える
		//if (materials.Count > photonView.ControllerActorNr)
		//{
		//	koiModel.GetComponent<SkinnedMeshRenderer>().material = materials[photonView.ControllerActorNr - 1];
		//}
		// スピードを最低スピードで初期化
		//speed = data.MinSpeed;
        //GetComponent<Rigidbody>().velocity = transform.forward * speed;
		// 持っているアイテムの数を初期化
		for(int i=0;i<Item.ITEM_COUNT;i++)
		{
			getItems[i] = 0;
		}

        life = 3;
        effectFlag = false;

        //if (photonView.IsMine)
        {
			// 体力を最大に
			int hp = Data.MaxHp;
			//PlayerProperties.SetHp(photonView.Controller, hp);
			// スコアを初期化
			int score = 0;
			//PlayerProperties.SetScore(photonView.Controller, score);
			// ブースト量を最大に
			totalBoostPower = data.TotalBoost;

			effect.AccelLineEffect = GameObject.FindGameObjectWithTag("AccelLine").GetComponent<ParticleSystem>();
			//GameObject.FindGameObjectWithTag("ToyConUI").GetComponent<ToyConUI>().player = this;
        }

        Application.targetFrameRate = 60;
    }

	// Update is called once per frame
	void Update()
    {
		//if(!SceneController.instance.IsStart)
		//{
		//	return;
		//}
        //自分が生成したオブジェクトのみのアップデートを実行する
        //if(photonView.IsMine)
        {
			if(Input.GetKeyDown(KeyCode.R))
			{
				TakeDamage(1);
			}
			nowState.Update();
			if (!IsBoost)
			{
				// ブースト量を回復
				ChargeBoost();
			}

        }
	}

	private void FixedUpdate()
	{
		//if (!SceneController.instance.IsStart)
		//{
		//	return;
		//}
		//自分が生成したオブジェクトのみのアップデートを実行する
		//if (photonView.IsMine)
        {
            //if(uiScript.timeLost)
            //{
            //    LoadScene();
            //}

            //エフェクトの仮生成処理
            //if (Input.GetMouseButtonDown(1))
            //{
            //    effectFlag = true;
            //}

            //if (effectFlag)
            //{
            //    Vector3 position = transform.position + transform.forward * 100;

            //    Vector3 spwanPos = new Vector3(transform.position.x, transform.position.y, transform.position.z) + transform.forward * 500;
            //    particleObject = PhotonNetwork.Instantiate("TestParticleSystem", spwanPos, transform.rotation);

            //    effectFlag = false;
            //}

            isFollowWall = false;
    		// 4方向分ループ
    		foreach (var ray in rays)
    		{
    			// Rayを作成
    			ray.CreateRay();
    			// Rayを射出
    			ray.ShotRay();
    			// 前後どちらのRayも当たっていたなら
    			if (ray.isGrounding)
    			{
    				isFollowWall = true;
    				// 壁に沿って回転
    				ray.RotateFollowWall();
    				// 壁との最短距離を保つ
    				ray.KeepShortestRange();
    			}
    		}
			Debug.Log(nowState);
    		nowState.FixedUpdate();
        }
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
		if (targetPlayer.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
		{
			int score = (changedProps["s"] is int s) ? s : -1;
			if (score >= 0 && scoreUI != null)
			{
				// UIを更新
				scoreUI.SetScoreUI(score);
			}
			int hp = (changedProps["h"] is int h) ? h : -1;
			if (hp >= 0 && hpUI != null)
			{
				// UIを更新
				hpUI.SetHpUI(hp);
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("HitPlayer"))
		{
			//if(photonView.IsMine)
			{
				// 法線から自機と壁の角度を計算
				Vector3 wallNormal = collision.contacts[0].normal;
				Vector3 dir = transform.forward;    // 自機の向き
				float angle = Vector3.Angle(wallNormal, dir);
				Debug.Log(angle);
				// 自機が正面から衝突したか後ろから衝突したかチェック
				if (angle < 90.0f)
				{
					knockbackState.KnockbackDir = transform.forward;
					knockbackState.isKnockbacking = true;
					//photonView.RPC(nameof(ChangeState), RpcTarget.All, PLAYER_STATE.KNOCK_BACK);
					ChangeState(PLAYER_STATE.KNOCK_BACK);
				}
				if (angle >= 90.0f)
				{
					knockbackState.KnockbackDir = -transform.forward;
					knockbackState.isKnockbacking = false;
                    //photonView.RPC(nameof(ChangeState), RpcTarget.All, PLAYER_STATE.KNOCK_BACK);
                    ChangeState(PLAYER_STATE.KNOCK_BACK);
                }
            }
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		// 弾かどうかチェック
		if (other.gameObject.tag == "Bullet")
		{
			// 自分が操作している機体以外が
			if(!photonView.IsMine)
			{
				var bullet = other.GetComponent<HomingBullet>();
				// ローカルプレイヤーが作った弾かチェック
				if (bullet.OwnerID == photonView.OwnerActorNr)
				{
					return;
				}
				BulletManager.Remove(bullet);
				TakeDamage(1, bullet.OwnerID);
			}
		}
		else if(other.gameObject.tag=="Enemy")
		{
			if(photonView.IsMine)
			{
				var enemy = other.GetComponent<Enemy>();
				EnemyManager.Remove(enemy);
				TakeDamage(1);
			}
		}
	}

	private void ChargeBoost()
	{
		if (totalBoostPower >= data.TotalBoost)
		{
			return;
		}
		totalBoostPower += data.BoostCharge * Time.deltaTime;
		if (totalBoostPower > data.TotalBoost)
		{
			totalBoostPower = data.TotalBoost;
		}
	}

	public void TakeDamage(int damage, int shooterID = -1)
	{
		if (nowState.State == PLAYER_STATE.STUN)
		{
			return;
		}
		Debug.Log(photonView.Controller.ActorNumber + "がダメージを受けます");
		// ダメージを受ける
		int hp = PlayerProperties.GetHp(photonView.Controller);
		hp -= damage;
		if(hp < 0)
		{
			hp = 0;
		}
		PlayerProperties.SetHp(photonView.Controller, hp);

		if (hp > 0)
		{
			// 被弾状態へ
			photonView.RPC(nameof(ChangeState), RpcTarget.All, PLAYER_STATE.STUN);
		}
		// 0以下になったなら撃破される
		else
		{
			if (shooterID >= 0)
			{
				var player = SceneController.instance.FindPlayer(shooterID);
				int score = PlayerProperties.GetScore(player.photonView.Controller);
				score += 300;
				Debug.Log(score);
				PlayerProperties.SetScore(player.photonView.Controller, score);
			}
			// 撃破時の処理
			photonView.RPC(nameof(ChangeState), RpcTarget.All, PLAYER_STATE.DEAD);
		}
	}

	/// <summary>
	/// 取得したアイテムを反映
	/// </summary>
	/// <param name="itemEffect">アイテムの効果</param>
	/// <param name="itemType">アイテムの種類</param>
	public void GetItem(Item.ITEM_EFFECT itemEffect,Item.ITEM_TYPE itemType)
	{
		int n = (int)itemEffect - 1;
		if (itemType==Item.ITEM_TYPE.PLUS)
		{
			getItems[n]++;
			// 最高数を超えたなら最高数に
			if (getItems[n] > Item.ITEM_MAX)
			{
				getItems[n]=Item.ITEM_MAX;
			}
		}
		else
		{
			getItems[n]--;
			// 最高数を超えたなら最高数に
			if (getItems[n] < 0)
			{
				getItems[n] = 0;
			}
		}
	}


	[PunRPC]
	public void ActivePlayerModel()
	{
		ModelObj.SetActive(true);
	}

	[PunRPC]
	public void InActivePlayerModel()
	{
		ModelObj.SetActive(false);
	}

	[PunRPC]
	public void StartRotate()
	{
		StartCoroutine(RotateModel());
	}

	public IEnumerator RotateModel()
	{
		float nowTime = 0.0f;
		// 回転数から1秒間に回転する数値を算出
		float rotationY = (Data.StunRotation * 360.0f) / Data.StunTime;
		// モデルの角度を戻す
		Vector3 rot = gameObject.transform.eulerAngles;
		rot.x = 0.0f; rot.z = 0.0f;
		gameObject.transform.eulerAngles = rot;
		Vector3 startRot = ModelObj.transform.eulerAngles;

		while (true)
		{
			yield return new WaitForFixedUpdate();
			Vector3 rotation = ModelObj.transform.eulerAngles;
			rotation.y += rotationY * Time.fixedDeltaTime;
			ModelObj.transform.eulerAngles = rotation;
			nowTime += Time.fixedDeltaTime;
			// 気絶時間を超えたなら終了
			if (nowTime >= Data.StunTime)
			{
				// モデルの角度を戻す
				rot = gameObject.transform.eulerAngles;
				rot.x = 0.0f; rot.z = 0.0f;
				gameObject.transform.eulerAngles = rot;
				ModelObj.transform.eulerAngles = startRot;
				ChangeState(PLAYER_STATE.IDLE);
				yield break;
			}

		}
	}


	[PunRPC]
	public void StartInvincible()
	{
		StartCoroutine(Invincible());
	}

	IEnumerator Invincible()
	{
		float nowTime = 0.0f;
		IsInvincible = true;
		//var mat = player.ModelObj.GetComponentInChildren<SkinnedMeshRenderer>().material;
		//var color = mat.color;
		float addAlpha = 0.02f;
		while (true)
		{
			yield return new WaitForFixedUpdate();
			nowTime += Time.fixedDeltaTime;
			if (ModelObj.activeSelf)
			{
				ModelObj.SetActive(false);
			}
			else
			{
				ModelObj.SetActive(true);
			}
			//color.a -= addAlpha;
			//if(color.a < 0.0f)
			//{
			//	color.a = 0.0f;
			//	addAlpha *= -1.0f;
			//}
			//if(color.a > 1.0f)
			//{
			//	color.a = 1.0f;
			//	addAlpha *= -1.0f;
			//}
			//mat.color = color;

			if (nowTime >= invincibleTime)
			{
				//color.a = 1.0f;
				//mat.color = color;
				ModelObj.SetActive(true);
				IsInvincible = false;
				yield break;
			}
		}
	}



	/// <summary>
	/// 次の状態へ遷移する関数
	/// </summary>
	/// <param name="nextStateType">次の状態</param>
	[PunRPC]	
	public void ChangeState(PLAYER_STATE nextStateType)
	{
		if (nowState != null)
		{
			//if(photonView.IsMine)
			{
				nowState.Exit();
			}
		}
		IState nextState = stateList[(int)nextStateType];
		s = nextStateType;
		nowState = nextState;
		if (nextState != null )
		{
			//if(photonView.IsMine)
			{
				nextState.Enter();
			}
		}
	}

    private void LoadScene()
    {
        SceneManager.LoadScene(nextScene);   
    }

    /*
     */
    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(life);
            stream.SendNext(effectFlag);
        }
        else
        {
            this.life = (int)stream.ReceiveNext();
            this.effectFlag = (bool)stream.ReceiveNext();
        }
    }

    #endregion

	public bool GameStartCheck()
	{
		if (nowState != stopState)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
