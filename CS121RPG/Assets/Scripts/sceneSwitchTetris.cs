using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // This method is for 3D collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Checks if colliding object has "Player" tag
        {
            Debug.Log("Player entered trigger!");
            SceneManager.LoadScene("PuzzleGame");  // Directly loads "PuzzleGame" scene
        }
    }

    // This method is for 2D collisions
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Checks if colliding object has "Player" tag
        {
            Debug.Log("Player entered trigger!");
            SceneManager.LoadScene("PuzzleGame");  // Directly loads "PuzzleGame" scene
        }
    }
}
