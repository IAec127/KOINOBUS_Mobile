using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public float timeScore;   //�f�[�^�󂯓n���p

    [SerializeField] private TextMeshProUGUI timeScoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var time = timeScore / 60.0f;
        timeScoreText.text = time.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
