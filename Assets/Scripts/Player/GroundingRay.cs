using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RAY_DIRECTION
{
	DOWN,
	UP,
	LEFT,
	RIGHT,
}

public class GroundingRay
{
	public bool isGrounding;        // 2�{��ray���ǂ�����������Ă��邩�ǂ����̃t���O
	public RAY_DIRECTION rayDirection;  // ray�̕���
	public RaycastHit frontHit;     // �O����ray�̏Փˏ��
	public RaycastHit backHit;      // �����ray�̏Փˏ��

	private Transform transform = null;

	private float rayPosOffset = 0.8f;  // �O��̃I�t�Z�b�g
	private float rayDistance = 3.0f;   // ray�̋���
	private float minHeight = 2.0f; // �n�ʂƂ̍ŏ������i������ɉ����������j

	private Vector3 frontOffset = Vector3.zero;
	private Vector3 backOffset = Vector3.zero;

	private Vector3 frontPos = Vector3.zero;
	private Vector3 backPos = Vector3.zero;
	// ���C�̖@��
	private Vector3 frontNormal = Vector3.zero;
	private Vector3 backNormal = Vector3.zero;



	public GroundingRay(Transform transform, RAY_DIRECTION dir, float offset, float dis, float minHeight)
	{
		isGrounding = false;
		rayDirection = dir;
		this.transform = transform;
		rayPosOffset = offset;
		rayDistance = dis;
		this.minHeight = minHeight;
	}

	public void CreateRay()
	{
		Vector3 bounds = transform.gameObject.GetComponent<BoxCollider>().bounds.extents;
		// �O��̃I�t�Z�b�g���Z�o
		frontOffset = transform.forward * (bounds.z * rayPosOffset);
		backOffset = transform.forward * (-bounds.z * rayPosOffset);

		// �I�t�Z�b�g���Z�b�g
		frontPos = transform.position + frontOffset;
		backPos = transform.position + backOffset;

		// ���C�̖@��
		switch (rayDirection)
		{
			case RAY_DIRECTION.DOWN:
				frontNormal = -transform.up;
				backNormal = -transform.up;
				break;
			case RAY_DIRECTION.UP:
				frontNormal = transform.up;
				backNormal = transform.up;
				break;
			case RAY_DIRECTION.LEFT:
				frontNormal = -transform.right;
				backNormal = -transform.right;
				break;
			case RAY_DIRECTION.RIGHT:
				frontNormal = transform.right;
				backNormal = transform.right;
				break;
			default:
				break;
		}

	}

	public void ShotRay()
	{
		// ���C���΂�
		bool frontHitSuccess = Physics.Raycast(frontPos, frontNormal, out frontHit, rayDistance,LayerMask.GetMask("HitPlayer"));
		bool backHitSuccess = Physics.Raycast(backPos, frontNormal, out backHit, rayDistance, LayerMask.GetMask("HitPlayer"));
		// �f�o�b�N�p�Ƀ��C��`��
		Debug.DrawRay(frontPos, frontNormal * rayDistance, Color.red);
		Debug.DrawRay(backPos, backNormal * rayDistance, Color.blue);

		//if (frontHitSuccess)
		//{
		//	frontNormal = frontHit.normal;
		//}

		//if (backHitSuccess)
		//{
		//	backNormal = backHit.normal;
		//}
		// �O��̃��C���ǂ�����Փ˂��Ă����Ȃ�
		if (frontHitSuccess && backHitSuccess)
		{
			isGrounding = true;
		}
		else
		{
			if (isGrounding)
			{
				isGrounding = false;
			}
		}
	}

