using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    private float countTime = 0.0f;//���Ԃ��͂���
    [SerializeField] private float timeLimit = 5.0f;//��������
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        countTime += Time.deltaTime;//�}�C�t���[�����ɂ����������Ԃ𑫂��Ă���


        if (countTime > timeLimit)
        {
            SceneManager.LoadScene("KOINOBUS_Mobile");//�w�肵�����Ԃ��߂�����V�[���J�ځB("")�̒��ɑJ�ڐ�̃V�[���̖��O�������B
        }
    }
}
