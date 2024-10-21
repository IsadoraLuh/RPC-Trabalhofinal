using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


public class Plus : MonoBehaviourPunCallbacks
{
  
    //Referência ao botão de recomeçar a partida
    public Button buttonRecomecarPartida;

    //Referência ao texto da UI de status
    public Text textStatus;

    public void MostrarResultados()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            buttonRecomecarPartida.gameObject.SetActive(true);
        }
    }


    [PunRPC]
    public void RecomecarPartidaParaTodos()
    {
        //Esconde o texto e o botão pois a partida vai iniciar
        textStatus.gameObject.SetActive(false);
      
        buttonRecomecarPartida.gameObject.SetActive(false);

        //Procura o objeto e classe GameManager e inicia a partida
        var gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager.StartGame();
    }
}
