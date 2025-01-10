using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "ScriptableObjects/CreateCameraDataAsset")]
public class CameraData : ScriptableObject
{
	// カメラの最低FOV
	[SerializeField, Tooltip("カメラの最低FOV")]
	private float minFov = 65.0f;
	public float MinFov
	{
		get { return minFov; }
		private set { minFov = value; }
	}

	// カメラの最高FOV
	[SerializeField, Tooltip("カメラの最高FOV")]
	private float maxFov = 100.0f;
	public float MaxFov
	{
		get { return maxFov; }
		private set { maxFov = value; }
	}

	// FOVの加算値（加速時）
	[SerializeField, Tooltip("FOVの加算値（加速時）")]
	private float addFov = 0.85f;
	public float AddFov
	{
		get { return addFov; }
		private set { addFov = value; }
	}

	// FOVの減算値（減速時）
	[SerializeField, Tooltip("FOVの減算値（減速時）")]
	private float subFov = 0.15f;
	public float SubFov
	{
		get { return subFov; }
		private set { subFov = value; }
	}

	// 自機が回転していないときのオフセット（変更するならYだけ）
	[SerializeField, Tooltip("自機が回転していないときのオフセット（変更するならYだけ）")]
	private Vector3 startTrackedOffset = new Vector3(0.0f, 1.4f, 0.0f);
	public Vector3 StartTrackedOffset
	{
		get { return startTrackedOffset; }
		set { startTrackedOffset = value; }
	}


	// 自機が回転している際に自機を端に寄せるオフセット
	[SerializeField, Tooltip("自機が回転した際に自機を端に寄せるオフセット")]
	private Vector3 trackedOffset = new Vector3(2.5f, 3.0f, 0.0f);
	public Vector3 TrackedOffset
	{
		get { return trackedOffset; }
		set { trackedOffset = value; }
	}


	// 自機が回転している際に自機が端に寄るまでの時間(3方向それぞれに設定できる)
	[SerializeField, Tooltip("自機が回転している際に自機が端に寄るまでの時間(3方向それぞれに設定できる)")]
	private Vector3 trackedTime = new Vector3(3.2f,5.0f,0.0f);
	public Vector3 TrackedTime
	{
		get { return trackedTime; }
		set { trackedTime = value; }
	}

	// 自機が端によって真ん中まで戻ってくる時間(3方向それぞれに設定できる)
	[SerializeField, Tooltip("自機が端によって真ん中まで戻ってくる時間(3方向それぞれに設定できる)")]
	private Vector3 returnTime = new Vector3(3.2f, 5.0f, 0.0f);
	public Vector3 ReturnTime
	{
		get { return returnTime; }
		set { returnTime = value; }
	}

	// レティクルの標準距離
	[SerializeField, Tooltip("レティクルの標準距離")]
	private float reticleDistance = 100.0f;
	public float ReticleDistance
	{
		get { return reticleDistance; }
		set { reticleDistance = value; }
	}


}

