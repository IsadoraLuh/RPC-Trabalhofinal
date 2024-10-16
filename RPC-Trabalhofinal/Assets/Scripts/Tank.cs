using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tank : MonoBehaviourPunCallbacks, IMovable
{
    Rigidbody2D rb;
    public float rotationSpeed = 5;
    public float moveSpeed = 0.09f;
    public GameObject tankShellPrefab;
    public Transform spawnLocation;
    public int health = 5;
    public float shootCooldown = 2f;
    private float shootTime = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

       //if (photonView.IsMine)
      // {
          // Camera.main.GetComponent<CameraFollow>().SetTarget(transform);
       // }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        Vector2 moveDirection = new Vector2(0, Input.GetAxisRaw("Vertical"));
        Move(moveDirection);

        if (Input.GetButtonDown("Fire1") && Time.time > shootTime)
        {
            photonView.RPC("Shoot", RpcTarget.All);
            shootTime = Time.time + shootCooldown;
            Debug.Log("Tiroo");
        }

        float rotationAmount = -Input.GetAxisRaw("Horizontal") * rotationSpeed;
        Rotate(rotationAmount);
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        rb.MovePosition(rb.position + (Vector2)transform.up * moveSpeed);
    }

    [PunRPC]
    public void Shoot()
    {
        Instantiate(tankShellPrefab, spawnLocation.position, spawnLocation.rotation);
    }

   /* public void TakeDamage(float amount)
    {
        health -= (int)amount;
        if (health <= 0) 
        {
            photonView.RPC("Respawn", RpcTarget.All);
        }
    }

   s [PunRPC]
    private void Respawn()
    {
        // Código para respawn em posição aleatória do mapa
        Vector2 respawnPosition = PlayerManager.Instance.spawnPoints[Random.Range(0, PlayerManager.Instance.spawnPoints.Length)].position;
    transform.position = respawnPosition;
    health = 100;
    }
   */
    public void Move(Vector2 direction)
    {
        rb.MovePosition(rb.position + (Vector2)transform.up * moveSpeed);
    }

    public void Rotate(float amount)
    {
        rb.MoveRotation(rb.rotation + amount);
    }

    [PunRPC]
    private void Initialize()
    {
        if (!photonView.IsMine)
        {
            Color color = Color.white;
            color.a = 0.1f;
            GetComponent<SpriteRenderer>().color = color;
        }
    }
}
