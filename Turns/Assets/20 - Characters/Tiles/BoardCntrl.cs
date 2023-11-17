using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private Transform boardParent;

    private VarientType varientType = VarientType.EASY;

    private GameObject[,] board;

    // Start is called before the first frame update
    void Start()
    {
        DisplayBoard();
    }

    public void DisplayBoard()
    {
        CreateBoard(boardParent);
    }

    public void StartGame()
    {
        ScrambleBoard();
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
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            RotateTile(true);
        }
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
                //Vector3 position = new Vector3(col * 5 - 5, 0.0f, row * 5 - 5);
                //Vector3 position = new Vector3((col-2) * 5.0f + 2.5f, 0.0f, (row-2) * 5.0f + 2.5f);
                //Vector3 position = new Vector3((col - 2) * 5.0f, 0.0f, (row - 2) * 5.0f);
                Vector3 position = GetPosition(col, row);

                GameObject tilePreFab = gameData.turnTilePreFab;
                GameObject tile = Instantiate(tilePreFab, position, Quaternion.identity);
                tile.GetComponent<TileCntrl>().Initialize(col, row);
                board[col, row] = tile;
                //tile.transform.SetParent(parent);

                CreateTileSymbols(tile, GameData.NORTH_TILE, GetSymbol());
                CreateTileSymbols(tile, GameData.EAST_TILE, GetSymbol());
            }
        }

        for (int row = 0; row < boardSize; row++)
        {
            for (int col = 0; col < boardSize; col++)
            {
                GameObject tile = board[col, row];
                tile.GetComponent<TileCntrl>().Initialize(col, row);

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

    /**
     * ScrambleBoard() - 
     */
    public void ScrambleBoard()
    {
        int boardSize = GameManager.Instance.BoardSize;

        for (int n = 0; n < gameData.nScramble; n++)
        {
            int col = Random.Range(0, boardSize);
            int row = Random.Range(0, boardSize);

            GameObject tile = board[col, row];

            Transform parent = tile.transform;
            bool turn = Random.Range(0, 2) == 0;

            RotateTile(parent, turn);

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

        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out RaycastHit hit, 1000))
        {
            Transform parent = hit.transform.parent;

            RotateTile(parent, turn);

            MakeVarientMove(parent, turn);
        }
    }

    private void MakeVarientMove(Transform parent, bool turn)
    {
        int col = parent.GetComponent<TileCntrl>().Col;
        int row = parent.GetComponent<TileCntrl>().Row;

        switch (varientType)
        {
            case VarientType.EASY:
                break;
            case VarientType.CROSS:
                Rotate(col, row + 1, !turn);
                Rotate(col, row - 1, !turn);
                Rotate(col + 1, row, !turn);
                Rotate(col - 1, row, !turn);
                break;
        }
    }

    private void Rotate(int col, int row, bool turn)
    {
        int boardSize = GameManager.Instance.BoardSize;

        if ((col >= 0) && (col < boardSize) && (row >= 0) && (row < boardSize))
        {
            RotateTile(board[col, row].transform, turn);
        }
    }

    private void RotateTile(Transform parent, bool turn)
    {
        Rotate(parent, turn);

        Rotate(parent.Find("NorthTile_Symbol").transform, !turn);
        Rotate(parent.Find("SouthTile_Symbol").transform, !turn);
        Rotate(parent.Find("EastTile_Symbol").transform, !turn);
        Rotate(parent.Find("WestTile_Symbol").transform, !turn);
    }

    private void Rotate(Transform transform, bool turn)
    {
        if (transform.TryGetComponent<RotationCntrl>(out RotationCntrl controller))
        {
            controller.RotateTile(turn);
        }
    }
}

public enum VarientType
{
    EASY = 0,
    CROSS = 1
}
