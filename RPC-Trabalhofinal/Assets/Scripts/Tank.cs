using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Linq;
using UnityEngine;

public class TanK : MonoBehaviourPun, IDamageable
{

    public float _velocidadeRotacao = 100f; // Velocidade de rota��o do tanque
    public float _velocidadeMovimento = 5f; // Velocidade do movimento do tank
    private Rigidbody2D rb;
    private GameManager gm;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();
    }

    void FixedUpdate()
    {
        if (gm.isGameOver) { return; }

        if (photonView.IsMine) // verifica se � o jogador que controla o tanque
        {
            float moverHorizonalmente = Input.GetAxis("Horizontal"); // girar o tanque (A ou D)
            float moverVerticalmente = Input.GetAxis("Vertical"); // mover o tanque (W ou S)
            Move(moverHorizonalmente, moverVerticalmente);
        }
    }

    void Move(float moverHorizonalmente, float moverVerticalmente)
    {
        Vector2 movimento = transform.right * moverVerticalmente * _velocidadeMovimento * Time.fixedDeltaTime; // Movimento do tanque

        // Verifica colis�o com outros tanques
        RaycastHit2D hit = Physics2D.Raycast(rb.position, transform.right, movimento.magnitude);
        rb.MovePosition(rb.position + movimento);
        if (hit.collider == null) // Se n�o colidir com nada
        {
            rb.MovePosition(rb.position + movimento); // Mover o tanque
        }
        else
        {
            // Pode adicionar l�gica para reagir � colis�o, se necess�rio
            Debug.Log("Colis�o detectada com: " + hit.collider.name);
        }

        // Rotaciona o tanque (A ou D)
        float rotacao = -moverHorizonalmente * _velocidadeRotacao * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotacao);
    }

    [PunRPC]
    public void ResetarPosicaoNoSpawn()
    {
        var localizacaoSpawn1 = FindObjectOfType<GameManager>().SpawnLocalization(photonView.Owner);
        transform.position = localizacaoSpawn1.transform.position; // Seta ao tanque a posi��o do respawn
        transform.rotation = localizacaoSpawn1.transform.rotation; // Seta ao tanque a rota��o do respawn
    }

   public void TakeDamage() // m�todo para receber dano
    {
        photonView.RPC("ResetarPosicaoNoSpawn", photonView.Owner); // ao receber um dano, o tanque � teleportado para a �rea de respawn
                                                                   // Adiciona a pontua��o para o jogador que causou o dano
       /* var scoreManager = FindObjectOfType<Score>();
        if (scoreManager != null)
        {
            scoreManager.AddScore(photonView.Owner); // Chama o m�todo AddScore
        }
       */
    }
 
}
