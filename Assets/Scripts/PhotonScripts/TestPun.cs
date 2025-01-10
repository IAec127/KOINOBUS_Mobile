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




    [SerializeField,Header("�J�ڐ�̃V�[��"),Tooltip("string")]
    private string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();
    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);

    }

    public override void OnJoinedRoom()
    {
        Vector3 position = new Vector3(Random.Range(minSpwanArea, maxSpwanArea), 10.0f, Random.Range(minSpwanArea, maxSpwanArea));
        player = PhotonNetwork.Instantiate(CreateObjectName, spwanPos+position, Quaternion.identity);

        CMVcamera.Follow = player.transform;
        CMVcamera.LookAt = player.transform;

        // �C�x���g�Ǘ��p�N���X�Ƀ��[�J���v���C���[��n��
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
