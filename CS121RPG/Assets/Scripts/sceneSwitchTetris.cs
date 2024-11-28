using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public GameController game; //Added for dayCount validation
    // This method is for 3D collisions
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Day: " + PlayerPrefs.GetInt("DayCount"));
        if(PlayerPrefs.GetInt("DayCount") % 4 != 0){ //Check if the cycle of the day is not a quiz day.
            if (other.CompareTag("Player"))  // Checks if colliding object has "Player" tag
            {
                Debug.Log("Player entered trigger!");
                SceneManager.LoadScene("PuzzleGame");  // Directly loads "PuzzleGame" scene
            }
        }
        else if (PlayerPrefs.GetInt("QuizDone") == 1) { //the day only remains a multiple of 4 if the quiz is done and the unit is divisible by 4
            if (other.CompareTag("Player")) {
                SceneManager.LoadScene("Wonderland");
            }
        }
    }

    // This method is for 2D collisions
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Day: " + PlayerPrefs.GetInt("DayCount"));
        if(PlayerPrefs.GetInt("DayCount") % 4 != 0) {
            if (other.CompareTag("Player"))  // Checks if colliding object has "Player" tag
            {
                Debug.Log("Player entered trigger!");
                SceneManager.LoadScene("PuzzleGame");  // Directly loads "PuzzleGame" scene
            }
        }
        else if (PlayerPrefs.GetInt("QuizDone") == 1) { //if day is multiple of 4, unit is multiple of 4, and quiz is done
            if (other.CompareTag("Player")) {
                SceneManager.LoadScene("Wonderland");
            }
        }
    }
}
