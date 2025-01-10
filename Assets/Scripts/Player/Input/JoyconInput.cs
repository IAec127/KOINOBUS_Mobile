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
    private Joycon joyconL;     // ���̃W���C�R��
    private Joycon joyconR;     // �E�̃W���C�R��

    private Vector3 gyroR = Vector3.zero;
    private Vector3 gyroL = Vector3.zero;


	[SerializeField]
    private Vector3 rotateOffset = Vector3.zero;    // �W���C���̌덷�𖳎�����I�t�Z�b�g

    [SerializeField]
    private Vector3 totalGyroR;     // �E�̃W���C���̌X���̍��v�l
    [SerializeField]
    private Vector3 totalGyroL;     // ���̃W���C���̌X���̍��v�l

    private Quaternion startQuatR = Quaternion.identity;
	private Quaternion startQuatL = Quaternion.identity;

    private Quaternion nowQuatR = Quaternion.identity;
	private Quaternion nowQuatL = Quaternion.identity;



	// Start is called before the first frame update
	private new void Start()
    {
		{
		    base.Start();
		    // �W���C�R�����擾
		    joyconList = JoyconManager.Instance.j;
		    if (joyconList == null || joyconList.Count == 0)
		    {
			    Debug.Log("joycon��������܂���");
		    }
		    else
		    {
			    // ���E�̃W���C�R����T��
			    joyconL = joyconList.Find(c => c.isLeft);
			    joyconR = joyconList.Find(c => !c.isLeft);

				// �ŏ��̃W���C�R���̎p�����擾
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
				// �W���C�R���̑O�t���[���Ƃ̊p�x�̍������擾
				//gyroR = joyconR.GetGyro();
				//gyroL = joyconL.GetGyro();

				// ���݂̎p�����擾

				nowQuatL = joyconL.GetVector();
				nowQuatL = new Quaternion(nowQuatL.x, nowQuatL.z, nowQuatL.y, nowQuatL.w);
				nowQuatL = Quaternion.Inverse(nowQuatL);

				nowQuatR = joyconR.GetVector();
				nowQuatR = new Quaternion(nowQuatR.x, nowQuatR.z, nowQuatR.y, nowQuatR.w);
				nowQuatR = Quaternion.Inverse(nowQuatR);
			}

			// ���݂̎p���̍����擾
			Quaternion deltaQuatL = Quaternion.Inverse(nowQuatL) * startQuatL;
			totalGyroL = GetNorm(deltaQuatL.eulerAngles);

			Quaternion deltaQuatR = Quaternion.Inverse(nowQuatR) * startQuatR;
			totalGyroR = GetNorm(deltaQuatR.eulerAngles);


			totalGyro = new Vector3(0.0f, -totalGyroL.y / playerMove.Data.MaxJoyconRot.x, totalGyroL.z / playerMove.Data.MaxJoyconRot.y);
			// �f�b�h�]�[������
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

			// ���E�̃W���C�R����x���̊p�x�̍�����E�̃W���C�R�������Ђ˂��Ă��邩�i�A�N�Z���j�𔻒�
			float offsetGyroX = totalGyroR.y + totalGyroL.y;
			if (offsetGyroX > playerMove.Data.AccelRot)
			{
				float speed = (offsetGyroX * playerMove.Data.AccelPower * Time.deltaTime) +
					(playerMove.Data.Item_acceleration * playerMove.GetItems[(int)Item.ITEM_EFFECT.ACCELERATION - 1]);
				// �u�[�X�g�c�ʂ��`�F�b�N
				speed = CheckCanBoost(speed);
				if (speed > 0.0f)
				{
					playerMove.Speed += speed;
					playerMove.IsAccel = true;	
				}
			}


			//      // �����̌X���͖���
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

			//// �������猻�݂̃W���C�R���̊p�x���Z�o
			//totalGyroR += gyroR;
			//totalGyroL += gyroL;

			//      // ���̃W���C�R���̃W���C���ϐ����㉺���E�̕����]���p�̕ϐ��Ɏ��𒲐����Ċi�[
			//totalGyro = new Vector3(totalGyroL.x, totalGyroL.z, totalGyroL.y);

			//gyro = gyroL;

			//      // ���E�̃W���C�R����x���̊p�x�̍�����E�̃W���C�R�������Ђ˂��Ă��邩�i�A�N�Z���j�𔻒�
			//      float offsetGyroX = totalGyroL.x + totalGyroR.x;
			//      if (offsetGyroX < -30.0f)
			//      {
			//          playerMove.Speed += (offsetGyroX * -1.0f * Time.deltaTime) +
			//              (playerMove.Data.Item_acceleration * playerMove.GetItems[(int)Item.ITEM_EFFECT.ACCELERATION - 1]);
			//          playerMove.IsAccel = true;
			//      }


			// �u���[�L
			if ((joyconR != null && joyconR.GetButton(Joycon.Button.SHOULDER_1)))
            {
                playerMove.IsBrake = true;
            }

            // �e�𔭎�
            if ((joyconR != null && joyconR.GetButtonUp(Joycon.Button.SHOULDER_2)))
            {
                Debug.Log("�e�������܂�");
                playerMove.IsShot = true;
            }
			if ((joyconR != null && joyconR.GetButton(Joycon.Button.SHOULDER_2)))
			{
                Debug.Log("�`���[�W���Ă܂�");
				playerMove.IsCharging = true;
			}

			// �W���C�����Z�b�g
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
	/// �W���C���֘A�����Z�b�g����֐�
	/// </summary>
	public override void ResetGyro()
	{
		// �ŏ��̃W���C�R���̎p�����擾
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
