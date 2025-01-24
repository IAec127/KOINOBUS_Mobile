using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public float timeScore;   //データ受け渡し用

    [SerializeField] private TextMeshProUGUI timeScoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var time = timeScore / 60.0f;
        timeScoreText.text = time.ToString("f1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReStart()
    {
        SceneManager.LoadScene("MobileTitle");
    }

    public void Quit()
    {
        Debug.Log("neko");
        Application.Quit();
    }
}
