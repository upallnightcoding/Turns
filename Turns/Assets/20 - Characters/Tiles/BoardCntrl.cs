using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private Transform boardParent;

    private GameObject[,] board;

    // Start is called before the first frame update
    void Start()
    {
        CreateBoard(boardParent);
        ScrambleBoard();
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

    private void CreateBoard(Transform parent)
    {
        board = new GameObject[gameData.boardSize, gameData.boardSize];

        for (int row = 0; row < gameData.boardSize; row++)
        {
            for (int col = 0; col < gameData.boardSize; col++)
            {
                Vector3 position = new Vector3(col * 5 - 5, 0.0f, row * 5 - 5);
                GameObject tilePreFab = gameData.turnTilePreFab;
                GameObject tile = Instantiate(tilePreFab, position, Quaternion.identity);
                tile.GetComponent<TileCntrl>().Initialize(col, row);
                board[col, row] = tile;
                //tile.transform.SetParent(parent);

                CreateTileSymbols(tile, GameData.NORTH_TILE, GetSymbol());
                CreateTileSymbols(tile, GameData.EAST_TILE, GetSymbol());
            }
        }

        for (int row = 0; row < gameData.boardSize; row++)
        {
            for (int col = 0; col < gameData.boardSize; col++)
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

    public void ScrambleBoard()
    {
        for (int n = 0; n < gameData.nScramble; n++)
        {
            int col = Random.Range(0, gameData.boardSize);
            int row = Random.Range(0, gameData.boardSize);

            GameObject tile = board[col, row];
            
            RotateTile(tile.transform, Random.Range(0, 2) == 0);
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

    private int GetSymbol()
    {
        return(Random.Range(0, gameData.turnTileSymbolsPreFab.Length));
    }

    private void RotateTile(bool turn)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out RaycastHit hit, 1000))
        {
            Transform parent = hit.transform.parent;

            RotateTile(parent, turn);
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
