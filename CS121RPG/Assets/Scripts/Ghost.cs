using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour {

    //vars for the piece preview
    public Tile tile;
    public Board board;
    public Piece trackingPiece;
    
    public Tilemap tilemap {get; private set;} //reference the ghost board
    public Vector3Int[] cells {get; private set;} //reference the cells of the piece
    public Vector3Int position {get; private set;} //reference the position of the piece

    private void Awake() {

        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];

    }

    //special update called after all other update functions
    private void LateUpdate() {

        Clear();
        Copy();
        Drop();
        Set();

    }

    private void Clear() { //copy of board.clear

        for(int i = 0; i < this.cells.Length; i++) {

            Vector3Int tilePosition = this.cells[i] + this.position; //get position to draw tile at
            this.tilemap.SetTile(tilePosition, null); //write tile to tilemap

        }

    }

    private void Copy() { //copy the cells of the piece to the preview

        for(int i = 0; i < this.cells.Length; i++) {

            this.cells[i] = this.trackingPiece.cells[i]; 

        }

    }

    private void Drop() { //drop the ghost piece as far as possible

        Vector3Int position = this.trackingPiece.position;

        int current = position.y;
        int bottom = -this.board.boardSize.y / 2 - 1;

        this.board.Clear(this.trackingPiece); //remove tracked piece to help with moving preview down

        for(int row = current; row >= bottom; row--) {

            position.y = row;

            if(this.board.IsValidPosition(this.trackingPiece, position)) {

                this.position = position;

            } else {

                break;

            }

        }

        this.board.Set(trackingPiece); //draw the piece back to the board

    }

    private void Set() { //copy of board.set

        for(int i = 0; i < this.cells.Length; i++) {

            Vector3Int tilePosition = this.cells[i] + this.position; //get position to draw tile at
            tilemap.SetTile(tilePosition, this.tile); //write tile to tilemap

        }

    }

}