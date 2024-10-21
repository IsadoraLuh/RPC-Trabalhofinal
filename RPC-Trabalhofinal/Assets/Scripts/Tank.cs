using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class TanK : MonoBehaviourPun, IDamageable
{
    //Velocidade de rota��o do tanque
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

    void Update()
    {
        if (gm.isGameOver)
        {
            return;
        }

        //Verifica se "sou eu"
        if (photonView.IsMine)
        {
            //Obt�m o comando de girar o tanque (A ou D)
            float moverHorizonalmente = Input.GetAxis("Horizontal");

            //Obt�m o comando de mover o tanque (W ou S)
            float moverVerticalmente = Input.GetAxis("Vertical");

            Move(moverHorizonalmente, moverVerticalmente);
        }
    }

    void Move(float moverHorizonalmente, float moverVerticalmente)
    {
    
        Vector2 movimento = transform.right * moverVerticalmente * _velocidadeMovimento * Time.fixedDeltaTime;// Movimento do tanque
        rb.MovePosition(rb.position + movimento);

        // Rotaciona o tanque (A ou D) - move no eixo Z para 2D
        float rotacao = -moverHorizonalmente * _velocidadeRotacao * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotacao);
    }

    
 
    [PunRPC]
    public void ResetarPosicaoNoSpawn()//respons�vel por resetar a posi��o
    {
        //Obt�m a posi��o com base no player
        var localizacaoSpawn1 = FindFirstObjectByType<GameManager>().SpawnLocalization(photonView.Owner);

        //Seta ao tanque, a posi��o do respawn
        transform.position = localizacaoSpawn1.transform.position;

        //Seta ao tanque, a rota��o do respawn
        transform.rotation = localizacaoSpawn1.transform.rotation;
    }
    public void TakeDamage() // metodo para receber dano
    {
        photonView.RPC("ResetarPosicaoNoSpawn", photonView.Owner);//ao receber um dano, o tanque � teleportado para a �rea de respawn
    }

}
