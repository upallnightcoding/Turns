using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCntrl : MonoBehaviour
{
    [SerializeField] private int[] symbols;

    public int Col { get; private set; }
    public int Row { get; private set; }

    public bool IsCorner { get; set; } = false;

    void Awake()
    {
        symbols = new int[4];
        symbols[GameData.NORTH_TILE] = -1;
        symbols[GameData.SOUTH_TILE] = -1;
        symbols[GameData.EAST_TILE] = -1;
        symbols[GameData.WEST_TILE] = -1;
    }

    public void Initialize(int col, int row)
    {
        this.Col = col;
        this.Row = row;
    }

    public void Set(int symbolPosIndex, int symbolIndex)
    {
        symbols[symbolPosIndex] = symbolIndex;
    }

    public int GetSymbol(int symbolPosIndex)
    {
        return (symbols[symbolPosIndex]);
    }

    public void ClockWise()
    {
        Swap(GameData.NORTH_TILE, GameData.WEST_TILE);
        Swap(GameData.WEST_TILE, GameData.SOUTH_TILE);
        Swap(GameData.SOUTH_TILE, GameData.EAST_TILE);
    }

    public void CounterClockWise()
    {
        Swap(GameData.NORTH_TILE, GameData.EAST_TILE);
        Swap(GameData.EAST_TILE, GameData.SOUTH_TILE);
        Swap(GameData.SOUTH_TILE, GameData.WEST_TILE);
    }

    private void Swap(int index1, int index2)
    {
        int temp = symbols[index1];
        symbols[index1] = symbols[index2];
        symbols[index2] = temp;
    }
}
