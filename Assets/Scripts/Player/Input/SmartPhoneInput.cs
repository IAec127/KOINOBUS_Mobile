using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class SmartPhone : PlayerInput
{
    private Gyroscope _gyro;
    private Vector3 _defaultAttitude;

    [SerializeField] private Vector3 _attitudeOffset = Vector3.zero;    // ジャイロの誤差を無視するオフセット
    [SerializeField] private Vector3 _rotateOffset = Vector3.zero;    // ジャイロの誤差を無視するオフセット

    //デバッグ用
    [SerializeField] private TextMeshProUGUI text1;

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

            if (attitude.x > 180) attitude.x -= 360;
            if (attitude.y > 180) attitude.y -= 360;
            if (attitude.z > 180) attitude.z -= 360;

            //修正後のattitudeを格納
            TotalGyro = new Vector3(attitude.x, attitude.y, attitude.z);

            Vector3 rotationRate = new Vector3(_gyro.rotationRateUnbiased.x * Mathf.Rad2Deg, _gyro.rotationRateUnbiased.y * Mathf.Rad2Deg, _gyro.rotationRateUnbiased.z * Mathf.Rad2Deg);

            if (MyAbs(rotationRate.x) < _rotateOffset.x) rotationRate.x = 0.0f;
            if (MyAbs(rotationRate.y) < _rotateOffset.y) rotationRate.y = 0.0f;
            if (MyAbs(rotationRate.z) < _rotateOffset.z) rotationRate.z = 0.0f;

            //修正後のrotationRateを格納
            Gyro = rotationRate * Mathf.Deg2Rad;

            text1.text = "Gyro" + Gyro + "TotalGyro" + TotalGyro + "DefaultAttitude.x" + _defaultAttitude.x + "Attitude" + _gyro.attitude + "EulerAttitude" + _gyro.attitude.eulerAngles;
            //Debug.Log("Gyro" + Gyro + "TotalGyro" + TotalGyro + "DefaultAttitude.x" + _defaultAttitude.x + "Attitude" + _gyro.attitude + "EulerAttitude" + _gyro.attitude.eulerAngles);
		}

    }

    /// <summary>
    /// ジャイロをリセットする関数
    /// </summary>
    public override void ResetGyro()
    {
        _defaultAttitude = _gyro.attitude.eulerAngles;
    }

    //絶対値を返す
    public float MyAbs(float num)
    {
        if (num >= 0.0f) return num;
        else return -num;
    }
}
