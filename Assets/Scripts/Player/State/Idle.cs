using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.Animations;
//using UnityEditor.Rendering;
//using UnityEditor.XR;
using UnityEngine;
using UnityEngine.Windows;

public class Idle : IState
{
	private CinemachineVirtualCamera vCamera = null;
	private CinemachineComposer vCameraCompoer;
	private Coroutine rollControlCoroutine = null;
	private bool beforeBrakeFlag = false;
	private float boostTime = 0.0f;
	private float boostCooldown = 0.0f;
	private int test;

	public Idle(PlayerMove playerMove) : base(playerMove) 
	{
		vCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
		vCameraCompoer = vCamera.GetCinemachineComponent<CinemachineComposer>();
		state = PLAYER_STATE.IDLE;
	}
	public override void Enter()
	{
	}

	public override void Exit()
	{
	}

	public override void Update()
	{
		// �N���b�`(����)
		if (player.IsAccel || player.EventType == EventManager.EVENT_TYPE.TAIL_WIND)
		{
			vCamera.m_Lens.FieldOfView += player.CameraData.AddFov;
			if (vCamera.m_Lens.FieldOfView > player.CameraData.MaxFov)
			{
				vCamera.m_Lens.FieldOfView = player.CameraData.MaxFov;
			}
		}
		// FOV�����Ƃɖ߂�
		else if (vCamera.m_Lens.FieldOfView > player.CameraData.MinFov && player.EventType != EventManager.EVENT_TYPE.TAIL_WIND)
		{
			vCamera.m_Lens.FieldOfView += -player.CameraData.SubFov;
			if (vCamera.m_Lens.FieldOfView <= player.CameraData.MinFov)
			{
				vCamera.m_Lens.FieldOfView = player.CameraData.MinFov;
			}
		}

		if (!player.IsBoost && player.IsAccel && player.EventType != EventManager.EVENT_TYPE.TAIL_WIND)
		{
			player.Effect.StartBoost();
		}
		if (player.IsBoost && !player.IsAccel && player.EventType != EventManager.EVENT_TYPE.TAIL_WIND)
		{
			player.Effect.EndBoost();
		}

		if (player.IsBrake)
		{
			if (!beforeBrakeFlag)
			{
				player.Effect.StartBrake();
			}
		}
		else if (!player.IsBrake && beforeBrakeFlag)
		{
			player.Effect.EndBrake();
		}
		beforeBrakeFlag = player.IsBrake;
		//player.IsBrake = false;

		// �Œ�E�ō����x�`�F�b�N
		if (player.Speed < player.Data.MinSpeed)
		{
			player.Speed = player.Data.MinSpeed;
		}
		if (player.Speed > player.MaxSpeed)
		{
			player.Speed = player.MaxSpeed;
		}

		player.BulletSpawner.BulletUpdate();

		player.IsShot = false;
		player.IsCharging = false;
	}


