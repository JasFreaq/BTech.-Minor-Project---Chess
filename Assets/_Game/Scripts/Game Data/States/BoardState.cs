using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState
{
    public BoardState()
    {
        Board = new SquareState[8, 8];
    }

    public SquareState[,] Board { get; set; }
}
