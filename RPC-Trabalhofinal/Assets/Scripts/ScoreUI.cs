using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviourPunCallbacks
{
    //Referência da UI de texto de pontuação
    public TextMeshPro
        textPontuacao;

    //Número do "Actor" do Photon PUN para ter a referência de quem pertence esta UI
    public int actorNumber;

    //Método executado automaticamente pelo PhotonPun quando é identificado que algum jogador teve uma propriedade alterada
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        // Verifica se a propriedade "Score" foi alterada e se deve atualizar esta UI
        if (changedProps.ContainsKey("Pontuacao") && targetPlayer.ActorNumber == actorNumber)
        {
            //Obtém a pontuação atualizada
            int newScore = (int)changedProps["Pontuacao"];

            //Altera a UI para o jogador
            textPontuacao.text = newScore.ToString();
        }
    }
}

