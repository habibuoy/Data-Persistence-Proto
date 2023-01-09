using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float speed = 2.0f;
    public float maxMovement = 2.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float input = Input.GetAxis("Horizontal");

        Vector3 pos = transform.position;
        pos.x += input * speed * Time.deltaTime;

        if (pos.x > maxMovement)
            pos.x = maxMovement;
        else if (pos.x < -maxMovement)
            pos.x = -maxMovement;

        transform.position = pos;
    }
}
