using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class BoardState
{
    static readonly string SAVE_FOLDER = Application.dataPath + "/_Game/Resources/";

    public BoardState()
    {
        Board = new TileState[8, 8];
    }
    
    public BoardState(TileState[,] boardStates)
    {
        Board = boardStates;
    }

    public TileState[,] Board { get; set; }
}
