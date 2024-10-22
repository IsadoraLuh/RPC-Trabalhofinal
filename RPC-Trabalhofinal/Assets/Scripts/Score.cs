using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
public class Score : MonoBehaviourPunCallbacks
{

    public void AddScore(Player player)// Adicionar a pontuação ao jogador
    {
        //Inicializa a pontuação como zero e obtem a pontuação do jogador
        int scoreAtual = 0;
        if (player.CustomProperties.ContainsKey("Pontuacao"))
        {
            scoreAtual = (int)player.CustomProperties["Pontuacao"];
        }

        //Adiciona a pontuação em 1
        scoreAtual += 1;

        //Atualiza a pontuação no PhotonPun e notifica todos jogadores, fazendo isso, o Photon Pun executará o método OnPlayerPropertiesUpdate da classe PontuacaoUIController
        Hashtable propriedadePontuacao = new Hashtable();
        propriedadePontuacao["Pontuacao"] = scoreAtual;
        player.SetCustomProperties(propriedadePontuacao);
       
    }
    // Adiciona a pontuação ao jogador
    public void ResetarScore(Player player)
   {
       //Atualiza a pontuação no PhotonPun e notifica todos jogadores, fazendo isso, o Photon Pun executará o método OnPlayerPropertiesUpdate da classe PontuacaoUIController
        Hashtable propriedadePontuacao = new Hashtable();
       propriedadePontuacao["Pontuacao"] = 0;
       player.SetCustomProperties(propriedadePontuacao);
       
    }
   
    
    


}
