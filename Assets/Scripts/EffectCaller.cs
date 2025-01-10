using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCaller : MonoBehaviour
{
    [Header("�G�t�F�N�g�̖��O����Ă�")]
    [SerializeField, Tooltip("Hierarchy���̂ɂȂ�܂�")] string effectName;
    [Header("�T�E���h�G�t�F�N�g�̐ݒ�")]
    public AudioClip clip;
    [SerializeField,Range(0.0f,1.0f)]float volume = 1.0f;
    [Header("�p�[�e�B�N���G�t�F�N�g�̐ݒ�")]
    public GameObject effect;
    [SerializeField,Tooltip("�ڂ���Ă�")] float length;
    [Space]
    [SerializeField] bool doloop;

    GameObject instanceEffect;
    float deleteTime;
    AudioSource source;
    public void SetSoundEffect(AudioClip se, float vol = 1.0f, bool loop = false)
    {
        source.clip = se;
        source.volume = vol;
        doloop = source.loop = loop;
        source.Play();
        deleteTime = se.length;
    }

    public void SetParticleEffect(GameObject prefab,float loopTime, bool loop = false)
    {
        instanceEffect =Instantiate(prefab, transform.position, transform.rotation);
        instanceEffect.name = effectName.Length > 0 ? effectName : "Effect";
        deleteTime = loopTime;
        doloop = loop;
    }

    public void SetEffects(AudioClip se, float vol = 1.0f, GameObject prefab = null, float loopTime = 0.0f, bool loop = false)
    {
        source.clip = se;
        source.volume = vol;
        doloop = source.loop = loop;
        source.Play();
        if (prefab != null)
        {
            instanceEffect = Instantiate(prefab, transform.position, transform.rotation);
            instanceEffect.name = effectName.Length > 0 ? effectName : "Effect";
        }
        deleteTime = loopTime > se.length ? loopTime : se.length;
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();

        if(clip != null && effect != null)
        {
            SetEffects(clip, volume, effect, length, doloop);
        }
        else if(clip != null)
        {
            SetParticleEffect(effect, length, doloop);
        }
        else if(effect != null)
        {
            SetSoundEffect(clip, volume, doloop);
        }
        gameObject.name = effectName.Length > 0 ? effectName : "Effect";
    }

    private void FixedUpdate()
    {
        if (doloop)
        {
            return;
        }

        if(deleteTime < 0f)
        {
            deleteTime -= Time.deltaTime;
        }
        else
        {
            Destroy(instanceEffect);
            Destroy(gameObject);
        }
    }

}
