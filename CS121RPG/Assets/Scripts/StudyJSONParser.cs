using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StudyJSONParser : MonoBehaviour
{
    public JsonDataStudy data;
    static string jsonPath = "studyjson.json"; // The JSON file path
    static string newPath = Path.Combine(Application.streamingAssetsPath, jsonPath);
    private string randomLessonText = "";
    private bool userIn = false; // Declare the 'userIn' variable here

    // Start is called once before the first frame update
    public void Start() 
    {
        // Load and parse JSON data
        if (File.Exists(newPath))
        {
            
            string jsonString = File.ReadAllText(newPath);
            data = JsonUtility.FromJson<JsonDataStudy>(jsonString);
            Debug.Log("JSON Loaded Successfully.");
        }
        else
        {
            Debug.LogError("JSON file not found at " + newPath);
        }
    }

    // Trigger event when something enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("user entered!");
        userIn = true; // Set 'userIn' to true when the player enters the trigger zone
    }

    // Trigger event when something exits the trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player exits the trigger area
        if (other.CompareTag("Player"))
        {
            Debug.Log("player left :(");
            userIn = false; // Set 'userIn' to false when the player exits the trigger zone
        }
    }

    // Update is called once per frame
    void Update() 
    {
        // Ensure the day count is not a multiple of 4 and the player is inside the trigger
        if (PlayerPrefs.GetInt("DayCount") % 4 != 0 && userIn && Input.GetKeyDown(KeyCode.E))
        {
            // Get the current unit from PlayerPrefs
            int currentUnit = PlayerPrefs.GetInt("Unit", 1); // Default to unit 1 if not set
            randomLessonText = GetRandomLessonFromUnit(currentUnit);
            Debug.Log("Random Lesson Selected: " + randomLessonText);

            // Increment the day in PlayerPrefs
            int currentDay = PlayerPrefs.GetInt("DayCount", 0); // Default to day 0 if not set
            PlayerPrefs.SetInt("DayCount", currentDay + 1); // Increment day by 1
            Debug.Log("Day incremented: " + (currentDay + 1));
        }
        else if(PlayerPrefs.GetInt("DayCount") % 4 == 0 && userIn && Input.GetKeyDown(KeyCode.E))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }

    private string GetRandomLessonFromUnit(int unit)
    {
        if (data != null && data.study_guide != null)
        {
            // Find all study guides matching the current unit
            List<Study_Guide> unitGuides = data.study_guide.FindAll(guide => guide.unit == unit);

            if (unitGuides.Count > 0)
            {
                // Select a random study guide for the current unit
                Study_Guide randomGuide = unitGuides[Random.Range(0, unitGuides.Count)];

                // If the selected guide has lessons, pick a random lesson
                if (randomGuide.lesson.Count > 0)
                {
                    Lesson randomLesson = randomGuide.lesson[Random.Range(0, randomGuide.lesson.Count)];
                    return randomLesson.text; // Return the lesson text
                }
            }
        }
        return "No lessons found for the current unit.";
    }

    private void OnGUI() 
    {
        if (!string.IsNullOrEmpty(randomLessonText))
        {
            // Calculate the size of the text
            GUIStyle labelStyle = GUI.skin.label;
            Vector2 textSize = labelStyle.CalcSize(new GUIContent(randomLessonText));

            // Set a minimum width for the box and adjust it based on the text size
            float boxWidth = Mathf.Max(textSize.x + 20f, 300f);  // Minimum width of 300
            float boxHeight = textSize.y + 20f;  // Add padding around the text

            // Set the position of the box at the bottom middle of the screen
            float boxX = (Screen.width - boxWidth) / 2;
            float boxY = Screen.height - boxHeight - 10f; // 10px from the bottom

            // Create the text box at the calculated position
            GUI.Box(new Rect(boxX, boxY, boxWidth, boxHeight), randomLessonText);
        }
    }
}

[System.Serializable]
public class JsonDataStudy
{
    public List<Study_Guide> study_guide; // List of study guides
}

[System.Serializable]
public class Study_Guide 
{
    public int unit; // Unit number for this study guide
    public string topic; // Topic of the study guide
    public List<Lesson> lesson; // List of lessons under this study guide
}

[System.Serializable]
public class Lesson 
{
    public int lesson; // Lesson number
    public string text; // Lesson content
}
