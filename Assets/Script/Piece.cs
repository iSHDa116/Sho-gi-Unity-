using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PieceTyoeで駒の種類を定義
public enum PieceType { Pawn, Rook, Bishop, Lance, Knight, SilverGeneral, GoldGeneral, King }
//PlayerTypeで先攻/後攻を定義
public enum PlayerType { Sente, Gote }

//これらの定義は、わかりやすように名前をつけてあげているに過ぎない(可読性向上)

//class Pieceで、駒とは何かを定義している。classの中に属性を書いている
//classは設計図みたいなもので、これをもとに実際のコマを作る
//Pieceクラスの中には、PieceType(駒の種類)とPlayerType(先攻/後攻)の二種類の情報が入っている
//この二種類の情報で、駒の状態を管理している
public class Piece
{
    public PieceType pieceType;
    public PlayerType playerType;

    //コンストラクタ・・・new Piece()をするときに呼ばれる、特別な関数。引数
    public Piece(PieceType type, PlayerType player)
    {
        pieceType = type;
        playerType = player;
    }

}
