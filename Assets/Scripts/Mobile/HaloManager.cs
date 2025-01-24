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
    //�J�E���g�A�b�v
    private float countUp = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMove = player.transform.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        count.text = (maxHaloNum - haloNum).ToString() + " / " + maxHaloNum.ToString();
        //���Ԃ��J�E���g�_�E������
        if (playerMove.GameStartCheck())
        {
            countUp++;
        }
        var countTime = countUp / 60.0f;
        time.text = countTime.ToString("f1");

        //�Q�[���I������
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
        // �C�x���g�ɓo�^
        SceneManager.sceneLoaded += GameSceneLoaded;

        SceneManager.LoadScene("MobileResult");
    }

    private void GameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �V�[���؂�ւ����ɌĂ΂��
        // �V�[���؂�ւ���̃X�N���v�g���擾
        var resultManager = GameObject.FindWithTag("ResultManager").GetComponent<ResultManager>();

        // �f�[�^��n������
        resultManager.timeScore = countUp;

        // �C�x���g����폜
        SceneManager.sceneLoaded -= GameSceneLoaded;
    }
}
