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
        NColors = gameData.turnTileSymbolsPreFab.Length;

        uiCntrl.Initialize();
    }

    public void NewGame()
    {
        uiCntrl.DisplayBoardSize(BoardSize);
        uiCntrl.DisplayNColors(NColors);

        boardCntrl.DisplayBoard();
    }

    /**
     * StartGame() - 
     */
    public void StartGame()
    {
        boardCntrl.StartGame();
    }

    /**
     * IncBoardSize() - 
     */
    public void IncBoardSize()
    {
        boardCntrl.DestroyBoard();

        uiCntrl.DisplayBoardSize(++BoardSize);

        cameraCntrl.UpdateCameraPosition(BoardSize - 3);

        boardCntrl.DisplayBoard();
    }

    /**
     * DecBoardSize() -
     */
    public void DecBoardSize()
    {
        boardCntrl.DestroyBoard();

        uiCntrl.DisplayBoardSize(--BoardSize);

        cameraCntrl.UpdateCameraPosition(BoardSize - 3);

        boardCntrl.DisplayBoard();
    }

    public void IncNColors()
    {
        uiCntrl.DisplayNColors(++NColors);
    }

    public void DecNColors()
    {
        uiCntrl.DisplayNColors(--NColors);
    }

    public void ReStart()
    {

    }

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
