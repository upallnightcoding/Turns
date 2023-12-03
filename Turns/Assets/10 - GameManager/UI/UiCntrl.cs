using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiCntrl : MonoBehaviour
{
    private static Color ACTIVATE_COLOR = new Color(1.0f, 1.0f, 1.0f);
    private static Color DEACTIVE_COLOR = new Color(0.55f, 0.55f, 0.55f);

    [SerializeField] private TMP_Text boardSizeText;
    [SerializeField] private TMP_Text nColorText;
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private GameObject wonPanel;

    [SerializeField] private TMP_Text[] varientButtons;

    public void Initialize()
    {
        DeActivateAllVarientButtons();

        ActivateButton(varientButtons[(int)VarientType.EASY]);
    }

    public void FlashWonPanel()
    {
        StartCoroutine(DisplayWonPanel());
    }

    private IEnumerator DisplayWonPanel()
    {
        wonPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        wonPanel.SetActive(false);
    }

    public void ActivateButton(VarientType type)
    {
        DeActivateAllVarientButtons();

        ActivateButton(varientButtons[(int)type]);
    }

    /*
     * DeActivateAllVarientButtons() - 
     */
    private void DeActivateAllVarientButtons()
    {
        DeActivateButton(varientButtons[(int)VarientType.EASY]);
        DeActivateButton(varientButtons[(int)VarientType.CROSS]);
        DeActivateButton(varientButtons[(int)VarientType.CORNERS]);
        DeActivateButton(varientButtons[(int)VarientType.ONTOP]);
    }

    private void ActivateButton(TMP_Text button)
    {
        button.color = ACTIVATE_COLOR;
    }

    private void DeActivateButton(TMP_Text button)
    {
        button.color = DEACTIVE_COLOR;
    }

    public void DisplayBoardSize(int boardSize)
    {
        boardSizeText.text = boardSize.ToString();
    }

    public void DisplayNColors(int nColors)
    {
        nColorText.text = nColors.ToString();
    }

    public void DisplayInstructionPanel()
    {
        instructionPanel.SetActive(true);
    }

    public void HideInstructionPanel()
    {
        instructionPanel.SetActive(false);
    }
}
