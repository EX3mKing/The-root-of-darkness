using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float positionLeft;
    public float positionRight;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    public float speed;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-speed, rb.velocity.y);
        col = gameObject.GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (transform.position.x < positionLeft)
        {
            rb.velocity = new Vector2(speed,rb.velocity.y);
        }
        
        if (transform.position.x > positionRight)
        {
            rb.velocity = new Vector2(-speed,rb.velocity.y);
        }
        
    }
}
