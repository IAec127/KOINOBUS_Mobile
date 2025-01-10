using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectPlayerList : MonoBehaviour
{
    public static ConnectPlayerList instance; // �C���X�^���X�̒�`

    [SerializeField]
    public List<GameObject> cpList;

    private void Awake()
    {
        // �V���O���g���̎���
        if (instance == null)
        {
            // ���g���C���X�^���X�Ƃ���
            instance = this;
        }
        else
        {
            // �C���X�^���X���������݂��Ȃ��悤�ɁA���ɑ��݂��Ă����玩�g����������
            Destroy(gameObject);
        }
    }
}
