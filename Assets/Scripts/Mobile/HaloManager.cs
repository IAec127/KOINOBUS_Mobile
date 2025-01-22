using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HaloManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private GameObject player;
    private PlayerMove playerMove;

    private int haloNum = 0;
    private int maxHaloNum = 0;
    private bool gameStart = false;
    //カウントアップ
    private float countUp = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMove = player.transform.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        count.text = haloNum.ToString() + " / " + maxHaloNum.ToString();
        //時間をカウントダウンする
        if (playerMove.GameStartCheck())
        {
            countUp++;
        }
        var countTime = (int)(countUp / 60.0f);
        time.text = countTime.ToString();

        //ゲーム終了判定
        if (gameStart && haloNum <= 0)
        {
            ChangeScene();
        }
    }

    public void Count()
    {
        if (gameStart == false) gameStart = true;
        haloNum++;
        maxHaloNum = haloNum;
    }

    public void Hit()
    {
        haloNum--;   
    }

    void ChangeScene()
    {
        // イベントに登録
        SceneManager.sceneLoaded += GameSceneLoaded;

        SceneManager.LoadScene("MobileResult");
    }

    private void GameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーン切り替え時に呼ばれる
        // シーン切り替え後のスクリプトを取得
        var resultManager = GameObject.FindWithTag("ResultManager").GetComponent<ResultManager>();

        // データを渡す処理
        resultManager.timeScore = countUp;

        // イベントから削除
        SceneManager.sceneLoaded -= GameSceneLoaded;
    }
}