	public override void FixedUpdate()
	{
		var vec = (player.transform.forward * player.Speed) - rigidbody.linearVelocity;
		// �ǂɒǏ]���Ă����ԂłȂ��Ȃ�
		if(!player.IsFollowWall)
		{
			// �C���C�x���g���Ȃ�C���ɉ������͂�������
			vec += player.burstVec;
		}

		// �͂�������
		rigidbody.AddForce(vec, ForceMode.Force);

		if(!player.IsAccel)
		{
			player.Speed -= player.Data.SpeedDecay * Time.fixedDeltaTime;
		}

		// Debug.Log(rigidbody.velocity.magnitude);
		
		// ���E�l�`�F�b�N
		if (rigidbody.linearVelocity.magnitude > player.MaxSpeed)
		{
			rigidbody.linearVelocity = rigidbody.linearVelocity.normalized * player.MaxSpeed;
		}
		else if (rigidbody.linearVelocity.magnitude < player.Data.MinSpeed)
		{
			rigidbody.linearVelocity = rigidbody.linearVelocity.normalized * player.Data.MinSpeed;
		}

		if (player.IsBrake)
		{
			// �ǂ����C�x���g���͑��x�������Ȃ�
			if (player.EventType != EventManager.EVENT_TYPE.TAIL_WIND)
			{
				player.Speed -= player.Data.BrakePower;
				if (player.Speed < 0.0f)
				{
					player.Speed = 0.0f;
				}
			}
		}

		float minAngle = -89.0f;
		float maxAngle = 89.0f;
		player.IsRotate = false;

		// �A�^�b�`����Ă�����̓X�N���v�g�̐�������]���͂��`�F�b�N
		foreach (PlayerInput input in player.Inputs)
		{
			// �e���͎��̊��x���擾(�A�C�e���ƃC�x���g�ł̊��x�̕ω����܂�)
			Vector3 sens = input.Yaw_Pitch_Role_Sensitivity +
				(player.Data.Item_rotation * player.GetItems[(int)Item.ITEM_EFFECT.ROTATION - 1]) + player.tornadoSens;

			// �ǂ����x�����X���Ă���Ȃ�㏸�E���~
			if (input.TotalGyro.y > 0.0f || input.TotalGyro.y < 0.0f)
			{
				// ��]�ʂ��Z�o
				float angle = -input.TotalGyro.y * sens.x * Time.fixedDeltaTime;
				float beforeAngle = GetNorm(player.transform.eulerAngles.x );
				player.transform.Rotate(Vector3.right, angle, Space.Self);
				// -180����180�ɐ��K��
				float normAngle = GetNorm(player.transform.eulerAngles.x);
				// ���E�l�`�F�b�N(���łɌ��E�l�̏ꍇ�͖���)
				if ((normAngle < minAngle && beforeAngle > minAngle) || (normAngle > maxAngle && beforeAngle < maxAngle))
				{
					player.transform.Rotate(Vector3.right, -angle, Space.Self);
				}
			}

			bool beforeRotate = player.IsRotate;
			// �ǂ����z�����X���Ă���Ȃ獶�E�ړ�
			if (input.TotalGyro.z > 0.0f || input.TotalGyro.z < 0.0f)
			{
				player.IsRotate = true;
				float angleY = input.TotalGyro.z * sens.y * Time.fixedDeltaTime;
				player.transform.Rotate(Vector3.up, angleY, Space.World);
				// ��]�ʂ��Z�o
				float angle = -input.TotalGyro.z * sens.z * Time.fixedDeltaTime;
				player.transform.Rotate(Vector3.forward, angle, Space.Self);
				// -180����180�ɐ��K��
				float normAngle = GetNorm(player.transform.eulerAngles.z);
				// ���E�l�`�F�b�N
				if (normAngle < -45.0f || normAngle > 45.0f)
				{
					player.transform.Rotate(Vector3.forward, -angle, Space.Self);
				}
			}

			//// �ǂ����x�����X���Ă���Ȃ�㏸�E���~
			//if (input.TotalGyro.x > 45.0f || input.TotalGyro.x < -45.0f || input.Gyro.x != 0.0f)
			//{
			//	// ���[����]�𐧌䂷��R���[�`�������Ă��邩�`�F�b�N
			//	if (rollControlCoroutine != null)
			//	{
			//		// ���Ă����Ȃ�R���[�`�����I��
			//		player.StopCoroutine(rollControlCoroutine);
			//		rollControlCoroutine = null;
			//	}
			//	// ��]�ʂ��Z�o
			//	float angle = input.TotalGyro.x * (-1.0f) * sens.x * Time.fixedDeltaTime;
			//	float beforeAngle = Mathf.Repeat(player.transform.eulerAngles.x + 180.0f, 360.0f) - 180.0f;
			//	player.transform.Rotate(Vector3.right, angle, Space.Self);
			//	// -180����180�ɐ��K��
			//	float normAngle = Mathf.Repeat(player.transform.eulerAngles.x + 180.0f, 360.0f) - 180.0f;
			//	// ���E�l�`�F�b�N(���łɌ��E�l�̏ꍇ�͖���)
			//	if ((normAngle < minAngle && beforeAngle > minAngle) || (normAngle > maxAngle && beforeAngle < maxAngle))
			//	{
			//		player.transform.Rotate(Vector3.right, -angle, Space.Self);
			//	}
			//}

			//bool beforeRotate = player.IsRotate;
			//// �ǂ����z�����X���Ă���Ȃ獶�E�ړ�
			//if (input.TotalGyro.y > 45.0f || input.TotalGyro.y < -45.0f || input.Gyro.y != 0.0f)
			//{
			//	// ���[����]�𐧌䂷��R���[�`�������Ă��邩�`�F�b�N
			//	if (rollControlCoroutine != null)
			//	{
			//		// ���Ă����Ȃ�R���[�`�����I��
			//		player.StopCoroutine(rollControlCoroutine);
			//		rollControlCoroutine = null;
			//	}
			//	player.IsRotate = true;
			//	float angleY = input.TotalGyro.y * (-1.0f) * sens.y * Time.fixedDeltaTime;
			//	// ��]���n�߂��t���[���Ȃ��]���Ă��鎞�Ԃ̃J�E���g���n�߂�
			//	if (!beforeRotate) 
			//	{
			//	}
			//	else
			//	{
			//		// ���Ԍo�߂ɍ��킹�ăJ�����̃I�t�Z�b�g��ς���
			//		Vector3 offset = vCameraCompoer.m_TrackedObjectOffset;
			//		if (angleY>=0.0f)
			//		{
			//			offset.x -= player.CameraData.TrackedOffset.x * (Time.fixedDeltaTime / player.CameraData.TrackedTime.x);
			//			offset.y += (player.CameraData.TrackedOffset.y - player.CameraData.StartTrackedOffset.y) * (Time.fixedDeltaTime / player.CameraData.TrackedTime.y);
			//		}
			//		else
			//		{
			//			offset.x += player.CameraData.TrackedOffset.x * (Time.fixedDeltaTime / player.CameraData.TrackedTime.x);
			//			offset.y += (player.CameraData.TrackedOffset.y - player.CameraData.StartTrackedOffset.y) * (Time.fixedDeltaTime / player.CameraData.TrackedTime.y);
			//		}
			//		if(offset.x<-player.CameraData.TrackedOffset.x)
			//		{
			//			offset.x = -player.CameraData.TrackedOffset.x;
			//		}
			//		if(offset.x > player.CameraData.TrackedOffset.x)
			//		{
			//			offset.x = player.CameraData.TrackedOffset.x;
			//		}
			//		if(offset.y > player.CameraData.TrackedOffset.y)
			//		{
			//			offset.y = player.CameraData.TrackedOffset.y;
			//		}
			//		vCameraCompoer.m_TrackedObjectOffset = offset;
			//	}
			//	player.transform.Rotate(Vector3.up, input.TotalGyro.y * sens.y * Time.fixedDeltaTime, Space.World);
			//	// ��]�ʂ��Z�o
			//	float angle = input.TotalGyro.y * (-1.0f) * sens.z * Time.fixedDeltaTime;
			//	player.transform.Rotate(Vector3.forward, angle, Space.Self);
			//	// -180����180�ɐ��K��
			//	float normAngle = Mathf.Repeat(player.transform.eulerAngles.z + 180.0f, 360.0f) - 180.0f;
			//	// ���E�l�`�F�b�N
			//	if (normAngle < -45.0f || normAngle > 45.0f)
			//	{
			//		player.transform.Rotate(Vector3.forward, -angle, Space.Self);
			//	}
			//}
			//else
			//{
			//	player.IsRotate = false;
			//	if (vCameraCompoer.m_TrackedObjectOffset != player.CameraData.StartTrackedOffset)
			//	{
			//		Vector3 offset = vCameraCompoer.m_TrackedObjectOffset;
			//		if (vCameraCompoer.m_TrackedObjectOffset.x >= 0.0f)
			//		{
			//			offset.x -= player.CameraData.TrackedOffset.x * (Time.fixedDeltaTime / player.CameraData.ReturnTime.x);
			//			if (offset.x < player.CameraData.StartTrackedOffset.x)
			//			{
			//				offset.x = player.CameraData.StartTrackedOffset.x;
			//			}
			//		}
			//		else
			//		{
			//			offset.x += player.CameraData.TrackedOffset.x * (Time.fixedDeltaTime / player.CameraData.ReturnTime.x);
			//			if (offset.x > player.CameraData.StartTrackedOffset.x)
			//			{
			//				offset.x = player.CameraData.StartTrackedOffset.x;
			//			}
			//		}
			//		offset.y -= (player.CameraData.TrackedOffset.y - player.CameraData.StartTrackedOffset.y) * (Time.fixedDeltaTime / player.CameraData.ReturnTime.y);
			//		if (offset.y < player.CameraData.StartTrackedOffset.y)
			//		{
			//			offset.y = player.CameraData.StartTrackedOffset.y;
			//		}
			//		vCameraCompoer.m_TrackedObjectOffset = offset;
			//	}
			//}
        }
		if(!player.IsRotate)
		{
			ReturnRoll();
		}
		//if (!player.IsRotate && (player.transform.eulerAngles.z > 0.001f || player.transform.eulerAngles.z < -0.001f) && rollControlCoroutine == null)
		//{
		//	if (player.IsFollowWall)
		//	{
		//		return;
		//	}
		//	rollControlCoroutine = player.StartCoroutine(RollAttitudeControl());
		//}
	}


