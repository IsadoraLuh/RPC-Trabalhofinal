using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankCannon : MonoBehaviourPun, IShootable
{
    public Transform LocalizacaoSaidaShell;// pra ver onde vai ta saindo a bala
    public float _frequenciaShell = 1f;//so vai ta conseguindo atirar se ele carregou
    public float _frequenciaShellAtual = 0f;

    public void Shoot()
    {
        _frequenciaShellAtual = 0f;// vai ta resetando a frequ�ncia para 0, ja que o tanque precisa recarregar 
        var shell = PhotonNetwork.Instantiate("BalaPrefab", LocalizacaoSaidaShell.transform.position, LocalizacaoSaidaShell.transform.rotation);// se a bala e instancia entre rede
        shell.GetComponent<TankShell>().Inicializar(GetComponentInParent<TanK>().gameObject);
        shell.GetComponent<TankShell>().SetShooter(PhotonNetwork.LocalPlayer.ActorNumber);// PASSAR O NUMERO DO aCTOR DO DO JOGADOR QUE ATIROU PARA A BALA
    }

    void Update()
    {
        _frequenciaShellAtual += Time.deltaTime;// contabilizando a frequencia da bala

  
        if (photonView.IsMine)
        {
          
            if (Input.GetKeyDown(KeyCode.Space) && _frequenciaShellAtual > _frequenciaShell)//se o bot�o de espa�o do teclado foi apertado e se o tanque j� est� carregado para atirar
            {
                Shoot();// executat o metodo de atirar a bala
            }
        }
    }
}
