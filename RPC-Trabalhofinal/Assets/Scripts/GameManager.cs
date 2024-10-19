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
    //Referência as localizações que cada jogador começa
    public List<GameObject> localizacoesSpawn;

    //Referência ao texto de UI que tem o cronômetro
    public Text textTimer;

    //Tempo de partida em segundos
    public float tempoDePartida = 120f;
    private float tempoDePartidaAtual = 0f;

    //Boolean informando se o jogo finalizou
    public bool ehGameOver = false;

    void Start()
    {
        //Inicia como falso, pois antes de iniciar a partida, o contador não deve aparecer
        textTimer.gameObject.SetActive(false);
    }

    //Método responsável por iniciar a partida
    public void IniciarPartida()
    {
        ehGameOver = false;
        FindObjectOfType<Score>().ResetarPontuacao(PhotonNetwork.LocalPlayer);

        //Faz o cronometro aparecer
        tempoDePartidaAtual = tempoDePartida;

        textTimer.gameObject.SetActive(true);
        AtualizarTimerUI();

        //Inicia uma co-rotina que executa a cada 1 segundo para atualizar o tempo do cronometro
        StartCoroutine(TimerCoroutine());

        //Obtém o índice do jogador para saber onde o tanque deve nascer
        //var indiceJogador = (PhotonNetwork.LocalPlayer.ActorNumber -1) % localizacoesSpawn.Count;
        //var go = localizacoesSpawn[indiceJogador];
        var go = ObterLocalizacaoSpawn(PhotonNetwork.LocalPlayer);

        //Cria o tanque no local onde ele deve ser criado
        var tanque = PhotonNetwork.Instantiate("TanquePrefab", go.transform.position, go.transform.rotation);
    }

    public GameObject ObterLocalizacaoSpawn(Player player)
    {
        var indice = (player.ActorNumber - 1) % localizacoesSpawn.Count;
        return localizacoesSpawn[indice];
    }

    //Co-rotina responsável por contar e atualizar em tela o cronômetro
    private IEnumerator TimerCoroutine()
    {
        //Enquanto o tempo da partida não acabou e o jogo não finalizou, aguarda 1 segundo e atualiza a interface
        while (tempoDePartidaAtual > 0 && !ehGameOver)
        {
            //Aguarda 1 segundo
            yield return new WaitForSeconds(1f);

            //Diminui o tempo em 1 segundo
            tempoDePartidaAtual -= 1f;

            //Atualiza o tempo
            AtualizarTimerUI();
        }

        //Se o tempo acabou e o jogo ainda não finalizou, finaliza
        if (tempoDePartidaAtual <= 0 && !ehGameOver)
        {
            //Se for o host, efetua o término de partida
            if (PhotonNetwork.IsMasterClient)
            {
                //Efetua o processo de finalização do jogo, avisando todos no RPC que a partida finalizou
                photonView.RPC("TerminarJogo", RpcTarget.All);
            }

            //Para o contador de tempo
            StopCoroutine(TimerCoroutine());
        }
    }

    //Método responsável por finalizar o jogo
    [PunRPC]
    public void TerminarJogo()
    {
        //Marca o jogo como finalizado
        ehGameOver = true;

        FindObjectsByType<TanK>(FindObjectsSortMode.None).ToList().ForEach(tanque =>
        {
            if (tanque.photonView.IsMine)
            {
                //Destrói o tanque
                PhotonNetwork.Destroy(tanque.gameObject);
            }
        });

        
    }

    //Método responsável por atualizar o tempo
    void AtualizarTimerUI()
    {
        int minutos = Mathf.FloorToInt(tempoDePartidaAtual / 60);
        int segundos = Mathf.FloorToInt(tempoDePartidaAtual % 60);
        textTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}
