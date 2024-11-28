using UnityEngine;

public class AliceCameraMovement : MonoBehaviour
{
  public float speed = 10.0f;

  void Update() {
    if (Input.GetKey(KeyCode.UpArrow)) {
      transform.position += Vector3.up * Time.deltaTime * speed;
    }
    else if (Input.GetKey(KeyCode.DownArrow)) {
      transform.position += Vector3.down * Time.deltaTime * speed;
    }
  }
}
