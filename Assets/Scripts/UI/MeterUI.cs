using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEditor;

public class MeterUI : MonoBehaviour
{

    [SerializeField]
    private Image meterUI = null;
    private PlayerMove player = null;
	private Rigidbody rb = null;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();

        rb = player.GetComponent<Rigidbody>();
    }

	// Update is called once per frame
	private void FixedUpdate()
	{
		// meterUI.fillAmount = (player.Speed - player.Data.MinSpeed) / (player.MaxSpeed - player.Data.MinSpeed);
		// meterUI.fillAmount = player.Speed / player.MaxSpeed;
		// meterUI.fillAmount = (rb.velocity.magnitude - player.Data.MinSpeed) / (player.MaxSpeed - player.Data.MinSpeed);
		meterUI.fillAmount = rb.linearVelocity.magnitude / player.MaxSpeed;
	}
}
