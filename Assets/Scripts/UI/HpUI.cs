using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour
{
    [SerializeField]
    List<Image> hpImageList=new List<Image> ();
    // Start is called before the first frame update
    void Awake()
    {
    }

    public void SetHpUI(int hp)
    {
        foreach (Image image in hpImageList)
        {
            image.enabled = false;
        }
        for(int i=0;i<hp;i++)
        {
            hpImageList[i].enabled = true;
        }
    }
}
