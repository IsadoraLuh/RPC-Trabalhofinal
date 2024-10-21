using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

using UnityEngine;


public class Score : MonoBehaviourPunCallbacks
{  // Para armazenar a pontuação de cada jogador
    private Dictionary<int, int> playerScores = new Dictionary<int, int>();

    public void AddScore(Player player)// Adicionar a pontuação ao jogador
    {
        // vai inicializar a pontuação como 0 e vai obter a pontuação do jogador
        int scoreAtual = 0;
        if (player.CustomProperties.ContainsKey("Pontuacao"))
        {
            scoreAtual = (int)player.CustomProperties["Pontuacao"];
        }

        scoreAtual += 1; //Adiciona a pontuação em 1

        //Atualiza a pontuação no PhotonPun e notifica todos jogadores,ai Photon Pun executará o método OnPlayerPropertiesUpdate da classe PontuacaoUIController
        Hashtable propriedadePontuacao = new Hashtable();
        propriedadePontuacao["Pontuacao"] = scoreAtual;
        player.SetCustomProperties(propriedadePontuacao);
        // Atualiza o dicionário local de pontuações
        playerScores[player.ActorNumber] = scoreAtual; // Atualiza a pontuação no dicionário
    }
    
    public void ResetarScore(Player player)// reseta pontuação ao jogador
    {
        
        Hashtable propriedadePontuacao = new Hashtable();////Atualiza a pontuação no PhotonPun e notifica todos jogadores
        propriedadePontuacao["Pontuacao"] = 0;// reseta a pontuacao
        player.SetCustomProperties(propriedadePontuacao);
    }
    public void AnnounceWinner()
    {
        // Encontra o jogador com a maior pontuação
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
        playerScores.Clear(); // Limpa as pontuações para a próxima partida
    }

}
