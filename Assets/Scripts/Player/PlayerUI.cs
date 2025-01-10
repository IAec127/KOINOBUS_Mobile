using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField, Header("制限時間")]
    private float withdrawalCount = 300;

    [SerializeField]
    List<Color> colorList = new List<Color>();
    private Color startColor = Color.white;

    [SerializeField]
    private float limitTime;

    [SerializeField]
    private Text timeText;

    private int minute;

    private float seconds;

    //監視するプレイヤー
    private PlayerMove target;

    public bool timeLost;

    // Start is called before the first frame update
    void Start()
    {
        timeLost = false;
        limitTime = withdrawalCount;
        timeText = transform.Find("Time").GetComponent<Text>();
        startColor = timeText.color;
        //GameObject.FindWithTag("EventManager").GetComponent<EventManager>().playerUI = this;
        TimeViewSet(0.0f);
    }

    // Update is called once per frame
    void Update()
    {
		// まだルームに参加していない場合は更新しない
		if (!PhotonNetwork.InRoom) { return; }
		// まだゲームの開始時刻が設定されていない場合は更新しない
		if (!PhotonNetwork.CurrentRoom.TryGetStartTime(out int timestamp)) { return; }
        // まだゲームが始まっていないならカウントしない
        if(!SceneController.instance.IsStart) { return; }

		float elapsedTime = Mathf.Max(0f, unchecked(PhotonNetwork.ServerTimestamp - timestamp) / 1000f);

        if ((limitTime - elapsedTime) <= 0f)
        {
            timeLost = true;
        }

		TimeViewSet(elapsedTime);
    }

    public void SetTarget(PlayerMove pTarget)
    {
        target = pTarget;
    }

    /// <summary>
    /// limitTime(残り時間)を分と秒に分けてUIのテキストに表示する
    /// </summary>
    private void TimeViewSet(float elapsedTime)
    {
        float diffTime = limitTime - elapsedTime;
        minute = (int)diffTime / 60;
        seconds = diffTime - minute * 60;
        timeText.text = minute.ToString("00") + ":" + ((int)seconds).ToString("00");

    }

    public void ChangeColor(int index)
    {
        timeText.color = colorList[index];
    }

    public void ResetColor()
    {
        timeText.color = startColor;
	}
}
