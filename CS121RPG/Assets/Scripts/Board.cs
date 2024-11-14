using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using JetBrains.Annotations;

//helps with handling drawing to the tilemap and handling game loop logic
public class Board : MonoBehaviour {
    //get tilemap and tetromino data
    public Tilemap tilemap {get; private set;} //hidden from editor so it does not get changed as easily
    public Piece activePiece {get; private set;}
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPosition; //where pieces spawn at the top, will be set in editor
    public Vector2Int boardSize = new Vector2Int(10, 20); //size of the board, helps with bounds checking
    private int[] bag = {0, 1, 2, 3, 4, 5, 6}; //bag of tetrominos to spawn (14 bag system)
    private int bagIndex = 0; //current index in the bag

    //game vars
    public int score; //player score, lacks the getter and setter definitions so other scripts can change it
    public int level {get; private set;} //player level
    public int lines {get; private set;} //player lines cleared
    
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

        score = 0;
        level = 1;
        lines = 0;
    
        for(int i = 0; i < this.tetrominos.Length; i++) {
    
            this.tetrominos[i].Initialize();
    
        }
    
    }
    
    //run as soon as the scene is loaded, used to just immediately spawn a piece
    private void Start() {
    
        ShuffleBag();
        SpawnPiece();
    
    }

    private void Update() {

        //update ui elements
        GameObject canvas = GameObject.Find("Canvas");
        
        canvas.transform.Find("Score/ScoreText").GetComponent<Text>().text = this.score.ToString();
        canvas.transform.Find("Lines/LinesText").GetComponent<Text>().text = this.lines.ToString();
        canvas.transform.Find("Level/LevelText").GetComponent<Text>().text = this.level.ToString();

    }
    
    //handles spawning a random piece at the top of the board
    public void SpawnPiece() {
    
        TetrominoData data = this.tetrominos[bag[bagIndex]];
        this.activePiece.Initialize(this, this.spawnPosition, data, (float) Math.Pow(0.8 - ((level - 1) * 0.007), level - 1)); //initialize the piece
    
        bagIndex += 1;
        if(bagIndex >= this.bag.Length) {
            bagIndex = 0;
            ShuffleBag();
        }

        if(IsValidPosition(this.activePiece, this.activePiece.position)) {
    
            Set(this.activePiece);
    
        } else {
    
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reload the scene if player loses
    
        }
    
    }

    //does what it says on the tin, shuffles the next bag of 14 tetrominos to be given to the player
    private void ShuffleBag() { //fisher-yates shuffle
    
        for (int i = bag.Length - 1; i > 0; i--) {

            int j = UnityEngine.Random.Range(0, i + 1);
            int temp = bag[i];
            bag[i] = bag[j];
            bag[j] = temp;

        }

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

        int oldLines = this.lines; //store old lines cleared
    
        while(row < bounds.yMax) {
    
            if(IsLineFull(row)) { //check if the line is full
    
                this.lines++; //increment lines cleared
                this.level = (int)Math.Floor((double) lines / 10) + 1; //increment level

                ClearLine(row); //clear the line
    
            } else {
    
                row++; //move up a row
    
            }
    
        }

        switch(this.lines - oldLines) {
            case 1:
                this.score += 40 * this.level;
                break;
            case 2:
                this.score += 100 * this.level;
                break;
            case 3:
                this.score += 300 * this.level;
                break;
            case 4:
                this.score += 1200 * this.level;
                break;
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