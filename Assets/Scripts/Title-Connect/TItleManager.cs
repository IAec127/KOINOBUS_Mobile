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

    [SerializeField,Header("�Q���l���̏���A����ɒB�����烋�[���̎Q������ߐ؂�")]
    private int maxParticipation = 4;

    //�v���C���[�̏����������ǂ����𔻕ʂ��� 
    public uint readyCount;

    //���ׂẴv���C���[�������������Ă����ꍇ�̃t���O
    public uint allReadyFlag;

    [SerializeField, Header("test�p")]
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
        //MasterClient�ɓ���
        PhotonNetwork.AutomaticallySyncScene = true;

        roomOptions = new RoomOptions
        {
            MaxPlayers = maxParticipation,         //�Q���l���̏���Ȃ�
            IsVisible = false,      //���X�g�Ɍ��J���Ȃ��@->�@�����_���ɎQ������Ȃ�
            IsOpen = true,          //���[���ɎQ���ł���悤�ɂ���
            PublishUserId = true,   //�Q�����[�U�[��ID�����J����
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

        //�Q���l��������𒴂�����Q������ߐ؂�
        if (PhotonNetwork.CountOfPlayersInRooms == roomOptions.MaxPlayers && roomOptions.IsOpen == true)
        {
            Debug.Log("<size=20><color=#ff0000ff>�Q���l��:" + PhotonNetwork.CountOfPlayersInRooms + "������l��:" + roomOptions.MaxPlayers + "�𒴂����ׁA�Q������ߐ؂�܂�</color></size>");
            roomOptions.IsOpen = false;
        }
    }

    /// <summary>
    /// ���[������ޏo�������ɌĂ΂��
    /// </summary>
    public override void OnLeftRoom()
    {

        //GameObject obj = MyPlayerFetch(ConnectPlayerList.instance.cpList);
        GameObject myObj = ConnectPlayerList.instance.cpList.Single(obj => obj.GetComponent<ConnectPlayer>().IsPlayerMine() == true);

        if (myObj != null)
        {
            ConnectPlayerList.instance.cpList.Remove(myObj);
        }

        // �Q�������ߐ؂��Ă����ꍇ
        if (!roomOptions.IsOpen)
        {
            //���[���ɎQ���ł���悤�ɂ��� 
            roomOptions.IsOpen = true;
            Debug.Log("<size=20><color=#ff0000ff>�Q���l��:" + PhotonNetwork.CountOfPlayersInRooms + "������l��:" + roomOptions.MaxPlayers + "����������ׁA���؂��������܂�</color></size>");
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
        hogeTxt.text = "�S��OK";
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