using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;


public class Score : MonoBehaviourPunCallbacks
{
    public void AddScore(Player player)// Adicionar a pontuação ao jogador
    {
        // vai inicializar a pontuação como 0 e vai obter a pontuação do jogador
        int scoreAtual = 0;
        if (player.CustomProperties.ContainsKey("Pontuacao"))
        {
            scoreAtual = (int)player.CustomProperties["Pontuacao"];
        }

        //Adiciona a pontuação em 1
        scoreAtual += 1;

        //Atualiza a pontuação no PhotonPun e notifica todos jogadores,ai Photon Pun executará o método OnPlayerPropertiesUpdate da classe PontuacaoUIController
        Hashtable propriedadePontuacao = new Hashtable();
        propriedadePontuacao["Pontuacao"] = scoreAtual;
        player.SetCustomProperties(propriedadePontuacao);
    }
    
    public void ResetarScore(Player player)// Adicionar a pontuação ao jogador
    {
        
        Hashtable propriedadePontuacao = new Hashtable();////Atualiza a pontuação no PhotonPun e notifica todos jogadores
        propriedadePontuacao["Pontuacao"] = 0;
        player.SetCustomProperties(propriedadePontuacao);
    }
}
