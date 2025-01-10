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
    public GameObject bulletPrefab; // �ʏ�̒e�̃v���n�u
    public GameObject chargeBulletPrefab;
    public Transform firePoint;
    public float chargeTime = 0f; // �`���[�W����
    [SerializeField]
    private int currentShots = 0; // ���݂̒e��
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

	private bool isReloading = false; // �����[�h�����ǂ���
    private bool isChargeComplete = false; // �`���[�W�����t���O
    [SerializeField]
    private bool isLockedOn = false; // ���b�N�I���������ǂ���

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
        //if(photonView.IsMine)
        {
            if (currentShots > 0)
            {
                LockOn();
            }

            // �`���[�W����
            HandleCharge();

            // ���ˏ���
            if (player.IsShot)
			{
				Fire();
			}

            // �����[�h�������m�F
            if (currentShots < data.MaxBullet && isReloading == false)
			{
				StartCoroutine(Reload());
			}
		}
	}

    void HandleCharge()
    {
        if (player.IsCharging)
        {
            chargeTime = Mathf.Min(chargeTime + Time.deltaTime, data.MaxChargeTime+ (player.GetItems[(int)Item.ITEM_EFFECT.CHARGE - 1] * player.Data.Item_charge));

            // �`���[�W���ԕ����������邩�����C�x���g���Ȃ�`���[�W����
            if ((chargeTime >= data.MaxChargeTime + (player.GetItems[(int)Item.ITEM_EFFECT.CHARGE - 1] * player.Data.Item_charge) ||
					player.EventType == EventManager.EVENT_TYPE.CRAZY_WIND) && !isChargeComplete)
            {
                Debug.Log("�`���[�W�����I");
                isChargeComplete = true;
            }
        }
    }

    void ResetCharge()
    {
        chargeTime = 0f;
        isChargeComplete = false;
    }

    private void LockOn()
    {
		TARGET_TYPE beforeType = targetType;
		int beforeID = targetID;
		// �^�[�Q�b�g��T��
		var targetTrans = FindClosestEnemy();
        // �^�[�Q�b�g�����߂ă��b�N�I�������Ƃ�
        if (beforeType == TARGET_TYPE.NONE && targetType != TARGET_TYPE.NONE)
        {
            // ���b�N�I��UI��\��
            lockOnUI.ActiveUI();
            lockOnUI.Target = targetTrans;
        }
        // �����^�[�Q�b�g�����b�N�I���������Ă��鎞
        else if (beforeType == targetType && beforeID == targetID && targetType != TARGET_TYPE.NONE)
        {
            // ���b�N�I�����Ԃ��X�V
            lockonTime += Time.deltaTime;
            if (lockonTime > maxLockOnTime)
            {
                Debug.Log("���b�N�I�����܂���");
                lockOnUI.LockedOn();
                lockonTime = 0.0f;
                isLockedOn = true;
            }
        }
        // ���b�N�I�����Ă���^�[�Q�b�g�����Ȃ��Ȃ�����
        else if (beforeType != TARGET_TYPE.NONE && targetType == TARGET_TYPE.NONE)
        {
            // ���b�N�I��UI���\��
            lockOnUI.InActiveUI();
            lockOnUI.LockOut();
            // ���b�N�I�����Ԃ�������
            lockonTime = 0.0f;
            isLockedOn = false;
        }
        // �Ⴄ�^�[�Q�b�g�����b�N�I��������
        else if (targetType != TARGET_TYPE.NONE && (targetType != beforeType || targetID != beforeID))
		{
			lockOnUI.Target = targetTrans;
			lockOnUI.LockOut();
			// ���b�N�I�����Ԃ�������
			lockonTime = 0.0f;
			isLockedOn = false;
        }
        else
        {
        }

	}

	void Fire()
    {
        // �����[�h���̃`�F�b�N�͕s�v�ɂȂ���
        int shotsRequired = isChargeComplete ? 3 : 1;

        if (currentShots - shotsRequired < 0)
        {
            Debug.Log("�e��������܂���I");
            ResetCharge();
            return;
        }

        shotEffect.Play();
        if(isLockedOn)
        {
            bulletManager.Fire(photonView.OwnerActorNr, firePoint.transform, isChargeComplete, targetType, targetID);
        }
        else
        {
			bulletManager.Fire(photonView.OwnerActorNr, firePoint.transform, isChargeComplete, TARGET_TYPE.NONE, -1);
		}

		// �����C�x���g���Ȃ�e������Ȃ�
		if (player.EventType!=EventManager.EVENT_TYPE.CRAZY_WIND)
        {
            currentShots -= shotsRequired;
        }
        if(isLockedOn)
        {
			lockOnUI.LockOut();
			// ���b�N�I�����Ԃ�������
			lockonTime = 0.0f;
			isLockedOn = false;
		    ResetCharge();
        }
    }

	private Transform FindClosestEnemy()
	{
        // �^�[�Q�b�g�̌��ƂȂ�G�ƃv���C���[���擾
		List<Enemy> enemies = enemyManager.GetEnemyList();
        List<PlayerMove> players = SceneController.instance.Players;
        List<Enemy> targetEnemyList = new List<Enemy>();
		List<PlayerMove> targetPlayerList = new List<PlayerMove>();

        Transform targetTrans = null;

		// �v���C���[�̕���
		Vector3 playerForward = player.transform.forward;

		// ��ʂ̒����t�߂ɂ���G�݂̂����
		for (int i = 0; i < enemies.Count; i++)
		{
			// �I�u�W�F�N�g�ւ̕���
			Vector3 toObject = (enemies[i].transform.position - player.transform.position).normalized;

			// �J�����̕����ƃI�u�W�F�N�g�̕����̊p�x���v�Z
			float angle = Vector3.Angle(playerForward, toObject);
			if (angle <= maxAngle)
			{
				targetEnemyList.Add(enemies[i]);
			}
		}

		// ��ʂ̒����t�߂ɂ���v���C���[�݂̂����
		for (int i = 0; i < players.Count; i++)
		{
			if (players[i].photonView.ControllerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                continue;
            }
            // �v���C���[�����łɔ�e���Ă��邩���G��ԂȂ�^�[�Q�b�g���X�g�ɓ���Ȃ�
            if (player.NowState.State == PLAYER_STATE.STUN || player.NowState.State == PLAYER_STATE.DEAD)
            {
                continue;
            }
            if (player.IsInvincible)
            {
                continue;
            }
			// �I�u�W�F�N�g�ւ̕���
			Vector3 toObject = (players[i].transform.position - player.transform.position).normalized;

			// �J�����̕����ƃI�u�W�F�N�g�̕����̊p�x���v�Z
			float angle = Vector3.Angle(playerForward, toObject);
			if (angle <= maxAngle)
			{
				targetPlayerList.Add(players[i]);
			}
		}

		float closestDistance = Mathf.Infinity;

        targetID = -1;
        targetType = TARGET_TYPE.NONE;
        // �^�[�Q�b�g���X�g�̒������ԋ߂��v���C���[���^�[�Q�b�g��
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
		// �^�[�Q�b�g���X�g�̒������ԋ߂��G���^�[�Q�b�g��
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
            Debug.Log("�e�̃^�[�Q�b�g��������܂���ł���");
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