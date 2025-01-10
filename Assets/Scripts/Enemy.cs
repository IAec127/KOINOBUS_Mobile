using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb = null;
    [SerializeField]
    private SkinnedMeshRenderer enemyRenderer;
    public SkinnedMeshRenderer EnemyRendrer
    {
        get { return enemyRenderer; }
        set { enemyRenderer = value; }
    }

    [SerializeField]
	ParticleSystem deadEffect = null;
    private BulletManager bulletManager = null;
    public EnemyManager EnemyManager { get; set; }
    public int ID { get; set; }
    public float MaxDistance { get; set; } = 0.0f; // �i�ދ���
    public Vector3 MoveVec { get; set; } = Vector3.zero;    // �i�ޕ���
    public float Speed { get; set; } = 0.0f;    // �i�ޑ���
    private float nowDistance = 0.0f;   // ���݂̐i�񂾋���

    // Start is called before the first frame update
    void Start()
    {
        bulletManager = GameObject.FindGameObjectWithTag("BulletManager").GetComponent<BulletManager>();
        transform.forward = MoveVec;
    }

	private void FixedUpdate()
	{
        var vec = MoveVec * Speed * Time.fixedDeltaTime;
		rb.linearVelocity = vec;
        nowDistance += vec.magnitude;
        if(nowDistance>=MaxDistance)
        {
            nowDistance = 0.0f;
			MoveVec *= -1.0f;
            transform.forward = MoveVec;
        }
	}

	private void OnTriggerEnter(Collider other)
	{
        // �e���ǂ����`�F�b�N
        if(other.gameObject.tag=="Bullet")
        {
            var bullet = other.GetComponent<HomingBullet>();
            // ���[�J���v���C���[�����������e���`�F�b�N
            if (PhotonNetwork.LocalPlayer.ActorNumber != bullet.OwnerID)
            {
                return;
            }
            // �������e�̃^�[�Q�b�g���m�F
    //        if (bullet.Target.gameObject != gameObject)
    //        {
				//return;
    //        }
			// ��e���̏���
			bulletManager.Remove(bullet);
            EnemyManager.Remove(this);
		}
	}

    public void PlayDeadEffect(Vector3 pos)
    {
		Instantiate(deadEffect, pos, Quaternion.Euler(-90.0f, 0.0f, 0.0f));
	}
}