	/// <summary>
	/// ���[���̎p����߂��R���[�`��
	/// </summary>
	/// <returns></returns>
	public IEnumerator RollAttitudeControl()
	{
		// ���݂̃��[�������̌X���Ɛ��펞�̌X������A�p���𒼂��N�H�[�^�j�I�����쐬
		Quaternion fromQuat = player.transform.rotation;
		Quaternion toQuat = Quaternion.Euler(player.transform.eulerAngles.x, player.transform.eulerAngles.y, 0.0f);
		float nowTime = 0.0f;
		while (true)
		{
			nowTime += Time.fixedDeltaTime;
			Quaternion rot = Quaternion.Slerp(fromQuat, toQuat, nowTime / player.Data.RollControllTime);
			player.transform.rotation = rot;
			if (nowTime / player.Data.RollControllTime >= 1.0f)
			{
				rollControlCoroutine = null;
				yield break;
			}
			yield return new WaitForFixedUpdate();
		}
	}

	private void ReturnRoll()
	{
		// ���݂̃��[�������̌X���Ɛ��펞�̌X������A�p���𒼂��N�H�[�^�j�I�����쐬
		Quaternion fromQuat = player.transform.rotation;
		Quaternion toQuat = Quaternion.Euler(player.transform.eulerAngles.x, player.transform.eulerAngles.y, 0.0f);
		Quaternion rot = Quaternion.Slerp(fromQuat, toQuat, player.Data.ReturnRollSpeed * Time.fixedDeltaTime);
		player.transform.rotation = rot;
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

}
