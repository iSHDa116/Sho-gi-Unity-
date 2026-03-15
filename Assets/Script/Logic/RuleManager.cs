using System.Collections.Generic;
using UnityEngine;

public class RuleManager
{
    Dictionary<PieceType, PieceType> PromtedTable = new Dictionary<PieceType, PieceType>()
    {
        {PieceType.Pawn,PieceType.GoldGeneral},
        {PieceType.Rook, PieceType.Dragon},
        {PieceType.Bishop, PieceType.Dragon},
        {PieceType.Lance, PieceType.GoldGeneral},
        {PieceType.Knight,PieceType.GoldGeneral}
    };

    public bool CanPromote(PieceType type)
    {
        return PromtedTable.ContainsKey(type);
    }

    public PieceType Promote(PieceType type)
    {
        if (PromtedTable.ContainsKey(type))
            return PromtedTable[type];

        return type;
    }
}