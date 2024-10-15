using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static PlayerManager Instance;

    public GameObject tankPrefab;
    public Transform[] spawnPoints;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            CreatePlayerTank();
        }
    }

    public void CreatePlayerTank()
    {
        // Escolhe uma posição de spawn aleatória
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instancia o tanque do jogador no local escolhido
        PhotonNetwork.Instantiate(tankPrefab.name, spawnPoint.position, spawnPoint.rotation);
    }
}
