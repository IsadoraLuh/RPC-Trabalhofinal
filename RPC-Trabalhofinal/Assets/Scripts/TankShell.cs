
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankShell : MonoBehaviourPun
{                
        public float velocidade = 5f; ////Velocidade da bala

    //Tempo de vida até a bala se auto destruir
    public float tempoDeVida = 3f;
        float tempoDeVidaAtual = 0f;

        //GameObject do tanque que disparou
        private GameObject atirador;

        //Inicializa a bala informando quem disparou ela
        public void Inicializar(GameObject atirador)
        {
            this.atirador = atirador;
        }

        // Update is called once per frame
        void Update()
        {
            //Faz a bala se movimentar 
            transform.Translate(Vector3.right * velocidade * Time.deltaTime);

            //Contabiliza o tempo para saber se deve destruir a bala
            tempoDeVidaAtual += Time.deltaTime;
            if (tempoDeVidaAtual > tempoDeVida)
            {
                //Destrói a bala
                AutoDestruir();
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            //Verifica se a bala está colidindo com o atirador, neste caso, não deve fazer nada
            if (atirador == collision.gameObject)
            {
                Debug.Log("A bala passou pelo próprio atirador");
                return;
            }

            //Obtem e verifica se o objeto com quem a bala colidiu, pode receber dano
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                //Faz com que o objeto que recebeu o tiro e pode receber dano, receba o dano
                damageable.TakeDamage();

                //Verifica se a bala "é minha", se for e acertou o alvo, contabiliza um ponto
                if (photonView.IsMine)
                {
                    //Adiciona a pontuação para o jogador atual
                    FindObjectOfType<Score>().AdicionarPontuacao(PhotonNetwork.LocalPlayer);
                }
            }

            AutoDestruir();
        }

        void AutoDestruir()
        {
            //Somente pode destruir o objeto quem é o dono dele ou o host da partida
            if (photonView.IsMine)
            {
                // Destrói a bala após a colisão
                PhotonNetwork.Destroy(gameObject);
            }
            else if (PhotonNetwork.IsMasterClient)
            {
                // Destrói a bala após a colisão
                PhotonNetwork.Destroy(gameObject);
            }
        }
    

}