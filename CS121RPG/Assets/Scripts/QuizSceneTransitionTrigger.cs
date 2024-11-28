using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene transitions

public class QuizSceneTransitionTrigger : MonoBehaviour
{
    private bool playerInRange = false; // Tracks if the player is in range
    public GameController game;

    void Update()
    {
        if((PlayerPrefs.GetInt("DayCount") % 4 == 0) && (PlayerPrefs.GetInt("QuizDone") == 0)){ //only activates on a day that is a multiple of 4, and if quiz is not done
            // If the player is in range and presses the E key
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene("MCQuestions"); // Load the MCQuestions scene
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the trigger area
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player exits the trigger area
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
