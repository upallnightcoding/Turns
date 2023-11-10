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
    }

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
}
