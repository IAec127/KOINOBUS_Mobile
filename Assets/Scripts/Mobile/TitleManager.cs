using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    private float countTime = 0.0f;//時間をはかる
    [SerializeField] private float timeLimit = 5.0f;//制限時間
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        countTime += Time.deltaTime;//マイフレーム事にかかった時間を足している


        if (countTime > timeLimit)
        {
            SceneManager.LoadScene("KOINOBUS_Mobile");//指定した時間が過ぎたらシーン遷移。("")の中に遷移先のシーンの名前をいれる。
        }
    }
}
