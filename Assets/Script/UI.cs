using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] Text turnText;
    public static UI instance;

    void Awake()
    {
        instance = this;
    }
    public void TurnChangetext()
    {
        turnText.text = (GameManager.isPlayer) ? "先手" : "後手";
        Debug.Log($"投了。次は{turnText.text}です。");
    }
}
