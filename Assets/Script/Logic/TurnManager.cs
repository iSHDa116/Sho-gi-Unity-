using UnityEngine;
public class TurnManager
{
    public PlayerType currentTurn = PlayerType.Sente;

    public void NextTurn()
    {
        PlayerType noePlayer = (currentTurn == PlayerType.Sente) ? PlayerType.Sente : PlayerType.Gote;
        currentTurn = noePlayer;
    }
}