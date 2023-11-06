using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCntrl : MonoBehaviour
{
    [SerializeField] private int[] symbols;

    private int col = -1;
    private int row = -1;

    void Awake()
    {
        symbols = new int[4];
        symbols[GameData.NORTH_TILE] = -1;
        symbols[GameData.SOUTH_TILE] = -1;
        symbols[GameData.EAST_TILE] = -1;
        symbols[GameData.WEST_TILE] = -1;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(int col, int row)
    {
        this.col = col;
        this.row = row;
    }

    public void Set(int symbolPosIndex, int symbolIndex)
    {
        symbols[symbolPosIndex] = symbolIndex;
    }

    public int GetSymbol(int symbolPosIndex)
    {
        return (symbols[symbolPosIndex]);
    }
}
