using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

using UnityEngine;


public class Score : MonoBehaviourPunCallbacks
{  // Para armazenar a pontua��o de cada jogador
    private Dictionary<int, int> playerScores = new Dictionary<int, int>();

    public void AddScore(Player player)// Adicionar a pontua��o ao jogador
    {
        // vai inicializar a pontua��o como 0 e vai obter a pontua��o do jogador
        int scoreAtual = 0;
        if (player.CustomProperties.ContainsKey("Pontuacao"))
        {
            scoreAtual = (int)player.CustomProperties["Pontuacao"];
        }

        scoreAtual += 1; //Adiciona a pontua��o em 1

        //Atualiza a pontua��o no PhotonPun e notifica todos jogadores,ai Photon Pun executar� o m�todo OnPlayerPropertiesUpdate da classe PontuacaoUIController
        Hashtable propriedadePontuacao = new Hashtable();
        propriedadePontuacao["Pontuacao"] = scoreAtual;
        player.SetCustomProperties(propriedadePontuacao);
        // Atualiza o dicion�rio local de pontua��es
        playerScores[player.ActorNumber] = scoreAtual; // Atualiza a pontua��o no dicion�rio
    }
    
    public void ResetarScore(Player player)// reseta pontua��o ao jogador
    {
        
        Hashtable propriedadePontuacao = new Hashtable();////Atualiza a pontua��o no PhotonPun e notifica todos jogadores
        propriedadePontuacao["Pontuacao"] = 0;// reseta a pontuacao
        player.SetCustomProperties(propriedadePontuacao);
    }
    public void AnnounceWinner()
    {
        // Encontra o jogador com a maior pontua��o
        int winningActorNumber = -1;
        int highestScore = -1;

        foreach (var entry in playerScores)
        {
            if (entry.Value > highestScore)
            {
                highestScore = entry.Value;
                winningActorNumber = entry.Key;
            }
        }

        // Mostra a mensagem no console
        if (winningActorNumber != -1)
        {
            Debug.Log("Jogador " + winningActorNumber + " ganhou com " + highestScore + " pontos!");
        }
    }

    public void ResetScores()
    {
        playerScores.Clear(); // Limpa as pontua��es para a pr�xima partida
    }

}
