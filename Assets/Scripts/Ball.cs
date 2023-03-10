using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private float maxVelocity = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        maxVelocity *= DataManager.Instance.GetDifficulty.value;
    }
    
    private void OnCollisionExit(Collision other)
    {
        var velocity = rb.velocity;
        
        //after a collision we accelerate a bit
        velocity += velocity.normalized * 0.01f;
        
        //check if we are not going totally vertically as this would lead to being stuck, we add a little vertical force
        if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.1f)
        {
            velocity += velocity.y > 0 ? Vector3.up * 0.5f : Vector3.down * 0.5f;
        }

        //max velocity
        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }

        rb.velocity = velocity;
    }
}
