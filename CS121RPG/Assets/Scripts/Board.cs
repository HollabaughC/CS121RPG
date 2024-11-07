using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
//helps with handling drawing to the tilemap and handling game loop logic
public class Board : MonoBehaviour {
    //get tilemap and tetromino data
    public Tilemap tilemap {get; private set;} //hidden from editor so it does not get changed as easily
    public Piece activePiece {get; private set;}
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPosition; //where pieces spawn at the top, will be set in editor
    public Vector2Int boardSize = new Vector2Int(10, 20); //size of the board, helps with bounds checking
    public RectInt bounds {
        
        get {
        
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        
        }
    
    }
    //acts as initializer for tilemap used to call the tetrominos init functions
    private void Awake() {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        for(int i = 0; i < this.tetrominos.Length; i++) {
            this.tetrominos[i].Initialize();
        }
    }
    //run as soon as the scene is loaded, used to just immediately spawn a piece
    private void Start() {
        SpawnPiece();
    }
    //handles spawning a random piece at the top of the board
    public void SpawnPiece() {
        //picks a piece at random (will be changed to a 14-bag system later) and loads its data
        int random = Random.Range(0, this.tetrominos.Length);
        TetrominoData data = this.tetrominos[random];
        this.activePiece.Initialize(this, this.spawnPosition, data);
        Set(this.activePiece);
    }
    //draws the pieces to the tilemap
    public void Set(Piece piece) {
        for(int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position; //get position to draw tile at
            this.tilemap.SetTile(tilePosition, piece.data.tile); //write tile to tilemap
        }
    }
    //clears a piece from tilemap, used when moving or rotating
    public void Clear(Piece piece) {
        for(int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position; //get position to draw tile at
            this.tilemap.SetTile(tilePosition, null); //write tile to tilemap
        }
    }
    public bool IsValidPosition(Piece piece, Vector3Int position) {
        
        RectInt bounds = this.bounds; //get board bounds
        for(int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + position; //get new position to check if valid
            if(!bounds.Contains((Vector2Int) tilePosition)) { //check if tile is out of bounds
                return false;                
                
            } if(this.tilemap.HasTile(tilePosition)) { //check if tile is already occupied
                return false;
            }
        }
        return true;
    }
    public void ClearLines() {
        RectInt bounds = this.bounds; //get board bounds
        int row = bounds.yMin; //start at the bottom of the board
        while(row < bounds.yMax) {
            if(IsLineFull(row)) { //check if the line is full
                ClearLine(row); //clear the line
            } else {
                row++; //move up a row
            }
        }
    }
    private bool IsLineFull(int row) {
        RectInt bounds = this.bounds; //get board bounds
        for(int col = bounds.xMin; col < bounds.xMax; col++) {
            Vector3Int tilePosition = new Vector3Int(col, row, 0); //get position to check
            if(!this.tilemap.HasTile(tilePosition)) { //check if tile is occupied
                return false;
            }
        }
        return true;
    }
    private void ClearLine(int row) {
        RectInt bounds = this.bounds; //get board bounds
        for(int col = bounds.xMin; col < bounds.xMax; col++) {
            Vector3Int tilePosition = new Vector3Int(col, row, 0); //get position to check
            this.tilemap.SetTile(tilePosition, null); //clear tile
        }
        //move all upper rows down
        while(row < bounds.yMax) {
            for(int col = bounds.xMin; col < bounds.xMax; col++) {
                Vector3Int tilePosition = new Vector3Int(col, row + 1, 0); //get position to check
                TileBase tile = this.tilemap.GetTile(tilePosition); //get tile to move
                this.tilemap.SetTile(tilePosition, null); //clear tile
                this.tilemap.SetTile(tilePosition + Vector3Int.down, tile); //move tile down
            }
            row++; //move up a row
        }
    }
}