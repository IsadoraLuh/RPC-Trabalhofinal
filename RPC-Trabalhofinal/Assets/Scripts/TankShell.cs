
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankShell : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 10, damage = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Movement();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        rb.velocity = transform.up * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}