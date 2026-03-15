using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    PlayerType playerType;
    public void StartTurn()
    {
        Debug.Log($"{playerType}のターン");
    }

    public void EndTurn()
    {
        
    }
}
