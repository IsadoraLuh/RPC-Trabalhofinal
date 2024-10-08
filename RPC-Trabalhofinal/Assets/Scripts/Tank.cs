using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    Rigidbody2D rb;
    public float rotationSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    private void FixedUpdate()
    {
        rb.MoveRotation(rb.rotation - Input.GetAxisRaw("Horizontal")* rotationSpeed);
    }
}
