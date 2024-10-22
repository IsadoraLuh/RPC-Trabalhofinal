using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviourPunCallbacks
{
    public List<GameObject> localizacoesSpawn; // Refer�ncia as localiza��es que cada jogador come�a
    public Text textTimer; // Texto do cron�metro
    public float tempoDePartida = 120f;
    private float tempoDePartidaAtual = 0f;
    public bool isGameOver = false; // Boolean para informar se o jogo finalizou

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartGameforAll", RpcTarget.All);
        }
    }

    public void StartGame() // Vai iniciar a partida
    {
        isGameOver = false;
       FindObjectOfType<Score>().ResetarScore(PhotonNetwork.LocalPlayer);

        // Configurar o cron�metro
        tempoDePartidaAtual = tempoDePartida;
        textTimer.gameObject.SetActive(true);
        UpdateTimerUI();

        // Somente o host inicia a corotina do cron�metro
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(TimerCoroutine());
        }

        // Criar o tanque na posi��o correta
        var go = SpawnLocalization(PhotonNetwork.LocalPlayer);
        PhotonNetwork.Instantiate("TanquePrefab - Copy", go.transform.position, go.transform.rotation);
    }

    public GameObject SpawnLocalization(Player player)
    {
        var indice = (player.ActorNumber - 1) % localizacoesSpawn.Count;
        return localizacoesSpawn[indice];
    }

    private IEnumerator TimerCoroutine() // Co-rotina para contar e atualizar o cron�metro na tela
    {
        while (tempoDePartidaAtual > 0 && !isGameOver)
        {
            yield return new WaitForSeconds(1f);
            tempoDePartidaAtual -= 1f;
            photonView.RPC("UpdateTimerUIForAll", RpcTarget.All, tempoDePartidaAtual);
        }

        // Finaliza o jogo se o tempo acabou
        if (tempoDePartidaAtual <= 0 && !isGameOver)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("EndGame", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void EndGame() // Finalizar o jogo
    {
        isGameOver = true;
        FindObjectsByType<TanK>(FindObjectsSortMode.None).ToList().ForEach(tank =>
        {
            if (tank.photonView.IsMine)
            {
                PhotonNetwork.Destroy(tank.gameObject);
            }
        });

    
    }

    [PunRPC]
    public void StartGameforAll()
    {
        if (!isGameOver)
        {
            StartGame();
        }
    }

    private void UpdateTimerUI() // Atualizar o cron�metro
    {
        int minutos = Mathf.FloorToInt(tempoDePartidaAtual / 60);
        int segundos = Mathf.FloorToInt(tempoDePartidaAtual % 60);
        textTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    [PunRPC]
    public void UpdateTimerUIForAll(float tempoAtual)
    {
        tempoDePartidaAtual = tempoAtual;
        UpdateTimerUI();
    }
}



