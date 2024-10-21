using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;


public class Score : MonoBehaviourPunCallbacks
{
    public void AddScore(Player player)// Adicionar a pontua��o ao jogador
    {
        // vai inicializar a pontua��o como 0 e vai obter a pontua��o do jogador
        int scoreAtual = 0;
        if (player.CustomProperties.ContainsKey("Pontuacao"))
        {
            scoreAtual = (int)player.CustomProperties["Pontuacao"];
        }

        //Adiciona a pontua��o em 1
        scoreAtual += 1;

        //Atualiza a pontua��o no PhotonPun e notifica todos jogadores,ai Photon Pun executar� o m�todo OnPlayerPropertiesUpdate da classe PontuacaoUIController
        Hashtable propriedadePontuacao = new Hashtable();
        propriedadePontuacao["Pontuacao"] = scoreAtual;
        player.SetCustomProperties(propriedadePontuacao);
    }
    
    public void ResetarScore(Player player)// Adicionar a pontua��o ao jogador
    {
        
        Hashtable propriedadePontuacao = new Hashtable();////Atualiza a pontua��o no PhotonPun e notifica todos jogadores
        propriedadePontuacao["Pontuacao"] = 0;
        player.SetCustomProperties(propriedadePontuacao);
    }
}
