using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownUI : MonoBehaviour
{
    [SerializeField]
    private Image background = null;
    [SerializeField]
    private Text text = null;
    [SerializeField]
    private List<Color> colorList = new List<Color>();
    [SerializeField]
    private float GoSize = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        SceneController.instance.CountDownUI = this;
        InActive();
    }

    
    public IEnumerator CountDown()
    {
        Active();
        yield return StartCoroutine(ShowNum("3", colorList[0], 1.0f));

		yield return StartCoroutine(ShowNum("2", colorList[1], 1.0f));

		yield return StartCoroutine(ShowNum("1", colorList[2], 1.0f));

		yield return StartCoroutine(ShowGo("Go", colorList[3], 1.0f));

		text.text = "";
        InActive();
        SceneController.instance.StartGame();
	}

    public IEnumerator ShowNum(string str,Color color,float time)
    {
        float nowTime = 0.0f;
        text.text = str;
        text.color = color;
        float diffAlpha = 1.0f / time;
        Color c = text.color;
        while (true)
        {
            c.a -= diffAlpha * Time.deltaTime;
            nowTime += Time.deltaTime;
            text.color = c;
            if (nowTime >= time)
            {
                c.a = 0.0f;
                text.color = c;
                yield break;
            }
            yield return null;
        }
    }

	public IEnumerator ShowGo(string str, Color color, float time)
	{
        background.enabled = false;
		float nowTime = 0.0f;
		text.text = str;
		text.color = color;
		float diffAlpha = 1.0f / time;
		Color c = text.color;
        Vector3 startsSize = text.gameObject.transform.localScale;
        float addSize = (GoSize - startsSize.x) / time;
		while (true)
		{
			c.a -= diffAlpha * Time.deltaTime;
            float s = addSize * Time.deltaTime;
            text.gameObject.transform.localScale += new Vector3(s, s, s);
			nowTime += Time.deltaTime;
			text.color = c;
			if (nowTime >= time)
			{
				c.a = 0.0f;
				text.color = c;
				text.gameObject.transform.localScale = startsSize;
				yield break;
			}
            yield return null;
		}
	}


	public void Active()
    {
        background.enabled = true;
        text.enabled = true;
    }

    public void InActive()
    {
		background.enabled = false;
		text.enabled = false;
	}
}
