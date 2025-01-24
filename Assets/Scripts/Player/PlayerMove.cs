using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Collections.Generic;
using Photon.Realtime;
using System.Collections;
using Unity.VisualScripting;
using System.Globalization;


//MonoBehaviour->MonoBehaviourPunCallbacks�ɕύX
public class PlayerMove : MonoBehaviourPunCallbacks, IPunObservable
{
	// �v���C���[�̓��̓X�N���v�g�̔z��
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


	// �v���C���[�̃f�[�^
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


	// �v���C���[�̃��f���f�[�^
	[SerializeField]
	private GameObject modelObj = null;
	public GameObject ModelObj
	{
		get { return modelObj; }
		set { modelObj = value; }
	}

	// �J�����̃f�[�^
	[SerializeField]
	private CameraData cameraData = null;
	public CameraData CameraData
	{
		get { return cameraData; }
		set { cameraData = value; }
	}

	// �v���C���[�̏��
	private IState nowState = null;

	public IState NowState
	{
		get { return nowState; }
		private set { nowState = value; }
	}

    // --------------------------
    // �e��Ǘ��p�N���X
    // --------------------------
    public ItemManager ItemManager { get; set; }
    public BulletManager BulletManager { get; set; }
    public EnemyManager EnemyManager { get; set; }

    // --------------------------
    // UI�֘A�N���X
    // --------------------------
    private ScoreUI scoreUI = null;
	private HpUI hpUI = null;


    // --------------------------
    // �e��Ԃ̎���
    //---------------------------
    // �e��Ԃ̎��̂̃��X�g
    List<IState> stateList = new List<IState>();
	[SerializeField]
	private PLAYER_STATE s;

    // �ʏ���
    private Idle idleState;
	public Idle IdleState
	{
		get { return idleState; }
		private set { idleState = value; }
	}
	// �m�b�N�o�b�N���
	private Knockback knockbackState;
	public Knockback KnockbackState
	{
		get { return knockbackState; }
		private set { knockbackState = value; }
	}
	// �C��i��e�j���
	private Stun stunState;
	public Stun StanState
	{
		get { return stunState; }
		private set { stunState = value; }
	}
	// ���S���
	private Dead deadState;
	public Dead DeadState
	{
		get { return deadState; }
		set { deadState = value; }
	}
	//�������
	private Stop stopState;
	public Stop StopState
	{
		get { return stopState; }
		set { stopState = value; }
	}

	//------------------------
	// ��{�X�e�[�^�X
	//------------------------

	// ���݂̃X�s�[�h
	[SerializeField]
	private float speed;
	public float Speed
	{
		get { return speed; }
		set { speed = value; }
	}

	// �ō����x(�A�C�e���̌���,�C�x���g�̉e�����܂߂��ŏI�I�ȍō����x)
	public float MaxSpeed
	{
		get 
		{
			return data.MaxSpeed + (getItems[(int)Item.ITEM_EFFECT.MAX_SPEED - 1] * data.Item_maxSpeed) + EventSpeed; 
		}
	}


	// �A�N�Z���𓥂�ł��邩�̃t���O
	private bool isAccel;
	public bool IsAccel
	{
		get { return isAccel; }
		set { isAccel = value; }
	}

	// �u���[�L�𓥂�ł��邩�̃t���O
	private bool isBrake;
	public bool IsBrake
	{
		get { return isBrake; }
		set { isBrake = value; }
	}

	// ���̃t���[���ŉ�]���Ă��邩��\���t���O
	[SerializeField]
	private bool isRotate = false;
	public bool IsRotate
	{
		get { return isRotate; }
		set { isRotate = value; }
	}

	// ���̃t���[���Ńu�[�X�g�i�A�N�Z���j���Ă��邩��\���t���O
	[SerializeField]
	private bool isBoost = false;
	public bool IsBoost
	{
		get { return isBoost; }
		set { isBoost = value; }
	}

	public bool IsShot { get; set; } = false;
	public bool IsCharging { get; set; } = false;


