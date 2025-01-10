using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private GameObject bulletPrefab = null;
	[SerializeField]
	private GameObject chargeBulletPrefab = null;
	[SerializeField]
	BulletData data = null;
	[SerializeField]
	private GameObject parent = null;
	// ���ݐ�������Ă���A�C�e��
	List<HomingBullet> bulletList = new List<HomingBullet>();
	private int id = 0;

	// Start is called before the first frame update
	void Start()
	{
	}

	private HomingBullet FindBullet(int id, int ownerID)
	{
		// �A�C�e����T��
		HomingBullet findBullet = null;
		foreach (var bullet in bulletList)
		{
			if (bullet.ID == id && bullet.OwnerID == ownerID)
			{
				findBullet = bullet;
			}
		}
		return findBullet;
	}

	public void Fire(int ownerId, Transform transform, bool isCharged, BulletSpawner.TARGET_TYPE targetType,int targetID)
	{
		photonView.RPC(nameof(FireBullet), RpcTarget.All, ownerId, transform.position, transform.rotation, isCharged, (int)targetType, targetID);
	}

	[PunRPC]
	public void FireBullet(int ownerID, Vector3 pos, Quaternion rot, bool isCharged, int targetType, int targetID)
	{
		GameObject bulletObj = null;
		if (isCharged)
		{
			bulletObj = Instantiate(chargeBulletPrefab, pos, rot, parent.transform) ;
		}
		else
		{
			bulletObj = Instantiate(bulletPrefab, pos, rot, parent.transform);
		}
		HomingBullet bullet = bulletObj.GetComponent<HomingBullet>();
		float speed = isCharged ? data.ChargeBulletSpeed : data.BulletSpeed;
		if (bullet != null)
		{
			bullet.OwnerID = ownerID;
			bullet.ID = id++;
			bullet.InitializeBullet(speed, isCharged, (BulletSpawner.TARGET_TYPE)targetType, targetID);
			bullet.isCarge = isCharged;
			bullet.BulletManager = this;
		}
		// ���X�g�ɒǉ�
		bulletList.Add(bullet);
	}

	public void Remove(HomingBullet bullet)
	{
		photonView.RPC(nameof(DestroyBullet), RpcTarget.All, bullet.ID, bullet.OwnerID);
	}

	[PunRPC]
	public void DestroyBullet(int id, int ownerId)
	{
		// ����ID�œ�������҂̒e��T��
		HomingBullet bullet = FindBullet(id, ownerId);
		// ���X�g������폜
		bulletList.Remove(bullet);
		// ���̒e��j��
		if(bullet.gameObject != null)
		{
			Destroy(bullet.gameObject);
		}
	}

}
