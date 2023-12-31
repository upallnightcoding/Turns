using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private Transform boardParent;
    [SerializeField] private LayerMask clickLayerMask;
    [SerializeField] private AudioSource audioSource;

    private VarientType varientType = VarientType.EASY;

    private GameObject[,] board;

    // Start is called before the first frame update
    void Start()
    {
        DisplayBoard();
        ScrambleBoard();
    }

    public void DisplayBoard()
    {
        CreateBoard(boardParent);
    }

    public void SetVarientType(VarientType type)
    {
        varientType = type;
    }

    public void DestroyBoard()
    {
        int boardSize = GameManager.Instance.BoardSize;

        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                Destroy(board[col, row]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RotateTile(false);
            audioSource.clip = PickTurningSound();
            audioSource.Play();

            if(CheckMatches())
            {
                GameManager.Instance.FlashWonPanel();
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            RotateTile(true);
            audioSource.clip = PickTurningSound();
            audioSource.Play();

            if (CheckMatches())
            {
                GameManager.Instance.FlashWonPanel();
            }
        }

    }

    private AudioClip PickTurningSound()
    {
        return (gameData.turningTilesSound[Random.Range(0, gameData.turningTilesSound.Length)]);
    }

    //=========================================================================

    private Vector3 GetPosition(int col, int row)
    {
        Vector4 position = Vector3.zero;

        switch(GameManager.Instance.BoardSize)
        {
            case 3: 
                position = new Vector3(col * 5 - 5, 0.0f, row * 5 - 5);
                break;
            case 4: 
                position = new Vector3((col - 2) * 5.0f + 2.5f, 0.0f, (row - 2) * 5.0f + 2.5f);
                break;
            case 5: 
                position = new Vector3((col - 2) * 5.0f, 0.0f, (row - 2) * 5.0f);
                break;
        }

        return (position);
    }

    private void CreateBoard(Transform parent)
    {
        int boardSize = GameManager.Instance.BoardSize;

        board = new GameObject[boardSize, boardSize];

        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                Vector3 position = GetPosition(col, row);

                GameObject tilePreFab = gameData.turnTilePreFab;
                GameObject tile = Instantiate(tilePreFab, position, Quaternion.identity);
                board[col, row] = tile;

                TileCntrl tileCntrl = tile.GetComponent<TileCntrl>();
                tileCntrl.Initialize(col, row);

                tileCntrl.IsCorner =
                    ((row == 0) && (col == 0)) ||
                    ((row == 0) && (col == boardSize - 1)) ||
                    ((row == boardSize - 1) && (col == 0)) ||
                    ((row == boardSize - 1) && (col == boardSize - 1));

                CreateTileSymbols(tile, GameData.NORTH_TILE, GetSymbol());
                CreateTileSymbols(tile, GameData.EAST_TILE, GetSymbol());
            }
        }

        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                GameObject tile = board[col, row];
                //tile.GetComponent<TileCntrl>().Initialize(col, row);

                if (col == 0)
                {
                    CreateTileSymbols(tile, GameData.WEST_TILE, GetSymbol());
                } else
                {
                    int match = board[col - 1, row].GetComponent<TileCntrl>().GetSymbol(GameData.EAST_TILE);
                    CreateTileSymbols(tile, GameData.WEST_TILE, match);
                }

                if (row == 0)
                {
                    CreateTileSymbols(tile, GameData.SOUTH_TILE, GetSymbol());
                } else
                {
                    int match = board[col, row - 1].GetComponent<TileCntrl>().GetSymbol(GameData.NORTH_TILE);
                    CreateTileSymbols(tile, GameData.SOUTH_TILE, match);
                }
            }
        }
    }

    private bool CheckMatches()
    {
        int boardSize = GameManager.Instance.BoardSize;
        bool wonGame = true;

        for (int row = 0; (row < boardSize) && wonGame; row++)
        {
            for (int col = 0; (col < boardSize) && wonGame; col++)
            {
                bool north = SymbolCompare(col, row, GameData.NORTH_TILE, col, row + 1, GameData.SOUTH_TILE, boardSize);
                bool south = SymbolCompare(col, row, GameData.SOUTH_TILE, col, row - 1, GameData.NORTH_TILE, boardSize);
                bool east  = SymbolCompare(col, row, GameData.EAST_TILE, col + 1, row, GameData.WEST_TILE, boardSize);
                bool west  = SymbolCompare(col, row, GameData.WEST_TILE, col - 1, row, GameData.EAST_TILE, boardSize);

                wonGame = north && south && east && west;
            }
        }

        return (wonGame);
    }

    private bool SymbolCompare(int col, int row, int direction, int ccol, int crow, int compdirection, int boardSize)
    {
        bool match = true;

        if ((ccol >= 0) && (ccol < boardSize) && (crow >= 0) && (crow < boardSize))
        {
            int symbol = board[col, row].GetComponent<TileCntrl>().GetSymbol(direction);
            int compare = board[ccol, crow].GetComponent<TileCntrl>().GetSymbol(compdirection);

            match = (symbol == compare);
        }

        return (match);
    }

    /**
     * ScrambleBoard() - 
     */
    public void ScrambleBoard()
    {
        int boardSize = GameManager.Instance.BoardSize;

        for (int n = 0; n < gameData.nScramble * boardSize; n++)
        {
            int col = Random.Range(0, boardSize);
            int row = Random.Range(0, boardSize);

            GameObject tile = board[col, row];

            Transform parent = tile.transform;
            bool turn = Random.Range(0, 2) == 0;

            TurnTile(parent, turn);

            MakeVarientMove(parent, turn);
        }
    }


    /**
     * CreateTileSymbols() - 
     */
    private void CreateTileSymbols(GameObject tile, int symbolPosIndex, int symbolIndex)
    {
        GameObject symbolPreFab = gameData.turnTileSymbolsPreFab[symbolIndex];

        string name = gameData.symbolName[symbolPosIndex];
        Vector3 position = tile.transform.Find(name).transform.position;
        tile.GetComponent<TileCntrl>().Set(symbolPosIndex, symbolIndex);

        GameObject symbol = Instantiate(symbolPreFab, position, Quaternion.identity);
        symbol.name = name + "_Symbol"; 

        symbol.transform.SetParent(tile.transform);
    }

    /**
     * GetSymbol() - 
     */
    private int GetSymbol()
    {
        return(Random.Range(0, GameManager.Instance.NColors));
    }

    /**
     * RotateTile() - 
     */
    private void RotateTile(bool turn)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray screenPointToRay = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(screenPointToRay, out RaycastHit hit, 1000, clickLayerMask))
        {
            Transform parent = hit.transform.parent;

            TurnTile(parent, turn);

            MakeVarientMove(parent, turn);
        }
    }

    private void MakeVarientMove(Transform parent, bool turn)
    {
        TileCntrl tileCntrl = parent.GetComponent<TileCntrl>();
        int col = tileCntrl.Col;
        int row = tileCntrl.Row;

        switch (varientType)
        {
            case VarientType.EASY:
                break;
            case VarientType.CROSS:
                TurnTile(col, row + 1, !turn);
                TurnTile(col, row - 1, !turn);
                TurnTile(col + 1, row, !turn);
                TurnTile(col - 1, row, !turn);
                break;
            case VarientType.CORNERS:
                ExecuteCornersVarient(tileCntrl, parent, turn);
                break;
            case VarientType.ONTOP:
                TurnTile(col, row + 1, !turn);
                break;
        }
    }

    private void ExecuteCornersVarient(TileCntrl tileCntrl, Transform parent, bool turn)
    {
        if (tileCntrl.IsCorner)
        {
            int boardSize = GameManager.Instance.BoardSize;

            TurnTile(parent, !turn);

            TurnTile(0, 0, turn);
            TurnTile(0, boardSize-1, turn);
            TurnTile(boardSize-1, 0, turn);
            TurnTile(boardSize-1, boardSize-1, turn);
        }
    }

    private void TurnTile(int col, int row, bool turn)
    {
        int boardSize = GameManager.Instance.BoardSize;

        if ((col >= 0) && (col < boardSize) && (row >= 0) && (row < boardSize))
        {
            TurnTile(board[col, row].transform, turn);
        }
    }

    /**
     * TurnTile() - 
     */
    private void TurnTile(Transform parent, bool turn)
    {
        Rotate(parent, turn);

        Rotate(parent.Find("NorthTile_Symbol").transform, !turn);
        Rotate(parent.Find("SouthTile_Symbol").transform, !turn);
        Rotate(parent.Find("EastTile_Symbol").transform, !turn);
        Rotate(parent.Find("WestTile_Symbol").transform, !turn);
    }

    /**
     * Rotate() - Execute the actual turn of a tile.  The RotationCntrl uses a
     * boolean to determine the direction to rotate the tile by 90 degrees.  If
     * the transform of the object does not have a RotationCntrl component the
     * rotation is skipped.
     */
    private void Rotate(Transform transform, bool turn)
    {
        if (transform.TryGetComponent<RotationCntrl>(out RotationCntrl rotationCntrl))
        {
            rotationCntrl.RotateTile(turn);
        }

        if (transform.TryGetComponent<TileCntrl>(out TileCntrl tileCntrl))
        {
            if (turn)
            {
                tileCntrl.ClockWise();
            } else
            {
                tileCntrl.CounterClockWise();
            }
        }
    }
}

public enum VarientType
{
    EASY = 0,
    CROSS = 1,
    CORNERS = 2,
    ONTOP = 3
}
