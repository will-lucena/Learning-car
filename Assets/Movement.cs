using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    private Rigidbody rb;
    private float hInput;
    private float vInput;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        vInput = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        float hMovement = speed * hInput * Time.fixedDeltaTime;
        float vMovement = speed * vInput * Time.fixedDeltaTime;
        rb.velocity = new Vector3(hMovement, 0, vMovement);
    }
}
