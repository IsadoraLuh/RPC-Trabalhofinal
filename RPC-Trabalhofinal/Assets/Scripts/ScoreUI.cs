using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;

public class ScoreUI : MonoBehaviourPunCallbacks
{
    public Text textScore;//texto de pontuacao
    public int actorNumber;//Número do "Actor" do Photon PUN para conseguir a referência de quem pertence esse UI

    //Método executado automaticamente pelo PhotonPun quando é identificado que algum jogador teve uma propriedade alterada
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // Verifica se a propriedade "Score" foi alterada e se deve atualizar esta UI
        if (changedProps.ContainsKey("Pontuacao") && targetPlayer.ActorNumber == actorNumber)
        {
            // UpdateScoreUI();// atualiza a ui quando a pontuacao muda 
            int newScore = (int)changedProps["Pontuacao"]; // Obtém a pontuação atualizada
            textScore.text = newScore.ToString(); // Atualiza o texto da UI
           
        }
    }
    /*private void Start()
    {
        actorNumber = PhotonNetwork.LocalPlayer.ActorNumber; //inicializa o numero do actor
        UpdateScoreUI();// atualiza a UI no inicia
    }

    private void UpdateScoreUI()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Pontuacao"))
        {
            int newScore = (int)PhotonNetwork.LocalPlayer.CustomProperties["Pontuacao"];
            textScore.text = newScore.ToString(); // Atualiza o texto com a pontuação atual
        }
    }
    */
}

