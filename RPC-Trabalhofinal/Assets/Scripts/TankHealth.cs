using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankHealth : MonoBehaviourPun, IDamageable
{
    public float maxHealth = 100f;
    private float currentHealth;
    public Transform[] spawnPoints;
    private GameManager gameManager;

    private void Start()

    {
        if(photonView.IsMine)
        {
            Respawn();
        }
       gameManager = FindObjectOfType<GameManager>();
        currentHealth = 100;
    }
    [PunRPC]
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            photonView.RPC("Respawn", RpcTarget.All);
            if (gameManager != null)
            {
                gameManager.photonView.RPC("AddScore", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void Respawn()
    {
        currentHealth = maxHealth;
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }
}
