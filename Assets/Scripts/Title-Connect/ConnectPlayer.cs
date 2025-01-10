using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectPlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    //���������蓖�Ă��Ă���PL��number���i�[����@��������Manager���Ŋ��蓖�Ă�
    public int playerListNumber;
    public bool readyFlag = false;

    private TItleManager titleM;

    void Start()
    {
        titleM = GameObject.Find("TitleManager").GetComponent<TItleManager>();

        //���g���V���O���g���̃��X�g�̎q�ɂ���
        this.gameObject.transform.SetParent(ConnectPlayerList.instance.gameObject.transform);

        //�V���O���g���̃��X�g�Ɏ��g��ǉ�
        ConnectPlayerList.instance.cpList.Add(this.gameObject);

        //���X�g�̗v�f���𐔂��A�v���C���[�̐l���Ƃ���
        playerListNumber = ConnectPlayerList.instance.cpList.Count;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (photonView.IsMine)
            {
                if (!readyFlag)
                {
                    readyFlag = true;
                }
                else
                {
                    readyFlag = false;
                }
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //���̃v���C���[�����L���Ă����瑼�̃v���C���[�Ƀf�[�^�𑗐M���܂�
        if (stream.IsWriting)
        {
            stream.SendNext(readyFlag);
        }
        else
        {
            //�l�b�g���[�N�v���C���[�B�f�[�^����M����
            this.readyFlag = (bool)stream.ReceiveNext();
        }
    }

    public bool IsPlayerMine()
    {
        if(photonView.IsMine)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetReadyFlag()
    {
        return readyFlag;
    }
}
