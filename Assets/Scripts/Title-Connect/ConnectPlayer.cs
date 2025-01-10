using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ConnectPlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    //自分が割り当てられているPLのnumberを格納する　生成時にManager側で割り当てる
    public int playerListNumber;
    public bool readyFlag = false;

    private TItleManager titleM;

    void Start()
    {
        titleM = GameObject.Find("TitleManager").GetComponent<TItleManager>();

        //自身をシングルトンのリストの子にする
        this.gameObject.transform.SetParent(ConnectPlayerList.instance.gameObject.transform);

        //シングルトンのリストに自身を追加
        ConnectPlayerList.instance.cpList.Add(this.gameObject);

        //リストの要素数を数え、プレイヤーの人数とする
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
        //このプレイヤーを所有していたら他のプレイヤーにデータを送信します
        if (stream.IsWriting)
        {
            stream.SendNext(readyFlag);
        }
        else
        {
            //ネットワークプレイヤー。データを受信する
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
