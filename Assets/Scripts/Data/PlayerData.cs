using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/CreatePlayerDataAsset")]
public class PlayerData : ScriptableObject
{
	[Header("基本項目")]
	// 最大HP
	[SerializeField, Tooltip("最大HP")]
	private int maxHp;
	public int MaxHp
	{
		get { return maxHp; }
		set { maxHp = value; }
	}


	// 最低速度
	[SerializeField,Tooltip("最低速度")]
	private float minSpeed;
	public float MinSpeed
	{
		get { return minSpeed; }
		private set { minSpeed = value; }
	}

	// 最高速度
	[SerializeField, Tooltip("最高速度")]
	private float maxSpeed;
	public float MaxSpeed
	{
		get { return maxSpeed; }
		private set { maxSpeed = value; }
	}

	[SerializeField, Tooltip("ブーストをしていない時の速度減衰")]
	private float speedDecay = 1.0f;

	public float SpeedDecay
	{
		get { return speedDecay; }
		set { speedDecay = value; }
	}

	// ブレーキ力
	[SerializeField, Tooltip("ブレーキ力")]
	private float brakePower = 1.0f;
	public float BrakePower
	{
		get { return brakePower; }
		set { brakePower = value; }
	}

	// 姿勢を直すまでの時間
	[SerializeField, Tooltip("姿勢を直すまでの時間")]
	private float rollControllTime = 1.0f;
	public float RollControllTime
	{
		get { return rollControllTime; }
		private set { rollControllTime = value; }
	}

	// 姿勢を直す速さ
	[SerializeField, Tooltip("姿勢を直す速さ")]
	private float returnRollSpeed = 1.0f;

	public float ReturnRollSpeed
	{
		get { return returnRollSpeed; }
		set { returnRollSpeed = value; }
	}


	[Header("ブーストの設定")]
	[SerializeField, Tooltip("合計ブースト量")]
	private float totalBoost = 100.0f;
	public float TotalBoost
	{
		get { return totalBoost; }
		set { totalBoost = value; }
	}

	[SerializeField, Tooltip("1秒間で回復するブーストの量")]
	private float boostCharge = 20.0f;
	public float BoostCharge
	{
		get { return boostCharge; }
		set { boostCharge = value; }
	}



	[Header("ノックバックの設定")]
	// 壁にぶつかった際のノックバックする力
	[SerializeField, Tooltip("壁にぶつかった際のノックバックする力")]
	private float knockbackPower = 10.0f;
	public float KnockbackPower
	{
		get { return knockbackPower; }
		private set { knockbackPower = value; }
	}

	// ノックバックしてから進む速度
	[SerializeField,Tooltip("ノックバックしてから進む速度")]
	private float returnSpeed;

	public float ReturnSpeed
	{
		get { return returnSpeed; }
		set { returnSpeed = value; }
	}

	[Header("被弾時の設定")]
	// 気絶時間
	[SerializeField, Tooltip("気絶時間")]
	private float stunTime = 3.0f;
	public float StunTime
	{
		get { return stunTime; }
		set { stunTime = value; }
	}

	// 回転量
	[SerializeField, Tooltip("被弾したときに自機の合計回転数")]
	private int stunRotation = 4;
	public int StunRotation
	{
		get { return stunRotation; }
		set { stunRotation = value; }
	}



	[Header("当たり判定の設定")]

	// 2本のRayの間隔
	[SerializeField, Tooltip("2本のRayの間隔")]
	private float rayOffset = 0.8f;
	public float RayOffset
	{
		get { return rayOffset; }
		set { rayOffset = value; }
	}

	// Rayの長さ
	[SerializeField, Tooltip("Rayの長さ")]
	private float rayDistance = 3.0f;
	public float RayDistance
	{
		get { return rayDistance; }
		set { rayDistance = value; }
	}

	// 地面との最短距離
	[SerializeField, Tooltip("地面との最短距離")]
	private float minHeight = 2.0f;
	public float MinHeight
	{
		get { return minHeight; }
		set { minHeight = value; }
	}


	[Header("各入力デバイスの感度設定")]
	// キーボード
	[SerializeField, Tooltip("キーボードでの旋回感度")]
	private Vector3 keyboardSens = new Vector3(1.2f, 1.0f, 0.8f);
	public Vector3 KeyboardSens
	{
		get { return keyboardSens; }
		set { keyboardSens = value; }
	}

	// ジョイコン
	[SerializeField, Tooltip("ジョイコンでの旋回感度")]
	private Vector3 joyconSens = new Vector3(1.0f, 1.0f, 1.0f);
	public Vector3 JoyconSens
	{
		get { return joyconSens; }
		set { joyconSens = value; }
	}

	// スマートフォン
	[SerializeField, Tooltip("スマートフォンでの旋回感度")]
	private Vector3 smartphoneSens = new Vector3(1.0f, 1.0f, 1.0f);
	public Vector3 SmartphoneSens
	{
		get { return smartphoneSens; }
		set { smartphoneSens = value; }
	}

	[Header("ジョイコンの設定")]

	[SerializeField, Tooltip("ジョイコンの最大角度")]
	private Vector2 maxJoyconRot = new Vector2(90.0f, 90.0f);
	public Vector2 MaxJoyconRot
	{
		get { return maxJoyconRot; }
		set { maxJoyconRot = value; }
	}

	[SerializeField, Tooltip("ジョイコンのデッドゾーン（反応しない範囲0.0から1.0まで）")]
	private Vector2 joyconDeadzone = new Vector2(0.1f, 0.1f);
	public Vector2 JoyconDeadzone
	{
		get { return joyconDeadzone; }
		set { joyconDeadzone = value; }
	}

	[SerializeField, Tooltip("アクセルを判定する右ジョイコンの角度")]
	private float accelRot = 30.0f;
	public float AccelRot
	{
		get { return accelRot; }
		set { accelRot = value; }
	}

	[SerializeField, Tooltip("アクセルを捻った時の加速の大きさ")]
	private float accelPower = 1.0f;
	public float AccelPower
	{
		get { return accelPower; }
		set { accelPower = value; }
	}


	[Header("アイテムの設定")]
	// 最高速度のアイテム
	[SerializeField, Tooltip("最高速度のアイテム")]
	private float item_maxSpeed = 3.0f;
	public float Item_maxSpeed
	{
		get { return item_maxSpeed; }
		set { item_maxSpeed = value; }
	}

	// 加速度のアイテム
	[SerializeField, Tooltip("加速度のアイテム")]
	private float item_acceleration = 0.01f;
	public float Item_acceleration
	{
		get { return item_acceleration; }
		set { item_acceleration = value; }
	}

	// 軽量のアイテム
	[SerializeField, Tooltip("軽量のアイテム")]
	private float item_lightness = 0.1f;
	public float Item_lightness
	{
		get { return item_lightness; }
		set { item_lightness = value; }
	}

	// 旋回のアイテム
	[SerializeField, Tooltip("旋回のアイテム")]
	private Vector3 item_rotation = new Vector3(0.05f, 0.05f, 0.05f);
	public Vector3 Item_rotation
	{
		get { return item_rotation; }
		set { item_rotation = value; }
	}

	// 空気砲のチャージアイテム
	[SerializeField, Tooltip("空気砲のチャージアイテム")]
	private float item_charge = -0.05f;
	public float Item_charge
	{
		get { return item_charge; }
		set { item_charge = value; }
	}


}
