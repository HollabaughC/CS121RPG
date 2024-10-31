using UnityEngine;
using UnityEngine.Tilemaps;

//references to piece names for future reference
public enum Tetromino {

    I,
    O,
    T,
    J,
    L,
    S,
    Z

}

//this makes the property show up in the unity editor to make it easier to link the tiles to the piece that needs to draw them
[System.Serializable]
public struct TetrominoData {

    public Tetromino tetromino;
    public Tile tile; //this is what we set in the editor
    public Vector2Int[] cells {get; private set;} //this is hidden from the editor because it was easier to just create the data manually in Data.cs
    public Vector2Int[,] wallKicks {get; private set;} //handles wall kicks for rotations, hidden from editor for same reason as cells

    //initialize the teromino data for use later
    public void Initialize() {

        this.cells = Data.Cells[this.tetromino];
        this.wallKicks = Data.WallKicks[this.tetromino];

    }

}