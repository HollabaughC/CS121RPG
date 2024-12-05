using UnityEngine;

/*
This script is attached to the Camera for the Drag and Drop/Alice-like game. It allows for the camera to scroll up and down while the user is playing the game.
*/
public class AliceCameraMovement : MonoBehaviour
{
  public float speed = 10.0f; 

  void Update() { //Update() is automatically called every frame. 
    if (Input.GetKey(KeyCode.UpArrow)) { //This condition is triggered if there is user input, such as a key being pressed, and seeing if the button is the Up Arrow
      transform.position += Vector3.up * Time.deltaTime * speed; 
      /* 
      transform.position is the position of the camera. 
      Even though we are a 2D game, all objects exist in a 3D space, which is why we use Vector3. Vector3.Up sets the direction.
      Time.deltaTime deals with seconds, as opposed to frames, I believe it is the difference from the last time it was called (which would be every frame).
      */
    }
    else if (Input.GetKey(KeyCode.DownArrow)) {
      transform.position += Vector3.down * Time.deltaTime * speed;
    }
  }
}
