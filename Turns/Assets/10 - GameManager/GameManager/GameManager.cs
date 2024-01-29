using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private UiCntrl uiCntrl;
    [SerializeField] private BoardCntrl boardCntrl;
    [SerializeField] private CameraCntrl cameraCntrl;

    public static GameManager Instance = null;

    public int BoardSize { get; private set; }
    public int NColors { get; private set; }

    private bool blockWonPanel = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        BoardSize = gameData.boardSize;
        NColors = gameData.turnTileSymbolsPreFab.Length - 1;

        uiCntrl.Initialize();
    }

    /**
     * NewGame() - This function is executed when the "New" button is
     * clicked.  The board is reset and scrambled in an effort to start
     * a new game.
     */
    public void NewGame()
    {
        //uiCntrl.DisplayBoardSize(BoardSize);
        //uiCntrl.DisplayNColors(NColors);

        boardCntrl.DestroyBoard();
        boardCntrl.DisplayBoard();
        boardCntrl.ScrambleBoard();

        blockWonPanel = false;
        uiCntrl.TurnNewArrowOff();
    }

    /**
     * FlashWonPanel() - Display the Won panel, if it has not been blocked.
     * The panel is block, if it has already been shown but the new button
     * has not been clicked to start a new game.
     */
    public void FlashWonPanel()
    {
        if (!blockWonPanel)
        {
            uiCntrl.FlashWonPanel();
            blockWonPanel = true;
        }
    }

    /**
     * QuitGame() - Quit the game and close the application.
     */
    public void QuitGame()
    {
        Application.Quit();
    }

    /**
     * IncBoardSize() - 
     */
    public void IncBoardSize()
    {
        if (BoardSize < 5)
        {
            boardCntrl.DestroyBoard();
            uiCntrl.DisplayBoardSize(++BoardSize);
            boardCntrl.DisplayBoard();
        }
    }

    /**
     * DecBoardSize() -
     */
    public void DecBoardSize()
    {
        if (BoardSize > 3) 
        { 
            boardCntrl.DestroyBoard();
            uiCntrl.DisplayBoardSize(--BoardSize);
            boardCntrl.DisplayBoard();
        }
    }

    /**
     * IncNColors() - 
     */
    public void IncNColors()
    {
        if (NColors < 5)
        {
            uiCntrl.DisplayNColors(++NColors);
        }
    }

    /**
     * DecNColors() - 
     */
    public void DecNColors()
    {
        if (NColors > 2)
        {
            uiCntrl.DisplayNColors(--NColors);
        }
    }

    /*public void ReStart()
    {

    }*/

    public void SetEasyVarient()
    {
        boardCntrl.SetVarientType(VarientType.EASY);
        uiCntrl.ActivateButton(VarientType.EASY);
    }

    public void SetCrossVarient()
    {
        boardCntrl.SetVarientType(VarientType.CROSS);
        uiCntrl.ActivateButton(VarientType.CROSS);
    }

    public void SetCornersVarient()
    {
        boardCntrl.SetVarientType(VarientType.CORNERS);
        uiCntrl.ActivateButton(VarientType.CORNERS);
    }

    public void SetOnTopVarient()
    {
        boardCntrl.SetVarientType(VarientType.ONTOP);
        uiCntrl.ActivateButton(VarientType.ONTOP);
    }
}
