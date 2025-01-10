using Photon.Pun;
using System.Runtime.Serialization;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviourPunCallbacks
{
    public enum EVENT_TYPE
    {
        TAIL_WIND,      // �ǂ���
        HEAD_WIND,      // ��������
        UP_BURST,       // �㏸�C��
        DOWN_BURST,     // ���~�C��
        TORNADO,        // ����
        CRAZY_WIND,     // ����
        NONE,
    }

    [SerializeField]
    private EventData data = null;
    private EVENT_TYPE eventType = EVENT_TYPE.NONE;
    private float startTime = 0.0f;     // �C�x���g���J�n��������

	// ���[�J���v���C���[
	private GameObject localPlayerObj;  
    public GameObject LocalPlayerObj
    {
        get { return localPlayerObj; }
        set { localPlayerObj = value; }
    }

    public PlayerUI playerUI { get; set; }

    //-------------------
    // �f�o�b�N�p
    //-------------------
    [SerializeField]
    bool tailWind = false;
	[SerializeField]
	bool headWind = false;
	[SerializeField]
	bool upBurst = false;
	[SerializeField]
	bool downBurst = false;
	[SerializeField]
	bool tornado = false;
	[SerializeField]
	bool crazyWind = false;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if (tailWind)
        {
            tailWind = false;
            CreateEvent(EVENT_TYPE.TAIL_WIND);
        }
        if(headWind)
        {
            headWind = false;
            CreateEvent(EVENT_TYPE .HEAD_WIND);
        }
        if (upBurst)
        {
            upBurst = false;
            CreateEvent (EVENT_TYPE .UP_BURST);
        }
        if(downBurst)
        {
            downBurst = false;
            CreateEvent(EVENT_TYPE.DOWN_BURST);
        }
        if(tornado)
        {
            tornado = false;
            CreateEvent(EVENT_TYPE.TORNADO);
        }
        if(crazyWind)
        {
            crazyWind = false;
            CreateEvent(EVENT_TYPE.CRAZY_WIND);
        }

        if(eventType==EVENT_TYPE.NONE)
        {
            startTime += Time.deltaTime;
            if (startTime >= data.CreateEventTime)
            {
                int rand = Random.Range(0, 6);
                // �C�x���g�J�n
                CreateEvent((EVENT_TYPE)rand);
                startTime = 0;
            }
        }
		else
        {
            startTime += Time.deltaTime;
            if (startTime >= data.EventTime)
            {
                // �C�x���g�I��
                EndEvent(eventType);
                startTime = 0;
            }
        }
    }

    /// <summary>
    /// �C�x���g���쐬����֐�
    /// </summary>
    /// <param name="type">�쐬����C�x���g</param>
    public void CreateEvent(EVENT_TYPE type)
    {
        if(type==EVENT_TYPE.NONE)
        {
            return;
        }
        // ���݃C�x���g�����s����Ă��邩�`�F�b�N
        if (eventType != EVENT_TYPE.NONE)
        {
            // �C�x���g�̏I������
            EndEvent(eventType);
        }
        startTime = 0.0f;
		photonView.RPC(nameof(StartAnimation), RpcTarget.All,type);
        // �c�莞��UI�̐F���C�x���g�̐F��
        playerUI.ChangeColor((int)type);
		// �C�x���g���J�n
		switch (type)
        {
            case EVENT_TYPE.TAIL_WIND:
                photonView.RPC(nameof(TailWind), RpcTarget.All);
                break;
            case EVENT_TYPE.HEAD_WIND:
				photonView.RPC(nameof(HeadWind), RpcTarget.All);
				break;
            case EVENT_TYPE.UP_BURST:
				photonView.RPC(nameof(UpBurst), RpcTarget.All);
				break;
            case EVENT_TYPE.DOWN_BURST:
				photonView.RPC(nameof(DownBurst), RpcTarget.All);
				break;
            case EVENT_TYPE.TORNADO:
				photonView.RPC(nameof(Tornado), RpcTarget.All);
				break;
            case EVENT_TYPE.CRAZY_WIND:
				photonView.RPC(nameof(CrazyWind), RpcTarget.All);
				break;
            default:
                break;
        }
    }

    public void EndEvent(EVENT_TYPE type)
    {
		if (type == EVENT_TYPE.NONE)
		{
			return;
		}
        // �c�莞��UI�̐F��߂�
        playerUI.ResetColor();
        switch (type)
        {
            case EVENT_TYPE.TAIL_WIND:
                photonView.RPC(nameof(EndTailWind), RpcTarget.All);
                break;
            case EVENT_TYPE.HEAD_WIND:
				photonView.RPC(nameof(EndHeadWind), RpcTarget.All);
				break;
            case EVENT_TYPE.UP_BURST:
				photonView.RPC(nameof(EndBurst), RpcTarget.All);
				break;
            case EVENT_TYPE.DOWN_BURST:
				photonView.RPC(nameof(EndBurst), RpcTarget.All);
				break;
            case EVENT_TYPE.TORNADO:
				photonView.RPC(nameof(EndTornado), RpcTarget.All);
				break;
            case EVENT_TYPE.CRAZY_WIND:
				photonView.RPC(nameof(EndCrazyWind), RpcTarget.All);
				break;
            default:
                break;
        }
    }

    [PunRPC]
    public void StartAnimation(EVENT_TYPE type)
    {
        var eventText=GameObject.FindGameObjectWithTag("EventText").GetComponent<Text>();
        // �J�n����C�x���g�ɉ������e�L�X�g�ɕύX
        switch (type)
        {
            case EVENT_TYPE.TAIL_WIND:
                eventText.text = data.TailWindText;
                break;
            case EVENT_TYPE.HEAD_WIND:
				eventText.text = data.HeadWindText;
				break;
            case EVENT_TYPE.UP_BURST:
				eventText.text = data.UpBurstText;
				break;
            case EVENT_TYPE.DOWN_BURST:
				eventText.text = data.DownBurstText;
				break;
            case EVENT_TYPE.TORNADO:
				eventText.text = data.TornadoText;
				break;
            case EVENT_TYPE.CRAZY_WIND:
				eventText.text = data.CrazyWindText;
				break;
            case EVENT_TYPE.NONE:
                break;
            default:
                break;
        }
        var eventAnim = GameObject.FindGameObjectWithTag("EventUI").GetComponent<Animator>();
        // UI���|�b�v�A�b�v
        eventAnim.SetTrigger("start");
        // �w�莞�Ԍ�ɏ���
        Invoke(nameof(EndAnim), data.PopupTime);
    }

    private void EndAnim()
    {
		var eventAnim = GameObject.FindGameObjectWithTag("EventUI").GetComponent<Animator>();
        eventAnim.SetTrigger("end");
	}

	#region �ǂ���

	/// <summary>
	/// �ǂ���
	/// </summary>
	[PunRPC]
    public void TailWind()
    {
        eventType = EVENT_TYPE.TAIL_WIND;
        var player=localPlayerObj.GetComponent<PlayerMove>();
        // �v���C���[�ɃC�x���g�̊J�n��ʒm
        player.EventType = eventType;
        // ���[�J���v���C���[�̍ō����x����𑝂₷
        player.EventSpeed = data.TailWindSpeed;
        // ���[�J���v���C���[�̑��x���ō����x��
        player.Speed = player.MaxSpeed;
        // �G�t�F�N�g���Đ�
        player.Effect.StartBoost();
		// �u�[�X�g�̃A�j���[�V�������Đ�
		player.ModelObj.GetComponent<Animator>().SetTrigger("startBoost");
	}

	[PunRPC]
    public void EndTailWind()
    {
        eventType = EVENT_TYPE.NONE;
		var player = localPlayerObj.GetComponent<PlayerMove>();
        // �v���C���[�ɃC�x���g�̏I����ʒm
        player.EventType = eventType;
        player.EventSpeed = 0.0f;
		player.Speed = player.MaxSpeed;
		player.Effect.EndBoost();
		// �u�[�X�g�̃A�j���[�V�������~
		player.ModelObj.GetComponent<Animator>().SetTrigger("endBoost");
	}

	#endregion

	#region ��������

	/// <summary>
	/// ��������
	/// </summary>
	[PunRPC]
    public void HeadWind()
    {
		eventType = EVENT_TYPE.HEAD_WIND;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// �v���C���[�ɃC�x���g�̊J�n��ʒm
		player.EventType = eventType;
		// ���[�J���v���C���[�̍ō����x��������炷
		player.EventSpeed = data.HeadWindSpeed;
		// ���[�J���v���C���[�̑��x���Œᑬ�x��
		player.Speed = player.Data.MinSpeed;
	}

    [PunRPC]
    public void EndHeadWind()
    {
		eventType = EVENT_TYPE.NONE;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// �v���C���[�ɃC�x���g�̏I����ʒm
		player.EventType = eventType;
        player.EventSpeed = 0.0f;
	}

    #endregion

    #region �C��

    /// <summary>
    /// �㏸�C��
    /// </summary>
    [PunRPC]
    public void UpBurst()
    {
		eventType = EVENT_TYPE.UP_BURST;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// �v���C���[�ɃC�x���g�̊J�n��ʒm
		player.EventType = eventType;
        // �㏸�C���̃x�N�g�����Z�b�g
        player.burstVec = data.UpBurstVec;
	}

    [PunRPC]
    public void DownBurst()
    {
		eventType = EVENT_TYPE.DOWN_BURST;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// �v���C���[�ɃC�x���g�̊J�n��ʒm
		player.EventType = eventType;
		// �㏸�C���̃x�N�g�����Z�b�g
		player.burstVec = data.DownBurstVec;
	}

	[PunRPC]
    public void EndBurst()
    {
		eventType = EVENT_TYPE.NONE;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// �v���C���[�ɃC�x���g�̏I����ʒm
		player.EventType = eventType;
        player.burstVec = Vector3.zero;
	}

    #endregion

    #region ����

    /// <summary>
    /// ����
    /// </summary>
    [PunRPC]
    public void Tornado()
    {
		eventType = EVENT_TYPE.TORNADO;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// �v���C���[�ɃC�x���g�̊J�n��ʒm
		player.EventType = eventType;
        player.tornadoSens = data.TornadoSens;
	}

    [PunRPC]
    public void EndTornado()
    {
		eventType = EVENT_TYPE.NONE;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// �v���C���[�ɃC�x���g�̏I����ʒm
		player.EventType = eventType;
		player.tornadoSens = Vector3.zero;
	}
	#endregion

	#region ����

	[PunRPC]
	public void CrazyWind()
	{
		eventType = EVENT_TYPE.CRAZY_WIND;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// �v���C���[�ɃC�x���g�̊J�n��ʒm
		player.EventType = eventType;

	}

	[PunRPC]
	public void EndCrazyWind()
	{
		eventType = EVENT_TYPE.NONE;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// �v���C���[�ɃC�x���g�̏I����ʒm
		player.EventType = eventType;
	}

	#endregion
}
