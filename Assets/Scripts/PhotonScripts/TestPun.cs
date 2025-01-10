using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class TestPun : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float minSpwanArea;

    [SerializeField]
    private float maxSpwanArea;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private CinemachineVirtualCamera CMVcamera;

    [SerializeField]
    private string CreateObjectName;

    [SerializeField]
    private Vector3 spwanPos;




    [SerializeField,Header("遷移先のシーン"),Tooltip("string")]
    private string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);

    }

    public override void OnJoinedRoom()
    {
        Vector3 position = new Vector3(Random.Range(minSpwanArea, maxSpwanArea), 10.0f, Random.Range(minSpwanArea, maxSpwanArea));
        player = PhotonNetwork.Instantiate(CreateObjectName, spwanPos+position, Quaternion.identity);

        CMVcamera.Follow = player.transform;
        CMVcamera.LookAt = player.transform;

        // イベント管理用クラスにローカルプレイヤーを渡す
        GameObject.FindGameObjectWithTag("EventManager").GetComponent<EventManager>().LocalPlayerObj = player;
	}

	//private void OnGUI()
 //   {
 //       GUI.skin.label.fontSize = 16;

 //       GUI.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

 //       GUILayout.Label("State:" + PhotonNetwork.NetworkClientState.ToString());
 //       GUILayout.Label("Ping:" + PhotonNetwork.GetPing().ToString() + "ms");

 //       if (PhotonNetwork.InRoom)
 //       {
 //           GUILayout.Label("RoomName:" + PhotonNetwork.CurrentRoom.Name);
 //           GUILayout.Label("PlayerCount:" + PhotonNetwork.CurrentRoom.PlayerCount.ToString());
 //           GUILayout.Label("MasterClient:" + PhotonNetwork.IsMasterClient.ToString());
 //       }
 //   }

}
