using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PieceTyoeで駒の種類を定義
public enum PieceType { Pawn, Rook, Bishop, Lance, Knight, SilverGeneral, GoldGeneral, King }

//PlayerTypeで先攻/後攻を定義
public enum PlayerType { Sente, Gote }

/*
    可読性向上のため、これらの定義には名前がつけられています。

    class Pieceで、駒とは何かを定義しています。
    
    Pieceクラスの中には、PieceType(駒の種類)とPlayerType(先攻/後攻)の二種類の情報が入っている
    この二種類の情報で、駒の状態を管理している
*/

public class Piece
{
    public PieceType pieceType;
    public PlayerType playerType;

    //コンストラクタ・・・new Piece()をするときに呼ばれる、特別な関数。
    //引数に入った情報を元に、作成した駒の情報が確定します
    public Piece(PieceType type, PlayerType player)
    {
        // 駒の種類の定義に使います
        pieceType = type;
        //その駒の先攻/後攻を決めます
        playerType = player;
    }
}
