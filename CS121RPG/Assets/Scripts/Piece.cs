using UnityEngine;
using UnityEngine.InputSystem;

public class Piece : MonoBehaviour {

    //data needed to be referenced by the controlled piece
    //all of it hidden from editor to prevent messing up data in Unity by mistake
    public Board board {get; private set;}
    public TetrominoData data {get; private set;}
    public Vector3Int[] cells {get; private set;}
    public Vector3Int position {get; private set;}
    public int rotationIndex {get; private set;}

    public float stepDelay = 1f; //how long to wait before moving the piece down automatically
    public float lockDelay = 0.5f; //how long to wait before locking the piece in place after it lands

    private float stepTime; //time since last step
    private float lockTime; //inactive piece timer

    //initialize the piece being controlled by the player and put it at the top of the board
    public void Initialize(Board board, Vector3Int position, TetrominoData data) {

        this.board = board;
        this.position = position; //true position of piece, cells just act as an offset for drawing the shape
        this.data = data;
        this.rotationIndex = 0;

        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;

        //check if the shape is empty and init it if so
        if(this.cells == null) {

            this.cells = new Vector3Int[data.cells.Length];

        }

        //change piece shape to match the tetromino
        for(int i = 0; i < data.cells.Length; i++) {

            this.cells[i] = (Vector3Int) data.cells[i]; //cast cell data to match piece property

        }

    }

    private void Update() { //called automatically every frame

        this.board.Clear(this); //clear the piece from the board

        this.lockTime += Time.deltaTime; //increment lock timer

        if(Input.GetKeyDown(KeyCode.Z)) { //rotate piece

            Rotate(-1);

        } else if(Input.GetKeyDown(KeyCode.X)) { //rotate piece the other way

            Rotate(1);

        } if(Input.GetKeyDown(KeyCode.LeftArrow)) { //move piece left

            Move(Vector2Int.left);
            
        } else if(Input.GetKeyDown(KeyCode.RightArrow)) { //move piece right

            Move(Vector2Int.right);

        } if(Input.GetKeyDown(KeyCode.DownArrow)) { //soft drop

            Move(Vector2Int.down);
            this.stepTime = Time.time + this.stepDelay; //reset step timer since you moved down manually

        } if(Input.GetKeyDown(KeyCode.UpArrow)) { //hard drop, instantly drops piece as far as possible

            while(Move(Vector2Int.down)) {} //keep moving piece down until it can't anymore
            Lock(); //lock the piece in place instantly

        }

        if(Time.time > this.stepTime) {

            Step(); //move piece down automatically

        }

        this.board.Set(this); //draw the piece to the board

    }

    private void Step() {

        this.stepTime = Time.time + this.stepDelay; //reset step timer
        Move(Vector2Int.down); //move piece down

        //start checking for a lock
        if(this.lockTime >= this.lockDelay) {

            Lock();

        }

    }

    private void Lock() {

        this.board.Set(this);
        this.board.ClearLines(); //check for line clears
        this.board.SpawnPiece(); //spawn a new piece

    }

    //handles moving the piece
    private bool Move(Vector2Int translation) {

        Vector3Int newPosition = this.position + (Vector3Int) translation; //get new position to check if valid

        bool valid = this.board.IsValidPosition(this, newPosition); //check if new position is valid

        if(valid) {

            this.position = newPosition; //set new position
            this.lockTime = 0f; //reset lock timer

        }

        return valid;

    }

    private void Rotate(int direction) {

        int originalRotationIndex = this.rotationIndex; //store original rotation index for potential wallkick revert
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4); //change rotation index

        ApplyRotationMatrix(direction); //rotate piece

        //wallkick tests
        if(!TestWallKicks(this.rotationIndex, direction)) {

            this.rotationIndex = Wrap(this.rotationIndex - direction, 0, 4); //revert rotation index if wallkick fails
            ApplyRotationMatrix(-direction); //revert rotation if wallkick fails

        }

    }

    private void ApplyRotationMatrix(int direction) {

        float[] matrix = Data.RotationMatrix; //for easier reference

        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 cell = cells[i];

            int x, y;

            switch (data.tetromino) {
                case Tetromino.I:
                case Tetromino.O: //i and o tetriminos have rotation center offset from grid, here is their special case

                    cell.x -= 0.5f;
                    cell.y -= 0.5f;

                    x = Mathf.CeilToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));

                    break;

                default: //every other piece is centered on the grid

                    x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));

                    break;
            }

            cells[i] = new Vector3Int(x, y, 0);
        }

    }
    
    private bool TestWallKicks(int rotationIndex, int direction) {

        int wallKickIndex = GetWallKickIndex(rotationIndex, direction);

        //tests each wallkick to see if it can move to the new position
        for(int i = 0; i < this.data.wallKicks.GetLength(1); i++) {

            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];

            if(Move(translation)) {

                return true;

            }

        }

        return false;

    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection) {

        //simplifies the pattern of checking which wallkicks to test (theres 40 to check otherwise)
        int wallKickIndex = rotationIndex * 2;

        if(rotationDirection < 0) {

            wallKickIndex--;

        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));

    }

    //wraps a value around a range, honestly probably not needed but it helps with a few functions for the piece so it stays
    private int Wrap(int input, int min, int max)
    {
        if (input < min) {
            return max - (min - input) % (max - min);
        } else {
            return min + (input - min) % (max - min);
        }
    }

}
