using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/CreateEnemyDataAsset")]
public class EnemyData : ScriptableObject
{
	// 敵の最大数
	[SerializeField, Tooltip("敵の最大数")]
	private int maxEnemy = 30;
	public int MaxEnemy
	{
		get { return maxEnemy; }
		set { maxEnemy = value; }
	}

	// 乱数で決める移動速度の最小値
	[SerializeField, Tooltip("乱数で決める移動速度の最小値")]
	private float minSpeed = 100;
	public float MinSpeed
	{
		get { return minSpeed; }
		set { minSpeed = value; }
	}

	// 乱数で決める移動速度の最大値
	[SerializeField, Tooltip("乱数で決める移動速度の最大値")]
	private float maxSpeed = 300;
	public float MaxSpeed
	{
		get { return maxSpeed; }
		set { maxSpeed = value; }
	}


	// 乱数で決める移動距離の最小値
	[SerializeField, Tooltip("乱数で決める移動距離の最小値")]
	private float minDistance = 2000;
	public float MinDistance
	{
		get { return minDistance; }
		set { minDistance = value; }
	}

	// 乱数で決める移動距離の最大値
	[SerializeField, Tooltip("乱数で決める移動距離の最大値")]
	private float maxDistance = 4000;
	public float MaxDistance
	{
		get { return maxDistance; }
		set { maxDistance = value; }
	}

	// 敵撃破時のスコア
	[SerializeField, Tooltip("敵撃破時のスコア")]
	private int enemyScore = 100;

	public int EnemyScore
	{
		get { return enemyScore; }
		set { enemyScore = value; }
	}

}
