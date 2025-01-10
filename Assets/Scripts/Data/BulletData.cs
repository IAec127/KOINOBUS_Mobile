using UnityEngine;


[CreateAssetMenu(fileName = "BulletData", menuName = "ScriptableObjects/CreateBulletDataAsset")]

public class BulletData : ScriptableObject
{
	// �ʏ�e�̑��x
	[SerializeField, Tooltip("�ʏ�e�̑��x")]
	private float bulletSpeed = 400;
	public float BulletSpeed
	{
		get { return bulletSpeed; }
		set { bulletSpeed = value; }
	}

	// �`���[�W�e�̑��x
	[SerializeField, Tooltip("�`���[�W�e�̑��x")]
	private float chargeBulletSpeed = 400;
	public float ChargeBulletSpeed
	{
		get { return chargeBulletSpeed; }
		set { chargeBulletSpeed = value; }
	}

	// ���x�̏��
	[SerializeField, Tooltip("���x�̏��")]
	private float maxSpeed = 800;
	public float MaxSpeed
	{
		get { return maxSpeed; }
		set { maxSpeed = value; }
	}

	// �����x
	[SerializeField, Tooltip("�����x")]
	private float acceleration = 5;
	public float Acceleration
	{
		get { return acceleration; }
		set { acceleration = value; }
	}

	// �����[�h����
	[SerializeField, Tooltip("�����[�h����")]
	private float reloadTime = 2.0f;
	public float ReloadTime
	{
		get { return reloadTime; }
		set { reloadTime = value; }
	}

	// �ő�`���[�W����
	[SerializeField, Tooltip("�ő�`���[�W����")]
	private float maxChargeTime = 1.0f;
	public float MaxChargeTime
	{
		get { return maxChargeTime; }
		set { maxChargeTime = value; }
	}

	// �ő�e��
	[SerializeField, Tooltip("�ő�e��")]
	private int maxBullet = 6;
	public int MaxBullet
	{
		get { return maxBullet; }
		set { maxBullet = value; }
	}

	// �ǔ�����
	[SerializeField, Tooltip("�ǔ�����")]
	private float homingTime = 5.0f;
	public float HomingTime
	{
		get { return homingTime; }
		set { homingTime = value; }
	}

	// �`���[�W�e�̒ǔ�����
	[SerializeField, Tooltip("�`���[�W�e�̒ǔ�����")]
	private float chargeHomingTime = 5.0f;
	public float ChargeHomingTime
	{
		get { return chargeHomingTime; }
		set { chargeHomingTime = value; }
	}


	// �ǔ�����܂Łi�܂�������ԁj�̎���
	[SerializeField, Tooltip("�ǔ�����")]
	private float beforeHomingTime = 1.0f;
	public float BeforeHomingTime
	{
		get { return beforeHomingTime; }
		set { beforeHomingTime = value; }
	}

	// �ǔ����ɂǂ̂��炢�ǔ����邩(�e���ǂ̂��炢�Ȃ��邩)
	[SerializeField, Tooltip("�ǔ����ɂǂ̂��炢�ǔ����邩(�e���ǂ̂��炢�Ȃ��邩)")]
	private float homingPower = 100.0f;
	public float HomingPower
	{
		get { return homingPower; }
		set { homingPower = value; }
	}

}
