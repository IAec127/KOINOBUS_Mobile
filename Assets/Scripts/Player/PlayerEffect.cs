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
	// �G�t�F�N�g�n
	//-----------------------------
	// �C�x���g���̃o�t�p�G�t�F�N�g���X�g
	[SerializeField]
	private List<ParticleSystem> buffEffects;
	public List<ParticleSystem> BuffEffects
	{
		get { return buffEffects; }
		set { buffEffects = value; }
	}

	// ��]���̃G�t�F�N�g
	[SerializeField]
	private ParticleSystem rotationEffect;
	public ParticleSystem RotationEffect
	{
		get { return rotationEffect; }
		set { rotationEffect = value; }
	}
	// �u�[�X�g���̃G�t�F�N�g
	[SerializeField]
	private ParticleSystem boostEffect;
	public ParticleSystem BoostEffect
	{
		get { return boostEffect; }
		set { boostEffect = value; }
	}
	// �u�[�X�g���̏W�����G�t�F�N�g
	public ParticleSystem AccelLineEffect { get; set; }
	// ��e���̔����G�t�F�N�g
	[SerializeField]
	private ParticleSystem explosionEffect;
	public ParticleSystem ExplosionEffect
	{
		get { return explosionEffect; }
		set { explosionEffect = value; }
	}
	// ��e���̋C��G�t�F�N�g
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
		// ��e���̔����G�t�F�N�g��\��
		ExplosionEffect.Play();
		// ��e���̋C��G�t�F�N�g��\��
		StunEffect.gameObject.SetActive(true);
		StunEffect.Play();
		// ��e���̉�]�G�t�F�N�g��\��
		RotationEffect.gameObject.SetActive(true);
		RotationEffect.Play();
	}

	[PunRPC]
	public void EndStun()
	{
		// ��e���̋C��G�t�F�N�g���\��
		StunEffect.Stop();
		StunEffect.gameObject.SetActive(false);
		// ��e���̉�]�G�t�F�N�g���\��
		RotationEffect.Stop();
		RotationEffect.gameObject.SetActive(false);
	}


	[PunRPC]
	public void StartDead()
	{
		explosionEffect.Play();
	}

}
