
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ExitGames.Client.Photon;


public class TankShell : MonoBehaviourPun
{
    public float velocidade = 5f; ////Velocidade da bala
    public float timelife = 1f; //Tempo de vida até a bala se auto destruir
    float timelifeAtual = 0f;
    private GameObject atirador;  //GameObject do tanque que disparou
    private int shooterActorNumber;// numero do jogador que atirou

    public void Inicializar(GameObject atirador)//  //Inicializa a bala informando quem disparou ela
    {
        this.atirador = atirador;
        //shooterActorNumber = atirador.GetComponent<PhotonView>().Owner.ActorNumber;// armazena o ator que atirou
    }
    void Update()
    {
        transform.Translate(Vector3.right * velocidade * Time.deltaTime);///movimentar a bala
        timelifeAtual += Time.deltaTime;//Contabilizar o tempo para ver se deve destruir a bala
        if (timelifeAtual > timelife)
        {

            Destroy();// para destruir a bala
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (atirador == collision.gameObject)//// Ignora colisao com o proprio atirador 
        {
            Debug.Log("A bala passou pelo próprio atirador");
            return;
        }

        IDamageable damageable = collision.GetComponent<IDamageable>();// bala colideiu vai ver se o objeto pode receber dano
        if (damageable != null)
        {
            damageable.TakeDamage();//Faz com que o objeto que recebeu o tiro e pode receber dano, receba o dano
           
            if (photonView.IsMine)
            {
                FindObjectOfType<Score>().AddScore(PhotonNetwork.LocalPlayer);/// vai ta adcionando o score para o jogador atual
            }

            Destroy();// destruir a bala apos impacto
         
        }

      
    }

    void Destroy()
    {
        if (photonView.IsMine)
         {
             PhotonNetwork.Destroy(gameObject);// destrui a bala apos colisao
         }
         else if (PhotonNetwork.IsMasterClient)
         {

             PhotonNetwork.Destroy(gameObject);// destruir bala pos colisao
         }
        
       
    }



    public void SetShooter(int actorNumber)
    {
        shooterActorNumber = actorNumber;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tanque"))
        {
            TanK tank = collision.gameObject.GetComponent<TanK>();
            if (tank != null)
            {
                // Chama o método de dano e passa o número do jogador que deu o tiro
                tank.photonView.RPC("TakeDamage", RpcTarget.All);
                photonView.RPC("AtualizarPontuacao", RpcTarget.MasterClient, shooterActorNumber);// envia um rpc para o masterclient para atualizar a pontuacao

                PhotonNetwork.Destroy(gameObject); // Destruir a bala após o impacto
            }
        }
    }
    [PunRPC]
    public void AtualizarPontuacao(int shooterActorNumber)
    {
        // Apenas o MasterClient deve executar essa lógica
        if (PhotonNetwork.IsMasterClient)
        {
            Player shooter = PhotonNetwork.PlayerList.FirstOrDefault(p => p.ActorNumber == shooterActorNumber);
            if (shooter != null)
            {
                // Pega a pontuação atual
                int score = shooter.CustomProperties.ContainsKey("Pontuacao") ? (int)shooter.CustomProperties["Pontuacao"] : 0;

                // Use a versão correta do Hashtable do Photon
                ExitGames.Client.Photon.Hashtable newScore = new ExitGames.Client.Photon.Hashtable();
                newScore["Pontuacao"] = score + 1;

                // Atualiza as propriedades do jogador
                shooter.SetCustomProperties(newScore);
            }
        }
    }

   
}
