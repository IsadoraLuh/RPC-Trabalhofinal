using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    
    public List<GameObject> localizacoesSpawn;//Referência as localizações que cada jogador começa
    public Text textTimer;// texto do cronometro
    public float tempoDePartida = 120f;
    private float tempoDePartidaAtual = 0f;
    public bool isGameOver = false;//Boolean pra informar  se o jogo finalizou


    private void Start()
    {
        photonView.RPC("StartGameforAll", RpcTarget.All);
    }

    public void StartGame() // vai ta iniciando a partida
    {
        isGameOver = false;
        FindObjectOfType<Score>().ResetarScore(PhotonNetwork.LocalPlayer);

        //Faz o cronometro aparecer
        tempoDePartidaAtual = tempoDePartida;

        textTimer.gameObject.SetActive(true);
        UptadeTimerUI();



        StartCoroutine(TimerCoroutine());// co-rotina para atualizar o tempo do cronometro

        var indiceJogador = (PhotonNetwork.LocalPlayer.ActorNumber -1) % localizacoesSpawn.Count; // ver o indece para saber onde o tank deve nascer
        var go = SpawnLocalization(PhotonNetwork.LocalPlayer);        //var go = localizacoesSpawn[indiceJogador];
        var tanque = PhotonNetwork.Instantiate("TanquePrefab - Copy", go.transform.position, go.transform.rotation);// vai ta criando o tank onde deve nascer assim
    }

    public GameObject SpawnLocalization(Player player)
    {
        var indice = (player.ActorNumber - 1) % localizacoesSpawn.Count;
        return localizacoesSpawn[indice];
    }

    private IEnumerator TimerCoroutine() // vai ta sendo responsável por contar e atualizar em tela o cronômetro
    {

        while (tempoDePartidaAtual > 0 && !isGameOver)// vai ta aguardando 1 segundo para e atualiznso a interface
        {
            yield return new WaitForSeconds(1f);
            tempoDePartidaAtual -= 1f;// //Diminui o tempo em 1 segundo
            UptadeTimerUI();////Atualizar o tempo
        }

   
        if (tempoDePartidaAtual <= 0 && !isGameOver)// finaliza o jogo se o tempo acabou
        {
            if (PhotonNetwork.IsMasterClient) // so host efetua o termino na partida
            {
                photonView.RPC("EndGame", RpcTarget.All);//avisando todos no RPC que a partida finalizou
            }

            StopCoroutine(TimerCoroutine());// vai ta parandoo o contador de tempo
        }
    }

  
    [PunRPC]
    public void EndGame() // vai ta sendo responsavel por finalizar o jogo
    {
        
        isGameOver = true;//Marcar o jogo como finalizado

        FindObjectsByType<TanK>(FindObjectsSortMode.None).ToList().ForEach(tank =>
        {
            if (tank.photonView.IsMine)
            {
  
                PhotonNetwork.Destroy(tank.gameObject);// para destruir o tank
            }
        }
        );

        
    }

    [PunRPC]
    public void StartGameforAll()
    {
        StartGame();
    }

 
    void UptadeTimerUI() // atualiza o tempo
    {
        int minutos = Mathf.FloorToInt(tempoDePartidaAtual / 60);
        int segundos = Mathf.FloorToInt(tempoDePartidaAtual % 60);
        textTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}
