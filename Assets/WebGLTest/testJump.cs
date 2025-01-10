using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testJump : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float jumpPower;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
            rb.AddForce(transform.up * jumpPower,ForceMode.Impulse);
    }
}
