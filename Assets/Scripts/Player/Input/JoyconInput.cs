using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JoyconInput : PlayerInput
{

    private static readonly Joycon.Button[] m_buttons =
    Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    List<Joycon> joyconList;
    private Joycon joyconL;     // 左のジョイコン
    private Joycon joyconR;     // 右のジョイコン

    private Vector3 gyroR = Vector3.zero;
    private Vector3 gyroL = Vector3.zero;


	[SerializeField]
    private Vector3 rotateOffset = Vector3.zero;    // ジャイロの誤差を無視するオフセット

    [SerializeField]
    private Vector3 totalGyroR;     // 右のジャイロの傾きの合計値
    [SerializeField]
    private Vector3 totalGyroL;     // 左のジャイロの傾きの合計値

    private Quaternion startQuatR = Quaternion.identity;
	private Quaternion startQuatL = Quaternion.identity;

    private Quaternion nowQuatR = Quaternion.identity;
	private Quaternion nowQuatL = Quaternion.identity;



	// Start is called before the first frame update
	private new void Start()
    {
		{
		    base.Start();
		    // ジョイコンを取得
		    joyconList = JoyconManager.Instance.j;
		    if (joyconList == null || joyconList.Count == 0)
		    {
			    Debug.Log("joyconが見つかりません");
		    }
		    else
		    {
			    // 左右のジョイコンを探す
			    joyconL = joyconList.Find(c => c.isLeft);
			    joyconR = joyconList.Find(c => !c.isLeft);

				// 最初のジョイコンの姿勢を取得
				startQuatL = joyconL.GetVector();
				startQuatL = new Quaternion(startQuatL.x, startQuatL.z, startQuatL.y, startQuatL.w);
				startQuatL = Quaternion.Inverse(startQuatL);

				startQuatR = joyconR.GetVector();
				startQuatR = new Quaternion(startQuatR.x, startQuatR.z, startQuatR.y, startQuatR.w);
				startQuatR = Quaternion.Inverse(startQuatR);


			}
			Yaw_Pitch_Role_Sensitivity = playerMove.Data.JoyconSens;
		}

	}

	// Update is called once per frame
	public new void Update()
    {
		{
		    base.Update();
            gyroR = Vector3.zero;
            gyroL = Vector3.zero;
            if (joyconList != null && joyconList.Count != 0)
            {
				// ジョイコンの前フレームとの角度の差分を取得
				//gyroR = joyconR.GetGyro();
				//gyroL = joyconL.GetGyro();

				// 現在の姿勢を取得

				nowQuatL = joyconL.GetVector();
				nowQuatL = new Quaternion(nowQuatL.x, nowQuatL.z, nowQuatL.y, nowQuatL.w);
				nowQuatL = Quaternion.Inverse(nowQuatL);

				nowQuatR = joyconR.GetVector();
				nowQuatR = new Quaternion(nowQuatR.x, nowQuatR.z, nowQuatR.y, nowQuatR.w);
				nowQuatR = Quaternion.Inverse(nowQuatR);
			}

			// 現在の姿勢の差を取得
			Quaternion deltaQuatL = Quaternion.Inverse(nowQuatL) * startQuatL;
			totalGyroL = GetNorm(deltaQuatL.eulerAngles);

			Quaternion deltaQuatR = Quaternion.Inverse(nowQuatR) * startQuatR;
			totalGyroR = GetNorm(deltaQuatR.eulerAngles);


			totalGyro = new Vector3(0.0f, -totalGyroL.y / playerMove.Data.MaxJoyconRot.x, totalGyroL.z / playerMove.Data.MaxJoyconRot.y);
			// デッドゾーン判定
			if (Mathf.Abs(totalGyro.y) <= playerMove.Data.JoyconDeadzone.x)
			{
				totalGyro.y = 0.0f;
			}
			if (Mathf.Abs(totalGyro.z) <= playerMove.Data.JoyconDeadzone.y)
			{
				totalGyro.z = 0.0f;
			}

			// totalGyro += joyconL.GetGyro() * Mathf.Rad2Deg * Time.deltaTime;

			//Debug.Log(joyconL.GetGyroRaw());
			//Debug.Log(joyconR.GetGyroRaw());

			// 左右のジョイコンのx軸の角度の差から右のジョイコンだけひねっているか（アクセル）を判定
			float offsetGyroX = totalGyroR.y + totalGyroL.y;
			if (offsetGyroX > playerMove.Data.AccelRot)
			{
				float speed = (offsetGyroX * playerMove.Data.AccelPower * Time.deltaTime) +
					(playerMove.Data.Item_acceleration * playerMove.GetItems[(int)Item.ITEM_EFFECT.ACCELERATION - 1]);
				// ブースト残量をチェック
				speed = CheckCanBoost(speed);
				if (speed > 0.0f)
				{
					playerMove.Speed += speed;
					playerMove.IsAccel = true;	
				}
			}


			//      // 多少の傾きは無視
			//      if (gyroR.x < rotateOffset.x && gyroR.x > -rotateOffset.x)
			//      {
			//          gyroR.x = 0.0f;
			//      }
			//      if (gyroR.y < rotateOffset.y && gyroR.y > -rotateOffset.y)
			//      {
			//          gyroR.y = 0.0f;
			//      }
			//      if (gyroR.z < rotateOffset.z && gyroR.z > -rotateOffset.z)
			//      {
			//          gyroR.z = 0.0f;
			//      }
			//      if (gyroL.x < rotateOffset.x && gyroL.x > -rotateOffset.x)
			//      {
			//          gyroL.x = 0.0f;
			//      }
			//      if (gyroL.y < rotateOffset.y && gyroL.y > -rotateOffset.y)
			//      {
			//          gyroL.y = 0.0f;
			//      }
			//      if (gyroL.z < rotateOffset.z && gyroL.z > -rotateOffset.z)
			//      {
			//          gyroL.z = 0.0f;
			//      }

			//// 差分から現在のジョイコンの角度を算出
			//totalGyroR += gyroR;
			//totalGyroL += gyroL;

			//      // 左のジョイコンのジャイロ変数を上下左右の方向転換用の変数に軸を調整して格納
			//totalGyro = new Vector3(totalGyroL.x, totalGyroL.z, totalGyroL.y);

			//gyro = gyroL;

			//      // 左右のジョイコンのx軸の角度の差から右のジョイコンだけひねっているか（アクセル）を判定
			//      float offsetGyroX = totalGyroL.x + totalGyroR.x;
			//      if (offsetGyroX < -30.0f)
			//      {
			//          playerMove.Speed += (offsetGyroX * -1.0f * Time.deltaTime) +
			//              (playerMove.Data.Item_acceleration * playerMove.GetItems[(int)Item.ITEM_EFFECT.ACCELERATION - 1]);
			//          playerMove.IsAccel = true;
			//      }


			// ブレーキ
			if ((joyconR != null && joyconR.GetButton(Joycon.Button.SHOULDER_1)))
            {
                playerMove.IsBrake = true;
            }

            // 弾を発射
            if ((joyconR != null && joyconR.GetButtonUp(Joycon.Button.SHOULDER_2)))
            {
                Debug.Log("弾を撃ちます");
                playerMove.IsShot = true;
            }
			if ((joyconR != null && joyconR.GetButton(Joycon.Button.SHOULDER_2)))
			{
                Debug.Log("チャージしてます");
				playerMove.IsCharging = true;
			}

			// ジャイロリセット
			if ((joyconL != null && joyconL.GetButton(Joycon.Button.SHOULDER_2)))
			{
                ResetGyro();
			}

		}
	}

	private float GetNorm(float angle)
	{
		return Mathf.Repeat(angle + 180.0f, 360.0f) - 180.0f;
	}

    private Vector3 GetNorm(Vector3 vec)
    {
        return new Vector3(
            GetNorm(vec.x),
            GetNorm(vec.y),
            GetNorm(vec.z)
            );
    }

	/// <summary>
	/// ジャイロ関連をリセットする関数
	/// </summary>
	public override void ResetGyro()
	{
		// 最初のジョイコンの姿勢を取得
		startQuatL = joyconL.GetVector();
		startQuatL = new Quaternion(startQuatL.x, startQuatL.z, startQuatL.y, startQuatL.w);
		startQuatL = Quaternion.Inverse(startQuatL);

		startQuatR = joyconR.GetVector();
		startQuatR = new Quaternion(startQuatR.x, startQuatR.z, startQuatR.y, startQuatR.w);
		startQuatR = Quaternion.Inverse(startQuatR);

		totalGyroL = Vector3.zero;
        totalGyroR = Vector3.zero;
		totalGyro = Vector3.zero;
		transform.rotation = Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f);

	}
}
