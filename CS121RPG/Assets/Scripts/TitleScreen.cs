using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    void Update()
    {
        // Check if the Enter key (Return key) is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Load the scene named "SampleScene"
            SceneManager.LoadScene("SampleScene");
        }
    }
}
