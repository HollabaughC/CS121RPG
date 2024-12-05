using UnityEngine;

/*
This script is attached to Arrow prefabs, which are Instantiated (spawned) in the Main Room, it allows them to move back and forth, relative to their position.    
*/
public class MoveArrow : MonoBehaviour
{
    public float speed = 0.75f;
    public float distance = 0.5f;
    private Vector3 startPosition;

    /*
    Awake() is called wen the prefab is spawned, it sets the arrow's start position.
    */
    void Awake() {
        startPosition = transform.position;
    }

    /*
    Update is called automatically every frame, this function sets the movement to be back or forth (depending on sin(time) value) and moves the arrow according to its rotation.
    */
    void Update() {
        float movement = Mathf.Sin(Time.time * speed) * distance; //find the arrow's movement based off a sin(time) function
        Vector3 movementDirection = Vector3.zero; //initializes the full movement vector
        float zRotation = transform.eulerAngles.z; //finds the z axis rotation for the arrow.
        if (zRotation >= 0 && zRotation < 45 || zRotation > 315 && zRotation <= 360) { //if is it facing up or down, set the angle
            movementDirection = transform.up * movement;
        }
        else if (zRotation == 90) { //if it is facing right, set the angle
            movementDirection = transform.up * movement;
        }
        else { //if it is facing left, set the angle.
            movementDirection = transform.up * movement;
        }
        transform.position = startPosition + movementDirection; //move the arrow.
    }
}
