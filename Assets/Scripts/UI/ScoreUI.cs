using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
	[SerializeField]
	private Text text = null;


	// Start is called before the first frame update
	void Awake()
    {
	}

	public void SetScoreUI(int score)
	{
		text.text = score.ToString();
	}
}
