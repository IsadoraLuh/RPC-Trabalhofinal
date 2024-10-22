using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
public class Score : MonoBehaviourPunCallbacks
{

    public void AddScore(Player player)// Adicionar a pontua��o ao jogador
    {
        //Inicializa a pontua��o como zero e obtem a pontua��o do jogador
        int scoreAtual = 0;
        if (player.CustomProperties.ContainsKey("Pontuacao"))
        {
            scoreAtual = (int)player.CustomProperties["Pontuacao"];
        }

        //Adiciona a pontua��o em 1
        scoreAtual += 1;

        //Atualiza a pontua��o no PhotonPun e notifica todos jogadores, fazendo isso, o Photon Pun executar� o m�todo OnPlayerPropertiesUpdate da classe PontuacaoUIController
        Hashtable propriedadePontuacao = new Hashtable();
        propriedadePontuacao["Pontuacao"] = scoreAtual;
        player.SetCustomProperties(propriedadePontuacao);
       
    }
    // Adiciona a pontua��o ao jogador
    public void ResetarScore(Player player)
   {
       //Atualiza a pontua��o no PhotonPun e notifica todos jogadores, fazendo isso, o Photon Pun executar� o m�todo OnPlayerPropertiesUpdate da classe PontuacaoUIController
        Hashtable propriedadePontuacao = new Hashtable();
       propriedadePontuacao["Pontuacao"] = 0;
       player.SetCustomProperties(propriedadePontuacao);
       
    }
   
    
    


}
