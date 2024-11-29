using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    public float speed = 0.75f;
    public float distance = 0.5f;
    private Vector3 startPosition;

    void Awake() {
        startPosition = transform.position;
    }

    void Update() {
        float movement = Mathf.Sin(Time.time * speed) * distance;
        Vector3 movementDirection = Vector3.zero;
        float zRotation = transform.eulerAngles.z;
        if (zRotation >= 0 && zRotation < 45 || zRotation > 315 && zRotation <= 360) {
            movementDirection = transform.up * movement;
        }
        else if (zRotation == 90) {
            movementDirection = transform.up * movement;
        }
        else {
            movementDirection = transform.up * movement;
        }
        transform.position = startPosition + movementDirection;
    }
}
