using UnityEngine;


[CreateAssetMenu(fileName = "BulletData", menuName = "ScriptableObjects/CreateBulletDataAsset")]

public class BulletData : ScriptableObject
{
	// 通常弾の速度
	[SerializeField, Tooltip("通常弾の速度")]
	private float bulletSpeed = 400;
	public float BulletSpeed
	{
		get { return bulletSpeed; }
		set { bulletSpeed = value; }
	}

	// チャージ弾の速度
	[SerializeField, Tooltip("チャージ弾の速度")]
	private float chargeBulletSpeed = 400;
	public float ChargeBulletSpeed
	{
		get { return chargeBulletSpeed; }
		set { chargeBulletSpeed = value; }
	}

	// 速度の上限
	[SerializeField, Tooltip("速度の上限")]
	private float maxSpeed = 800;
	public float MaxSpeed
	{
		get { return maxSpeed; }
		set { maxSpeed = value; }
	}

	// 加速度
	[SerializeField, Tooltip("加速度")]
	private float acceleration = 5;
	public float Acceleration
	{
		get { return acceleration; }
		set { acceleration = value; }
	}

	// リロード時間
	[SerializeField, Tooltip("リロード時間")]
	private float reloadTime = 2.0f;
	public float ReloadTime
	{
		get { return reloadTime; }
		set { reloadTime = value; }
	}

	// 最大チャージ時間
	[SerializeField, Tooltip("最大チャージ時間")]
	private float maxChargeTime = 1.0f;
	public float MaxChargeTime
	{
		get { return maxChargeTime; }
		set { maxChargeTime = value; }
	}

	// 最大弾数
	[SerializeField, Tooltip("最大弾数")]
	private int maxBullet = 6;
	public int MaxBullet
	{
		get { return maxBullet; }
		set { maxBullet = value; }
	}

	// 追尾時間
	[SerializeField, Tooltip("追尾時間")]
	private float homingTime = 5.0f;
	public float HomingTime
	{
		get { return homingTime; }
		set { homingTime = value; }
	}

	// チャージ弾の追尾時間
	[SerializeField, Tooltip("チャージ弾の追尾時間")]
	private float chargeHomingTime = 5.0f;
	public float ChargeHomingTime
	{
		get { return chargeHomingTime; }
		set { chargeHomingTime = value; }
	}


	// 追尾するまで（まっすぐ飛ぶ）の時間
	[SerializeField, Tooltip("追尾時間")]
	private float beforeHomingTime = 1.0f;
	public float BeforeHomingTime
	{
		get { return beforeHomingTime; }
		set { beforeHomingTime = value; }
	}

	// 追尾時にどのくらい追尾するか(弾がどのくらい曲がるか)
	[SerializeField, Tooltip("追尾時にどのくらい追尾するか(弾がどのくらい曲がるか)")]
	private float homingPower = 100.0f;
	public float HomingPower
	{
		get { return homingPower; }
		set { homingPower = value; }
	}

}