    // ���G���ǂ���
    [SerializeField]
	private bool isInvincible = false;
	public bool IsInvincible
	{
		get { return isInvincible; }
		set { isInvincible = value; }
	}


	// �ڒn���������4������ray
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
	// �u�[�X�g�n
	//---------------------
	[SerializeField]
	private float totalBoostPower = 0.0f;

	public float TotalBoostPower
	{
		get { return totalBoostPower; }
		set { totalBoostPower = value; }
	}



	//---------------------
	// �A�C�e���n
	//---------------------

	// �A�C�e���̎�ނ��Ƃ̏������Ă��鐔
	private int[] getItems = new int[Item.ITEM_COUNT];
	public int[] GetItems
	{
		get { return  getItems; }
		set {  getItems = value; }
	}

	//-----------------------
	// �C�x���g�n
	//-----------------------
	// ���݂̃C�x���g�̎��
	[SerializeField]
	private EventManager.EVENT_TYPE eventType = EventManager.EVENT_TYPE.NONE;
	public EventManager.EVENT_TYPE EventType
	{
		get { return eventType; }
		set 
		{ 
			var beforType = eventType;
			eventType = value;
			// �C�x���g���I�������Ȃ�G�t�F�N�g���~�߂�
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

	// �X�s�[�h�ɉe��������C�x���g�̑��x
    public float EventSpeed { get; set; }
    // �C���n�C�x���g�̋C���̕���
    public Vector3 burstVec { get; set; } = Vector3.zero;
    // �����C�x���g���̐�����Z�l
    public Vector3 tornadoSens { get; set; }

    //-----------------------------
    // �e�X�g�p
    //-----------------------------

    private float invincibleTime = 5.0f;

	//�e�X�g�p�@���̃p�[�e�B�N���V�X�e��
	[SerializeField]
    private GameObject particleObject;

    [SerializeField]
    private GameObject UI;

    private PlayerUI uiScript;

    public bool effectFlag;

    //��HP
    private int life;

    [SerializeField,Header("�J�ڐ�̃V�[��")]
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
			//UI�𐶐�
			if (UI != null)
			{
				GameObject pUI = Instantiate(UI);
				var canvas=pUI.GetComponent<Canvas>();
				pUI.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
				uiScript = pUI.GetComponent<PlayerUI>();
				// ���e�B�N���N���X�Ƀ��[�J���v���C���[��n��
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
		// �V�[���Ǘ��p�N���X�Ɏ�����o�^
		//SceneController.instance.AddPlayer(this);
		nowState = stopState;

	}

	private void Start()
	{
		// �e��Ǘ��p�N���X���擾
		ItemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
		BulletManager = GameObject.FindGameObjectWithTag("BulletManager").GetComponent<BulletManager>();
		EnemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
		// �v���C���[�̃}�e���A����ς���
		//if (materials.Count > photonView.ControllerActorNr)
		//{
		//	koiModel.GetComponent<SkinnedMeshRenderer>().material = materials[photonView.ControllerActorNr - 1];
		//}
		// �X�s�[�h���Œ�X�s�[�h�ŏ�����
		//speed = data.MinSpeed;
        //GetComponent<Rigidbody>().velocity = transform.forward * speed;
		// �����Ă���A�C�e���̐���������
		for(int i=0;i<Item.ITEM_COUNT;i++)
		{
			getItems[i] = 0;
		}

        life = 3;
        effectFlag = false;

        //if (photonView.IsMine)
        {
			// �̗͂��ő��
			int hp = Data.MaxHp;
			//PlayerProperties.SetHp(photonView.Controller, hp);
			// �X�R�A��������
			int score = 0;
			//PlayerProperties.SetScore(photonView.Controller, score);
			// �u�[�X�g�ʂ��ő��
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
        //���������������I�u�W�F�N�g�݂̂̃A�b�v�f�[�g�����s����
        //if(photonView.IsMine)
        {
			if(Input.GetKeyDown(KeyCode.R))
			{
				TakeDamage(1);
			}
			nowState.Update();
			if (!IsBoost)
			{
				// �u�[�X�g�ʂ���
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
		//���������������I�u�W�F�N�g�݂̂̃A�b�v�f�[�g�����s����
		//if (photonView.IsMine)
        {
            //if(uiScript.timeLost)
            //{
            //    LoadScene();
            //}

            //�G�t�F�N�g�̉���������
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
    		// 4���������[�v
    		foreach (var ray in rays)
    		{
    			// Ray���쐬
    			ray.CreateRay();
    			// Ray���ˏo
    			ray.ShotRay();
    			// �O��ǂ����Ray���������Ă����Ȃ�
    			if (ray.isGrounding)
    			{
    				isFollowWall = true;
    				// �ǂɉ����ĉ�]
    				ray.RotateFollowWall();
    				// �ǂƂ̍ŒZ������ۂ�
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
				// UI���X�V
				scoreUI.SetScoreUI(score);
			}
			int hp = (changedProps["h"] is int h) ? h : -1;
			if (hp >= 0 && hpUI != null)
			{
				// UI���X�V
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
				// �@�����玩�@�ƕǂ̊p�x���v�Z
				Vector3 wallNormal = collision.contacts[0].normal;
				Vector3 dir = transform.forward;    // ���@�̌���
				float angle = Vector3.Angle(wallNormal, dir);
				Debug.Log(angle);
				// ���@�����ʂ���Փ˂�������납��Փ˂������`�F�b�N
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
		// �e���ǂ����`�F�b�N
		if (other.gameObject.tag == "Bullet")
		{
			// ���������삵�Ă���@�̈ȊO��
			if(!photonView.IsMine)
			{
				var bullet = other.GetComponent<HomingBullet>();
				// ���[�J���v���C���[��������e���`�F�b�N
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
		Debug.Log(photonView.Controller.ActorNumber + "���_���[�W���󂯂܂�");
		// �_���[�W���󂯂�
		int hp = PlayerProperties.GetHp(photonView.Controller);
		hp -= damage;
		if(hp < 0)
		{
			hp = 0;
		}
		PlayerProperties.SetHp(photonView.Controller, hp);

		if (hp > 0)
		{
			// ��e��Ԃ�
			photonView.RPC(nameof(ChangeState), RpcTarget.All, PLAYER_STATE.STUN);
		}
		// 0�ȉ��ɂȂ����Ȃ猂�j�����
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
			// ���j���̏���
			photonView.RPC(nameof(ChangeState), RpcTarget.All, PLAYER_STATE.DEAD);
		}
	}

	/// <summary>
	/// �擾�����A�C�e���𔽉f
	/// </summary>
	/// <param name="itemEffect">�A�C�e���̌���</param>
	/// <param name="itemType">�A�C�e���̎��</param>
	public void GetItem(Item.ITEM_EFFECT itemEffect,Item.ITEM_TYPE itemType)
	{
		int n = (int)itemEffect - 1;
		if (itemType==Item.ITEM_TYPE.PLUS)
		{
			getItems[n]++;
			// �ō����𒴂����Ȃ�ō�����
			if (getItems[n] > Item.ITEM_MAX)
			{
				getItems[n]=Item.ITEM_MAX;
			}
		}
		else
		{
			getItems[n]--;
			// �ō����𒴂����Ȃ�ō�����
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
		// ��]������1�b�Ԃɉ�]���鐔�l���Z�o
		float rotationY = (Data.StunRotation * 360.0f) / Data.StunTime;
		// ���f���̊p�x��߂�
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
			// �C�⎞�Ԃ𒴂����Ȃ�I��
			if (nowTime >= Data.StunTime)
			{
				// ���f���̊p�x��߂�
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
	/// ���̏�Ԃ֑J�ڂ���֐�
	/// </summary>
	/// <param name="nextStateType">���̏��</param>
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
