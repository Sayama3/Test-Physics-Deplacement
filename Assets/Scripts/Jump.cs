using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Title("Jump")] 
    [SerializeField] private float JumpPower = 500f;

    private Rigidbody rb;
    private bool doJump = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            doJump = true;
        }
    }

    private void FixedUpdate()
    {
        //Check if grounded
        
        
        if (doJump)
        {
            bool overlapSomething = false;
            var test = Physics.OverlapSphere(transform.position + (Vector3.down), 0.2f);
            for (int i = 0; i < test.Length; i++)
            {
                if (!test[i].CompareTag("Player"))
                {
                    overlapSomething = true;
                    break;
                }
            }

            if (overlapSomething)
            {
                rb.AddForce(Vector3.up * JumpPower,ForceMode.Impulse);
                            doJump = false;
            }
            
        }
    }
}
