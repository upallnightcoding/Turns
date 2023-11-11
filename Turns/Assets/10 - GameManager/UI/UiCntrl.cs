using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiCntrl : MonoBehaviour
{
    [SerializeField] private TMP_Text boardSizeText;
    [SerializeField] private TMP_Text nColorText;

    public void DisplayBoardSize(int boardSize)
    {
        boardSizeText.text = boardSize.ToString();
    }

    public void DisplayNColors(int nColors)
    {
        nColorText.text = nColors.ToString();
    }
}
