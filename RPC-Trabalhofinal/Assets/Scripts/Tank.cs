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
            GameObject bullet = Instantiate(TankShellPrebab, spawnLocation.position, spawnLocation.rotation  );
        }

    }
    private void FixedUpdate()
    {
        rb.MoveRotation(rb.rotation - Input.GetAxisRaw("Horizontal")* rotationSpeed);
        rb.MovePosition(rb.position + new Vector2( 0, moveSpeed));
    }
}
