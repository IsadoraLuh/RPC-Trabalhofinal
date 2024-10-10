using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    Rigidbody2D rb;
    public float rotationSpeed = 5;

    Vector2 moveAmount;
    public float moveSpeed = 0.09f;

    public GameObject TankShellPrebab;
    public Transform spawnLocation;

    float shootTime = 0;
    public float coolDown = 3;
    public int health = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        moveAmount = transform.up * Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Fire1"))
        {
            if(Time.time> shootTime)
            {
                GameObject bullet = Instantiate(TankShellPrebab, spawnLocation.position, spawnLocation.rotation);
                shootTime = Time.time + coolDown;
            }
        }

    }
    private void FixedUpdate()
    {
        rb.MoveRotation(rb.rotation - Input.GetAxisRaw("Horizontal")* rotationSpeed);
        rb.MovePosition(rb.position + (Vector2)transform.up * moveSpeed);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        health = health - 1;

        if(health <1)
        {
            Destroy(gameObject);
        }
    }
}