	public void RotateFollowWall()
	{
		Vector3 frontHitNormal = frontHit.normal;
		Vector3 backHitNormal = backHit.normal;

		// 2�̖@���̕��ς��Ƃ�
		Vector3 averageNormal = (frontHitNormal + backHitNormal).normalized;

		//// �@���Ɋ�Â�����]�̊p�x�𐧌�
		//float tiltAngle = Vector3.Angle(transform.up, averageNormal);
		//if (tiltAngle > maxTiltAngle)
		//{
		//	// �ő��]�p�x�𒴂��Ȃ��悤�ɐ�������
		//	averageNormal = Vector3.Slerp(transform.up, averageNormal, maxTiltAngle / tiltAngle);
		//}

		// ��]��K�p
		Quaternion targetRotation = Quaternion.identity;
		switch (rayDirection)
		{
			case RAY_DIRECTION.DOWN:
				targetRotation = Quaternion.FromToRotation(transform.up, averageNormal) * transform.rotation;
				break;
			case RAY_DIRECTION.UP:
				targetRotation = Quaternion.FromToRotation(-transform.up, averageNormal) * transform.rotation;
				break;
			case RAY_DIRECTION.LEFT:
				targetRotation = Quaternion.FromToRotation(transform.right, averageNormal) * transform.rotation;
				break;
			case RAY_DIRECTION.RIGHT:
				targetRotation = Quaternion.FromToRotation(-transform.right, averageNormal) * transform.rotation;
				break;
			default:
				break;
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f);

	}

	public void KeepShortestRange()
	{
		// �n�ʂƂ̍ŏ�������ۂ����i�I�u�W�F�N�g�̏�����Ɋ�Â������̒����j
		// �O��̃q�b�g�|�C���g�̒��ԓ_���擾
		Vector3 averageHeight = Vector3.Lerp(frontHit.point, backHit.point, 0.5f);

		float distanceToGround = 0.0f;

		switch (rayDirection)
		{
			case RAY_DIRECTION.DOWN:
				// ���݂̒n�ʂ���̋������v�Z�i�I�u�W�F�N�g�̏�����ɑ΂��āj
				distanceToGround = Vector3.Dot(transform.up, transform.position - averageHeight);

				// �������ŏ�����(minHeight)�����������ꍇ�ɂ݈̂ʒu�𒲐�
				if (distanceToGround < minHeight)
				{
					Vector3 targetPosition = transform.position + transform.up * (minHeight - distanceToGround);
					transform.position = targetPosition;
				}
				break;
			case RAY_DIRECTION.UP:
				// ���݂̒n�ʂ���̋������v�Z�i�I�u�W�F�N�g�̉������ɑ΂��āj
				distanceToGround = Vector3.Dot(-transform.up, transform.position - averageHeight);

				// �������ŏ�����(minHeight)�����������ꍇ�ɂ݈̂ʒu�𒲐�
				if (distanceToGround < minHeight)
				{
					Vector3 targetPosition = transform.position + -transform.up * (minHeight - distanceToGround);
					transform.position = targetPosition;
				}
				break;
			case RAY_DIRECTION.LEFT:
				// ���݂̒n�ʂ���̋������v�Z�i�I�u�W�F�N�g�̉������ɑ΂��āj
				distanceToGround = Vector3.Dot(transform.right, transform.position - averageHeight);

				// �������ŏ�����(minHeight)�����������ꍇ�ɂ݈̂ʒu�𒲐�
				if (distanceToGround < minHeight)
				{
					Vector3 targetPosition = transform.position + transform.right * (minHeight - distanceToGround);
					transform.position = targetPosition;
				}
				break;
			case RAY_DIRECTION.RIGHT:
				// ���݂̒n�ʂ���̋������v�Z�i�I�u�W�F�N�g�̉������ɑ΂��āj
				distanceToGround = Vector3.Dot(-transform.right, transform.position - averageHeight);

				// �������ŏ�����(minHeight)�����������ꍇ�ɂ݈̂ʒu�𒲐�
				if (distanceToGround < minHeight)
				{
					Vector3 targetPosition = transform.position + -transform.right * (minHeight - distanceToGround);
					transform.position = targetPosition;
				}
				break;
			default:
				break;
		}
	}
}
