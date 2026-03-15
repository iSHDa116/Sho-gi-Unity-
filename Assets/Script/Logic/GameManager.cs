using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isPlayer = true;

    public Player sentePlayer;
    public Player gotePlayer;
    Player currentPlayer;

    [Header("駒")]
    [SerializeField] GameObject[] piecePrefab = new GameObject[9]; // 0-7 for pieces, 8 for King Gote

    [Header("インスタンス化")]
    [SerializeField] BoardView boardView;
    Board board = new Board();

    public static GameManager Instance;
    [Header("その他")]
    [SerializeField] Camera cam;
    [SerializeField] Transform pieceManager;

    //シングルトンパターン・・・ゲーム全体で唯一存在するクラスを作る、デザインパターン。
    //どこからでもアクセスできる
    //変数Instanceに自分自身を代入することで、代入したクラス(今回はGameManager)を探さず、直接呼び出せる
    //クラスをインスタンス化しなくても使える(staticはMonobehaviourを継承できないから、Unityにおいてはこちらを使うことが多い)
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Instanceはすでに存在します");
        }
    }
    void Start()
    {
        GameStart();
    }

    void GameStart()
    {
        boardView.SetBoard();
        currentPlayer = sentePlayer;
        currentPlayer.StartTurn();
    }

    void GameOver()
    {
        currentPlayer = (currentPlayer == sentePlayer) ? sentePlayer : gotePlayer;

        currentPlayer.EndTurn();
    }

    public void TurnChange()
    {
        isPlayer = !isPlayer;
        UIManager.instance.TurnChangetext();
        cam.transform.Rotate(0, 0, 180);
    }
}