using UnityEngine;

[CreateAssetMenu(fileName = "EventData", menuName = "ScriptableObjects/CreateEventDataAsset")]

public class EventData : ScriptableObject
{
	// イベントの時間
	[SerializeField, Tooltip("イベントの時間")]
	private float eventTime = 30;
	public float EventTime
	{
		get { return eventTime; }
		set { eventTime = value; }
	}

	// イベントを作成するまでの時間
	[SerializeField, Tooltip("イベントを作成するまでの時間")]
	private float createEventTime = 90;
	public float CreateEventTime
	{
		get { return createEventTime; }
		set { createEventTime = value; }
	}

	// 追い風で上昇する最高速度
	[SerializeField, Tooltip("追い風で上昇する最高速度")]
	private float tailWindSpeed = 30;
	public float TailWindSpeed
	{
		get { return tailWindSpeed; }
		set { tailWindSpeed = value; }
	}

	// 向かい風で減少する最高速度
	[SerializeField, Tooltip("向かい風で減少する最高速度")]
	private float headWindSpeed = -60;

	public float HeadWindSpeed
	{
		get { return headWindSpeed; }
		set { headWindSpeed = value; }
	}

	// 上昇気流のベクトル
	[SerializeField, Tooltip("上昇気流のベクトル")]
	private Vector3 upBurstVec = new Vector3(0.0f, 1.0f, 0.0f);
	public Vector3 UpBurstVec
	{
		get { return upBurstVec; }
		set { upBurstVec = value; }
	}

	// 下降気流のベクトル
	[SerializeField, Tooltip("下降気流のベクトル")]
	private Vector3 downBurstVec = new Vector3(0.0f, -1.0f, 0.0f);
	public Vector3 DownBurstVec
	{
		get { return downBurstVec; }
		set { downBurstVec = value; }
	}


	// 竜巻時の旋回速度の加算値
	[SerializeField, Tooltip("竜巻時の旋回速度の加算値")]
	private Vector3 tornadoSens = new Vector3(1.0f, 1.0f, 0.8f);
	public Vector3 TornadoSens
	{
		get { return tornadoSens; }
		set { tornadoSens = value; }
	}

	// 狂風時の増加した弾の追尾時間
	[SerializeField, Tooltip("狂風時の増加した弾の追尾時間")]
	private float crazyHomingTime = 5.0f;
	public float CrazyHomingTime
	{
		get { return crazyHomingTime; }
		set { crazyHomingTime = value; }
	}


	[Header("UI用テキストの設定")]

	[SerializeField]
	private float popupTime = 5.0f;
	public float PopupTime
	{
		get { return popupTime; }
		set { popupTime = value; }
	}


	[SerializeField]
	private string tailWindText = "突風が発生！30秒間機体の速度が最高速度に固定!!";
	public string TailWindText
	{
		get { return tailWindText; }
		set { tailWindText = value; }
	}

	[SerializeField]
	private string headWindText = "逆風が発生！30秒間機体の速度が最高速度が低下!!";
	public string HeadWindText
	{
		get { return headWindText; }
		set { headWindText = value; }
	}

	[SerializeField]
	private string upBurstText = "上昇気流が発生！30秒間機体が持ち上げられる!!";
	public string UpBurstText
	{
		get { return upBurstText; }
		set { upBurstText = value; }
	}

	[SerializeField]
	private string downBurstText = "ダウンバーストが発生！30秒間機体が沈む!!";
	public string DownBurstText
	{
		get { return downBurstText; }
		set { downBurstText = value; }
	}

	[SerializeField]
	private string tornadoText = "竜巻が発生！30秒間機体の旋回が上昇!!";
	public string TornadoText
	{
		get { return tornadoText; }
		set { tornadoText = value; }
	}

	[SerializeField]
	private string crazyWindText = "狂風が発生！30秒間弾を撃ち放題!!";
	public string CrazyWindText
	{
		get { return crazyWindText; }
		set { crazyWindText = value; }
	}

}
