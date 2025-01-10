using Photon.Pun;
using System.Runtime.Serialization;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviourPunCallbacks
{
    public enum EVENT_TYPE
    {
        TAIL_WIND,      // 追い風
        HEAD_WIND,      // 向かい風
        UP_BURST,       // 上昇気流
        DOWN_BURST,     // 下降気流
        TORNADO,        // 竜巻
        CRAZY_WIND,     // 狂風
        NONE,
    }

    [SerializeField]
    private EventData data = null;
    private EVENT_TYPE eventType = EVENT_TYPE.NONE;
    private float startTime = 0.0f;     // イベントを開始した時間

	// ローカルプレイヤー
	private GameObject localPlayerObj;  
    public GameObject LocalPlayerObj
    {
        get { return localPlayerObj; }
        set { localPlayerObj = value; }
    }

    public PlayerUI playerUI { get; set; }

    //-------------------
    // デバック用
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
                // イベント開始
                CreateEvent((EVENT_TYPE)rand);
                startTime = 0;
            }
        }
		else
        {
            startTime += Time.deltaTime;
            if (startTime >= data.EventTime)
            {
                // イベント終了
                EndEvent(eventType);
                startTime = 0;
            }
        }
    }

    /// <summary>
    /// イベントを作成する関数
    /// </summary>
    /// <param name="type">作成するイベント</param>
    public void CreateEvent(EVENT_TYPE type)
    {
        if(type==EVENT_TYPE.NONE)
        {
            return;
        }
        // 現在イベントが実行されているかチェック
        if (eventType != EVENT_TYPE.NONE)
        {
            // イベントの終了処理
            EndEvent(eventType);
        }
        startTime = 0.0f;
		photonView.RPC(nameof(StartAnimation), RpcTarget.All,type);
        // 残り時間UIの色をイベントの色に
        playerUI.ChangeColor((int)type);
		// イベントを開始
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
        // 残り時間UIの色を戻す
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
        // 開始するイベントに応じたテキストに変更
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
        // UIをポップアップ
        eventAnim.SetTrigger("start");
        // 指定時間後に消す
        Invoke(nameof(EndAnim), data.PopupTime);
    }

    private void EndAnim()
    {
		var eventAnim = GameObject.FindGameObjectWithTag("EventUI").GetComponent<Animator>();
        eventAnim.SetTrigger("end");
	}

	#region 追い風

	/// <summary>
	/// 追い風
	/// </summary>
	[PunRPC]
    public void TailWind()
    {
        eventType = EVENT_TYPE.TAIL_WIND;
        var player=localPlayerObj.GetComponent<PlayerMove>();
        // プレイヤーにイベントの開始を通知
        player.EventType = eventType;
        // ローカルプレイヤーの最高速度上限を増やす
        player.EventSpeed = data.TailWindSpeed;
        // ローカルプレイヤーの速度を最高速度に
        player.Speed = player.MaxSpeed;
        // エフェクトを再生
        player.Effect.StartBoost();
		// ブーストのアニメーションを再生
		player.ModelObj.GetComponent<Animator>().SetTrigger("startBoost");
	}

	[PunRPC]
    public void EndTailWind()
    {
        eventType = EVENT_TYPE.NONE;
		var player = localPlayerObj.GetComponent<PlayerMove>();
        // プレイヤーにイベントの終了を通知
        player.EventType = eventType;
        player.EventSpeed = 0.0f;
		player.Speed = player.MaxSpeed;
		player.Effect.EndBoost();
		// ブーストのアニメーションを停止
		player.ModelObj.GetComponent<Animator>().SetTrigger("endBoost");
	}

	#endregion

	#region 向かい風

	/// <summary>
	/// 向かい風
	/// </summary>
	[PunRPC]
    public void HeadWind()
    {
		eventType = EVENT_TYPE.HEAD_WIND;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// プレイヤーにイベントの開始を通知
		player.EventType = eventType;
		// ローカルプレイヤーの最高速度上限を減らす
		player.EventSpeed = data.HeadWindSpeed;
		// ローカルプレイヤーの速度を最低速度に
		player.Speed = player.Data.MinSpeed;
	}

    [PunRPC]
    public void EndHeadWind()
    {
		eventType = EVENT_TYPE.NONE;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// プレイヤーにイベントの終了を通知
		player.EventType = eventType;
        player.EventSpeed = 0.0f;
	}

    #endregion

    #region 気流

    /// <summary>
    /// 上昇気流
    /// </summary>
    [PunRPC]
    public void UpBurst()
    {
		eventType = EVENT_TYPE.UP_BURST;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// プレイヤーにイベントの開始を通知
		player.EventType = eventType;
        // 上昇気流のベクトルをセット
        player.burstVec = data.UpBurstVec;
	}

    [PunRPC]
    public void DownBurst()
    {
		eventType = EVENT_TYPE.DOWN_BURST;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// プレイヤーにイベントの開始を通知
		player.EventType = eventType;
		// 上昇気流のベクトルをセット
		player.burstVec = data.DownBurstVec;
	}

	[PunRPC]
    public void EndBurst()
    {
		eventType = EVENT_TYPE.NONE;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// プレイヤーにイベントの終了を通知
		player.EventType = eventType;
        player.burstVec = Vector3.zero;
	}

    #endregion

    #region 竜巻

    /// <summary>
    /// 竜巻
    /// </summary>
    [PunRPC]
    public void Tornado()
    {
		eventType = EVENT_TYPE.TORNADO;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// プレイヤーにイベントの開始を通知
		player.EventType = eventType;
        player.tornadoSens = data.TornadoSens;
	}

    [PunRPC]
    public void EndTornado()
    {
		eventType = EVENT_TYPE.NONE;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// プレイヤーにイベントの終了を通知
		player.EventType = eventType;
		player.tornadoSens = Vector3.zero;
	}
	#endregion

	#region 狂風

	[PunRPC]
	public void CrazyWind()
	{
		eventType = EVENT_TYPE.CRAZY_WIND;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// プレイヤーにイベントの開始を通知
		player.EventType = eventType;

	}

	[PunRPC]
	public void EndCrazyWind()
	{
		eventType = EVENT_TYPE.NONE;
		var player = localPlayerObj.GetComponent<PlayerMove>();
		// プレイヤーにイベントの終了を通知
		player.EventType = eventType;
	}

	#endregion
}
