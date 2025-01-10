using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerEffect : MonoBehaviourPunCallbacks
{
	[SerializeField]
	PlayerMove player = null;
	[SerializeField]
	private Animator animator = null;

	//-----------------------------
	// エフェクト系
	//-----------------------------
	// イベント時のバフ用エフェクトリスト
	[SerializeField]
	private List<ParticleSystem> buffEffects;
	public List<ParticleSystem> BuffEffects
	{
		get { return buffEffects; }
		set { buffEffects = value; }
	}

	// 回転時のエフェクト
	[SerializeField]
	private ParticleSystem rotationEffect;
	public ParticleSystem RotationEffect
	{
		get { return rotationEffect; }
		set { rotationEffect = value; }
	}
	// ブースト時のエフェクト
	[SerializeField]
	private ParticleSystem boostEffect;
	public ParticleSystem BoostEffect
	{
		get { return boostEffect; }
		set { boostEffect = value; }
	}
	// ブースト時の集中線エフェクト
	public ParticleSystem AccelLineEffect { get; set; }
	// 被弾時の爆発エフェクト
	[SerializeField]
	private ParticleSystem explosionEffect;
	public ParticleSystem ExplosionEffect
	{
		get { return explosionEffect; }
		set { explosionEffect = value; }
	}
	// 被弾時の気絶エフェクト
	[SerializeField]
	private ParticleSystem stunEffect;
	public ParticleSystem StunEffect
	{
		get { return stunEffect; }
		set { stunEffect = value; }
	}


	// Start is called before the first frame update
	void Start()
    {
        
    }

	[PunRPC]
	public void StartBoost()
	{
		player.IsBoost = true;
		BoostEffect.gameObject.SetActive(true);
		BoostEffect.Play();
		if (photonView.IsMine)
		{
			AccelLineEffect.gameObject.SetActive(true);
			AccelLineEffect.Play();
		}
		animator.SetBool("IsBoost", true);
	}

	[PunRPC]
	public void EndBoost()
	{
		player.IsBoost = false;
		if(boostEffect.isPlaying)
		{
			BoostEffect.Stop();
			BoostEffect.gameObject.SetActive(false);
		}
		if (photonView.IsMine)
		{
			if(AccelLineEffect.isPlaying)
			{
				AccelLineEffect.Stop();
				AccelLineEffect.gameObject.SetActive(false);
			}
		}
		animator.SetBool("IsBoost", false);
	}

	[PunRPC]
	public void StartBrake()
	{
		animator.SetBool("IsBrake", true);
	}

	[PunRPC]
	public void EndBrake()
	{
		animator.SetBool("IsBrake",false);
	}

	[PunRPC]
	public void StartStun()
	{
		// 被弾時の爆発エフェクトを表示
		ExplosionEffect.Play();
		// 被弾中の気絶エフェクトを表示
		StunEffect.gameObject.SetActive(true);
		StunEffect.Play();
		// 被弾中の回転エフェクトを表示
		RotationEffect.gameObject.SetActive(true);
		RotationEffect.Play();
	}

	[PunRPC]
	public void EndStun()
	{
		// 被弾中の気絶エフェクトを非表示
		StunEffect.Stop();
		StunEffect.gameObject.SetActive(false);
		// 被弾中の回転エフェクトを非表示
		RotationEffect.Stop();
		RotationEffect.gameObject.SetActive(false);
	}


	[PunRPC]
	public void StartDead()
	{
		explosionEffect.Play();
	}

}
