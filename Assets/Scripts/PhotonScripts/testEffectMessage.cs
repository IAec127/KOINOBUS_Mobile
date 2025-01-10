using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testEffectMessage : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        print("パーティクルの再生が終了したよ！");
        Destroy(this.gameObject);
    }
}
