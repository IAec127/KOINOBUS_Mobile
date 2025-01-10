using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockOnUI : MonoBehaviour
{
    [SerializeField]
    private Color lockOnColor = Color.green;
	[SerializeField]
	private Color lockedOnColor = Color.red;
    [SerializeField]
    private Image lockOnImage = null;
    public Transform Target { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        InActiveUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (lockOnImage.enabled && Target != null)
        {
			Vector3 viewPos = Camera.main.WorldToViewportPoint(Target.position);
            viewPos.z = 0.0f;
            transform.position = Camera.main.ViewportToScreenPoint(viewPos);
        }
    }

    public void ActiveUI()
    {
        lockOnImage.enabled = true;
    }

    public void InActiveUI()
    {
		lockOnImage.enabled = false;
	}

    public void LockOut()
	{
		lockOnImage.color = lockOnColor;
	}

	public void LockedOn()
    {
        lockOnImage.color = lockedOnColor;
    }
}
