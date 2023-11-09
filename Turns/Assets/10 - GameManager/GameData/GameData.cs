using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="GameData", menuName = "Turns/Game Data")]
public class GameData : ScriptableObject
{
    public static int NORTH_TILE    = 0;
    public static int EAST_TILE     = 1;
    public static int SOUTH_TILE    = 2;
    public static int WEST_TILE     = 3;

    [Header("Board Attributes")]
    public int boardSize;
    public GameObject parent;
    public GameObject turnTilePreFab;
    public GameObject[] turnTileSymbolsPreFab;

    public string[] symbolName;

    public int nScramble;

    public float[] cameraPosition;
}
