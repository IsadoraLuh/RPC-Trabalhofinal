using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviourPun
{

    #region Singleton
    public static PlayerManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
    #endregion

    Vector2 screenBounds;
   // const string playerPrefabPath = "Prefabs/Player";
    int playersInGame = 0;

    public Vector2 ScreenBounds { get => screenBounds; }

    public GameObject tankPrefab;
    public Transform[] spawnPoints;

    private void Start()
    {
        photonView.RPC("AddPlayer", RpcTarget.AllBuffered);
    }

    private void CreatePlayer()
    { // Escolhe uma posição de spawn aleatória
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        // Instancia o tanque do jogador no local escolhido
        PhotonNetwork.Instantiate(tankPrefab.name, spawnPoint.position, spawnPoint.rotation);
        
    }

    [PunRPC]
    private void AddPlayer()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length)
        {
            CreatePlayer();
        }
    }

   /* void Start()
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
   */
}

