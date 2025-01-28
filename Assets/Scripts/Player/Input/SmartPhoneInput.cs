using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SmartPhone : PlayerInput
{

    private Gyroscope _gyro;
    private Vector3 _defaultAttitude;

    private bool isAccel;

    [SerializeField] private Vector3 _attitudeOffset = Vector3.zero;    // ジャイロの誤差を無視するオフセット
    [SerializeField] private Vector3 _rotateOffset = Vector3.zero;    // ジャイロの誤差を無視するオフセット

    //デバッグ用
    [SerializeField] private TextMeshProUGUI _text1;
    [SerializeField] private Image _debugImage;

    // Start is called before the first frame update
    public new void Start()
    {
		//if (photonView.IsMine)
		{
		    base.Start();

            _gyro = Input.gyro;
            _gyro.enabled = true;
            Yaw_Pitch_Role_Sensitivity = playerMove.Data.SmartphoneSens;

            ResetGyro();
        }
    }

    // Update is called once per frame
    public new void Update()
    {
		//if (photonView.IsMine)
		{
		    base.Update();

            Vector3 attitude = _gyro.attitude.eulerAngles;

            attitude = new Vector3(attitude.x - _defaultAttitude.x, attitude.y - _defaultAttitude.y, attitude.z - _defaultAttitude.z);

            //微量の傾きは無視する
            if (MyAbs(attitude.x) < _attitudeOffset.x) attitude.x = 0.0f;
            if (MyAbs(attitude.y) < _attitudeOffset.y) attitude.y = 0.0f;
            if (MyAbs(attitude.z) < _attitudeOffset.z) attitude.z = 0.0f;

            //attitudeを-180～180に補正
            if (attitude.x > 180)
            {
                attitude.x -= 360;
            }
            else if (attitude.x < -180)
            {
                attitude.x += 360;
            }

            if (attitude.y > 180) {
                attitude.y -= 360;
            }
            else if (attitude.y < -180)
            {
                attitude.y += 360;
            }

            if (attitude.z > 180)
            {
                attitude.z -= 360;
            }
            else if (attitude.z < -180)
            {
                attitude.z += 360;
            }

            //修正後のattitudeを格納
            TotalGyro = new Vector3(attitude.z, attitude.y, attitude.x);

            Vector3 rotationRate = new Vector3(_gyro.rotationRateUnbiased.x * Mathf.Rad2Deg, _gyro.rotationRateUnbiased.y * Mathf.Rad2Deg, _gyro.rotationRateUnbiased.z * Mathf.Rad2Deg);

            if (MyAbs(rotationRate.x) < _rotateOffset.x) rotationRate.x = 0.0f;
            if (MyAbs(rotationRate.y) < _rotateOffset.y) rotationRate.y = 0.0f;
            if (MyAbs(rotationRate.z) < _rotateOffset.z) rotationRate.z = 0.0f;

            //修正後のrotationRateを格納
            Gyro = rotationRate * Mathf.Deg2Rad;

            _text1.text = "Gyro" + Gyro + "TotalGyro" + TotalGyro + "DefaultAttitude.x" + _defaultAttitude.x + "Attitude" + _gyro.attitude + "EulerAttitude" + _gyro.attitude.eulerAngles;
            _debugImage.transform.position = new Vector3(TotalGyro.x + 960.0f, TotalGyro.y + 540.0f, 0.0f);
            //Debug.Log("Gyro" + Gyro + "TotalGyro" + TotalGyro + "DefaultAttitude.x" + _defaultAttitude.x + "Attitude" + _gyro.attitude + "EulerAttitude" + _gyro.attitude.eulerAngles);

            //各種ボタン判定
            if (isAccel)
            {
                float speed = 0.2f;
                speed = CheckCanBoost(speed);
                if (speed > 0.0f)
                {
                    playerMove.Speed += speed;
                }
            }
        }
    }

    /// <summary>
    /// ジャイロをリセットする関数
    /// </summary>
    public override void ResetGyro()
    {
        _defaultAttitude = _gyro.attitude.eulerAngles;
        totalGyro = Vector3.zero;
        transform.rotation = Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f);
    }

    //絶対値を返す
    public float MyAbs(float num)
    {
        if (num >= 0.0f) return num;
        else return -num;
    }

    //
    //アクセル＆ブレーキボタン関数
    //
    public void StartAccel()
    {
        playerMove.IsAccel = true;
        isAccel = true;
    }

    public void StopAccel()
    {
        playerMove.IsAccel = false;
        isAccel = false;
    }

    public void StartBrake()
    {
        playerMove.IsBrake = true;
    }

    public void StopBrake()
    {
        playerMove.IsBrake = false;
    }
}
