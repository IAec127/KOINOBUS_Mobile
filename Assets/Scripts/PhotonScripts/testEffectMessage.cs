using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testEffectMessage : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        print("�p�[�e�B�N���̍Đ����I��������I");
        Destroy(this.gameObject);
    }
}
