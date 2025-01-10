using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectPlayerList : MonoBehaviour
{
    public static ConnectPlayerList instance; // インスタンスの定義

    [SerializeField]
    public List<GameObject> cpList;

    private void Awake()
    {
        // シングルトンの呪文
        if (instance == null)
        {
            // 自身をインスタンスとする
            instance = this;
        }
        else
        {
            // インスタンスが複数存在しないように、既に存在していたら自身を消去する
            Destroy(gameObject);
        }
    }
}
