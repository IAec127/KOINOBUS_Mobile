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
	public bool isGrounding;        // 2本のrayがどちらも当たっているかどうかのフラグ
	public RAY_DIRECTION rayDirection;  // rayの方向
	public RaycastHit frontHit;     // 前方のrayの衝突情報
	public RaycastHit backHit;      // 後方のrayの衝突情報

	private Transform transform = null;

	private float rayPosOffset = 0.8f;  // 前後のオフセット
	private float rayDistance = 3.0f;   // rayの距離
	private float minHeight = 2.0f; // 地面との最小距離（上方向に沿った距離）

	private Vector3 frontOffset = Vector3.zero;
	private Vector3 backOffset = Vector3.zero;

	private Vector3 frontPos = Vector3.zero;
	private Vector3 backPos = Vector3.zero;
	// レイの法線
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
		// 前後のオフセットを算出
		frontOffset = transform.forward * (bounds.z * rayPosOffset);
		backOffset = transform.forward * (-bounds.z * rayPosOffset);

		// オフセットをセット
		frontPos = transform.position + frontOffset;
		backPos = transform.position + backOffset;

		// レイの法線
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
		// レイを飛ばす
		bool frontHitSuccess = Physics.Raycast(frontPos, frontNormal, out frontHit, rayDistance,LayerMask.GetMask("HitPlayer"));
		bool backHitSuccess = Physics.Raycast(backPos, frontNormal, out backHit, rayDistance, LayerMask.GetMask("HitPlayer"));
		// デバック用にレイを描画
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
		// 前後のレイがどちらも衝突していたなら
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

		// 2つの法線の平均をとる
		Vector3 averageNormal = (frontHitNormal + backHitNormal).normalized;

		//// 法線に基づいた回転の角度を制限
		//float tiltAngle = Vector3.Angle(transform.up, averageNormal);
		//if (tiltAngle > maxTiltAngle)
		//{
		//	// 最大回転角度を超えないように制限する
		//	averageNormal = Vector3.Slerp(transform.up, averageNormal, maxTiltAngle / tiltAngle);
		//}

		// 回転を適用
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
		// 地面との最小距離を保つ処理（オブジェクトの上方向に基づく高さの調整）
		// 前後のヒットポイントの中間点を取得
		Vector3 averageHeight = Vector3.Lerp(frontHit.point, backHit.point, 0.5f);

		float distanceToGround = 0.0f;

		switch (rayDirection)
		{
			case RAY_DIRECTION.DOWN:
				// 現在の地面からの距離を計算（オブジェクトの上方向に対して）
				distanceToGround = Vector3.Dot(transform.up, transform.position - averageHeight);

				// 距離が最小距離(minHeight)よりも小さい場合にのみ位置を調整
				if (distanceToGround < minHeight)
				{
					Vector3 targetPosition = transform.position + transform.up * (minHeight - distanceToGround);
					transform.position = targetPosition;
				}
				break;
			case RAY_DIRECTION.UP:
				// 現在の地面からの距離を計算（オブジェクトの下方向に対して）
				distanceToGround = Vector3.Dot(-transform.up, transform.position - averageHeight);

				// 距離が最小距離(minHeight)よりも小さい場合にのみ位置を調整
				if (distanceToGround < minHeight)
				{
					Vector3 targetPosition = transform.position + -transform.up * (minHeight - distanceToGround);
					transform.position = targetPosition;
				}
				break;
			case RAY_DIRECTION.LEFT:
				// 現在の地面からの距離を計算（オブジェクトの下方向に対して）
				distanceToGround = Vector3.Dot(transform.right, transform.position - averageHeight);

				// 距離が最小距離(minHeight)よりも小さい場合にのみ位置を調整
				if (distanceToGround < minHeight)
				{
					Vector3 targetPosition = transform.position + transform.right * (minHeight - distanceToGround);
					transform.position = targetPosition;
				}
				break;
			case RAY_DIRECTION.RIGHT:
				// 現在の地面からの距離を計算（オブジェクトの下方向に対して）
				distanceToGround = Vector3.Dot(-transform.right, transform.position - averageHeight);

				// 距離が最小距離(minHeight)よりも小さい場合にのみ位置を調整
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
