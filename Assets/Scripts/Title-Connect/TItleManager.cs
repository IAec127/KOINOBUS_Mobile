using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System;
using System.Linq;


public class TItleManager : MonoBehaviourPunCallbacks
{
    RoomOptions roomOptions;

    [SerializeField,Header("参加人数の上限、上限に達したらルームの参加を締め切る")]
    private int maxParticipation = 4;

    //プレイヤーの準備完了かどうかを判別する 
    public uint readyCount;

    //すべてのプレイヤーが準備完了していた場合のフラグ
    public uint allReadyFlag;

    [SerializeField, Header("test用")]
    private Text hogeTxt;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        hogeTxt = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<Text>();

    }

    private void Awake()
    {
        //MasterClientに同期
        PhotonNetwork.AutomaticallySyncScene = true;

        roomOptions = new RoomOptions
        {
            MaxPlayers = maxParticipation,         //参加人数の上限なし
            IsVisible = false,      //リストに公開しない　->　ランダムに参加されない
            IsOpen = true,          //ルームに参加できるようにする
            PublishUserId = true,   //参加ユーザーのIDを公開する
        };

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(ConnectPlayerList.instance.cpList.All(obj => obj.GetComponent<ConnectPlayer>().GetReadyFlag()))
            {
                //LoadScene();
                photonView.RPC(nameof(TestHoge), RpcTarget.All);
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("COINOBUS", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        GameObject player = PhotonNetwork.Instantiate("ConnectPlayer", Vector3.zero, Quaternion.identity);

        //参加人数が上限を超えたら参加を締め切る
        if (PhotonNetwork.CountOfPlayersInRooms == roomOptions.MaxPlayers && roomOptions.IsOpen == true)
        {
            Debug.Log("<size=20><color=#ff0000ff>参加人数:" + PhotonNetwork.CountOfPlayersInRooms + "が上限人数:" + roomOptions.MaxPlayers + "を超えた為、参加を締め切ります</color></size>");
            roomOptions.IsOpen = false;
        }
    }

    /// <summary>
    /// ルームから退出した時に呼ばれる
    /// </summary>
    public override void OnLeftRoom()
    {

        //GameObject obj = MyPlayerFetch(ConnectPlayerList.instance.cpList);
        GameObject myObj = ConnectPlayerList.instance.cpList.Single(obj => obj.GetComponent<ConnectPlayer>().IsPlayerMine() == true);

        if (myObj != null)
        {
            ConnectPlayerList.instance.cpList.Remove(myObj);
        }

        // 参加が締め切られていた場合
        if (!roomOptions.IsOpen)
        {
            //ルームに参加できるようにする 
            roomOptions.IsOpen = true;
            Debug.Log("<size=20><color=#ff0000ff>参加人数:" + PhotonNetwork.CountOfPlayersInRooms + "が上限人数:" + roomOptions.MaxPlayers + "を下回った為、締切を解除します</color></size>");
        }
    }

    [PunRPC]
    public void LoadScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    [PunRPC]
    public void TestHoge()
    {
        hogeTxt.text = "全員OK";
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 16;

        GUI.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

        GUILayout.Label("State:" + PhotonNetwork.NetworkClientState.ToString());
        GUILayout.Label("Ping:" + PhotonNetwork.GetPing().ToString() + "ms");

        if (PhotonNetwork.InRoom)
        {
            GUILayout.Label("RoomName:" + PhotonNetwork.CurrentRoom.Name);
            GUILayout.Label("PlayerCount:" + PhotonNetwork.CurrentRoom.PlayerCount.ToString());
            GUILayout.Label("MasterClient:" + PhotonNetwork.IsMasterClient.ToString());
        }
    }
}