using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class TanK : MonoBehaviourPun, IDamageable
{
    //Velocidade de rotação do tanque
    public float _velocidadeRotacao = 100f;

    //Velocidade de movimento do tanque
    public float _velocidadeMovimento = 5f;

    private Rigidbody2D rb;
    private GameManager gm;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.ehGameOver)
        {
            return;
        }

        //Verifica se "sou eu"
        if (photonView.IsMine)
        {
            //Obtém o comando de girar o tanque (A ou D)
            float moverHorizonalmente = Input.GetAxis("Horizontal");

            //Obtém o comando de mover o tanque (W ou S)
            float moverVerticalmente = Input.GetAxis("Vertical");

            MoverTanque(moverHorizonalmente, moverVerticalmente);
        }
    }

    void MoverTanque(float moverHorizonalmente, float moverVerticalmente)
    {
        // Movimento do tanque (Move o tanque na direção em que ele está apontado)
        Vector2 movimento = transform.right * moverVerticalmente * _velocidadeMovimento * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movimento);

        // Rotaciona o tanque (A ou D) - move no eixo Z para 2D
        float rotacao = -moverHorizonalmente * _velocidadeRotacao * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotacao);
    }

    //Método herdado da interface que trata o recebimento de dano
  
    //Método responsável por resetar a posição
    [PunRPC]
    public void ResetarPosicaoNoSpawn()
    {
        //Obtém a posição com base no player
        var localizacaoSpawn1 = FindFirstObjectByType<GameManager>().ObterLocalizacaoSpawn(photonView.Owner);

        //Seta ao tanque, a posição do respawn
        transform.position = localizacaoSpawn1.transform.position;

        //Seta ao tanque, a rotação do respawn
        transform.rotation = localizacaoSpawn1.transform.rotation;
    }
    public void TakeDamage()
    {
        //No caso deste jogo, ao receber um dano, o tanque é teleportado para a área de respawn
        //Por este motivo, envia uma mensagem ao dono deste tanque para que ele resete a posição pois só ele pode fazer isso
        photonView.RPC("ResetarPosicaoNoSpawn", photonView.Owner);
    }

}
