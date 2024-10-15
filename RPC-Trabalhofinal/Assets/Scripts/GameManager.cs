using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI timerText; // Referência para o texto do timer
    public GameObject scorePanel; // Referência para o painel que conterá os scores
    public GameObject scoreEntryPrefab; // Prefab para a entrada de placar
    public int matchDuration = 120; // Duração de 2 minutos (120 segundos)
    private float timer;

    private Dictionary<int, int> playerScores = new Dictionary<int, int>();

    void Start()
    {
        timer = matchDuration;
        UpdateScoreUI();

      
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient) // Somente o MasterClient controla o temporizador
        {
            timer -= Time.deltaTime;
            photonView.RPC("UpdateTimer", RpcTarget.All, timer);

            if (timer <= 0)
            {
                EndGame();
            }
        }
    }

    [PunRPC]
    void UpdateTimer(float time)
    {
        timerText.text = "Time: " + Mathf.Ceil(time).ToString(); // Atualiza o texto do timer
    }

    public void AddScore(int playerID)
    {
        if (playerScores.ContainsKey(playerID))
        {
            playerScores[playerID]++;
        }
        else
        {
            playerScores[playerID] = 1; // Adiciona novo jogador
        }
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        // Limpa o painel de placar antes de atualizar
        foreach (Transform child in scorePanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Atualiza o placar para cada jogador
        foreach (var player in playerScores)
        {
            GameObject scoreEntry = Instantiate(scoreEntryPrefab, scorePanel.transform);
            scoreEntry.GetComponent<Text>().text = $"Player {player.Key}: {player.Value}"; // Exibe a pontuação
        }
    }

    void EndGame()

    {
        scorePanel.SetActive(true);
        string resultMessage = "Game Over!";

        photonView.RPC("ShowEndGameMessage", RpcTarget.All, resultMessage);
        // Espera alguns segundos antes de voltar para o lobby
        StartCoroutine(ReturnToLobbyAfterDelay(5)); // Aguarda 5 segundos
    }

    private IEnumerator ReturnToLobbyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.LoadLevel("LobbyScene"); // Retorna para o lobby
    }

    [PunRPC]
    void ShowEndGameMessage(string message)
    {
        // Mostrar mensagem na tela com o resultado (pode usar UI para isso)
        Debug.Log(message);
        // Aqui você pode mostrar a mensagem em um painel ou texto UI
    }
}

