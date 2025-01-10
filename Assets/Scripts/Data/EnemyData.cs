using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/CreateEnemyDataAsset")]
public class EnemyData : ScriptableObject
{
	// �G�̍ő吔
	[SerializeField, Tooltip("�G�̍ő吔")]
	private int maxEnemy = 30;
	public int MaxEnemy
	{
		get { return maxEnemy; }
		set { maxEnemy = value; }
	}

	// �����Ō��߂�ړ����x�̍ŏ��l
	[SerializeField, Tooltip("�����Ō��߂�ړ����x�̍ŏ��l")]
	private float minSpeed = 100;
	public float MinSpeed
	{
		get { return minSpeed; }
		set { minSpeed = value; }
	}

	// �����Ō��߂�ړ����x�̍ő�l
	[SerializeField, Tooltip("�����Ō��߂�ړ����x�̍ő�l")]
	private float maxSpeed = 300;
	public float MaxSpeed
	{
		get { return maxSpeed; }
		set { maxSpeed = value; }
	}


	// �����Ō��߂�ړ������̍ŏ��l
	[SerializeField, Tooltip("�����Ō��߂�ړ������̍ŏ��l")]
	private float minDistance = 2000;
	public float MinDistance
	{
		get { return minDistance; }
		set { minDistance = value; }
	}

	// �����Ō��߂�ړ������̍ő�l
	[SerializeField, Tooltip("�����Ō��߂�ړ������̍ő�l")]
	private float maxDistance = 4000;
	public float MaxDistance
	{
		get { return maxDistance; }
		set { maxDistance = value; }
	}

	// �G���j���̃X�R�A
	[SerializeField, Tooltip("�G���j���̃X�R�A")]
	private int enemyScore = 100;

	public int EnemyScore
	{
		get { return enemyScore; }
		set { enemyScore = value; }
	}

}
