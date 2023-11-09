using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiCntrl : MonoBehaviour
{
    [SerializeField] private TMP_Text boardSizeText;

    public void DisplayBoardSize(int boardSize)
    {
        boardSizeText.text = boardSize.ToString();
    }
}
