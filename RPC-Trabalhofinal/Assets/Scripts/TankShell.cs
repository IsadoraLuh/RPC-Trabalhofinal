using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShell : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 2, damage = 2;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public float Damage
    {
        get => damage;
        set
        {
            if (damage ==0)
            {
                damage = value;
            }
        }
    }
  
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        rb.velocity = transform.up * speed;

    }
}
