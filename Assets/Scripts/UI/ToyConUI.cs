using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyConUI : MonoBehaviour
{
    [SerializeField]
    Camera camera = null;
    public PlayerMove player { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void FixedUpdate()
	{
        if(player != null)
        {
            transform.eulerAngles = new Vector3(player.Inputs[1].TotalGyro.x, player.Inputs[1].TotalGyro.z, player.Inputs[1].TotalGyro.y) + new Vector3(0.0f, 180.0f, 0.0f);
        }
	}
}
