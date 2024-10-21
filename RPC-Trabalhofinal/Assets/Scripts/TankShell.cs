
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankShell : MonoBehaviourPun
{                
        public float velocidade = 5f; ////Velocidade da bala
      public float timelife = 3f; //Tempo de vida até a bala se auto destruir
    float timelifeAtual = 0f;
        private GameObject atirador;  //GameObject do tanque que disparou

    public void Inicializar(GameObject atirador)//  //Inicializa a bala informando quem disparou ela
    {
            this.atirador = atirador;
        }

        // Update is called once per frame
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
            
            if (atirador == collision.gameObject)////Verifica se a bala está colidindo com o atirador
        {
                Debug.Log("A bala passou pelo próprio atirador");
                return;
            }

            IDamageable damageable = collision.GetComponent<IDamageable>();// bala colideiu vai ver se o objeto pode receber dano
            if (damageable != null)
            {
                damageable.TakeDamage();//Faz com que o objeto que recebeu o tiro e pode receber dano, receba o dano
            if (photonView.IsMine)// se a bala for minha e acertou o alvo contabiliza 1 ponto
                {
                    FindObjectOfType<Score>().AddScore(PhotonNetwork.LocalPlayer);/// vai ta adcionando o score para o jogador atual
                }
            }

            Destroy();
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
    

}