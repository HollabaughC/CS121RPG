using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/*
This script is attached to an invisible option in the Main Room.
It parses the JSON responsible for studying content and breaks it down into a series of:
    -Units, which contain:
        -Study Guides (series), which contain:
            -Unit
            -Topic
            -Lessons (series), which contain:
                -Lesson
                -Text

Additionally, it also handles colliders, allowing the user to engage in studying for the current unit.
*/

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
            string jsonString = File.ReadAllText(newPath); //read the text as a string.
            data = JsonUtility.FromJson<JsonDataStudy>(jsonString); //parse the string with JsonUtility.FromJson
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
        userIn = true; // Set 'userIn' to true when the player enters the trigger zone
    }

    // Trigger event when something exits the trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player exits the trigger area
        if (other.CompareTag("Player"))
        {
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
            randomLessonText = GetRandomLessonFromUnit(currentUnit); //choose a random lesson from the unit

            // Increment the day in PlayerPrefs
            int currentDay = PlayerPrefs.GetInt("DayCount", 0); // Default to day 0 if not set
            PlayerPrefs.SetInt("DayCount", currentDay + 1); // Increment day by 1
        }
        else if(PlayerPrefs.GetInt("DayCount") % 4 == 0 && userIn && Input.GetKeyDown(KeyCode.E)) //if the user shouldn't  be able to study, and they press e while on top of the collider
        {
            string currentSceneName = SceneManager.GetActiveScene().name; //get the current scene name
            SceneManager.LoadScene(currentSceneName); //load the current scene
        }
    }

    /*
    This function is called to get a random lesson from the unit
    */
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

    /*
    OnGUI() is triggered, through the unity engine, whenever any event changes something in the GUI. 
    This function displays the lesson text if the user attempts to study.
    */
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
            float boxY = Screen.height - boxHeight - 20f; // 10px from the bottom

            // Create the text box at the calculated position
            GUI.Box(new Rect(boxX, boxY, boxWidth, boxHeight), randomLessonText);
        }
    }
}

/*
All of the following classes are used for JSON parsing, and are all serializable as they act as List containers.
*/
[System.Serializable]
public class JsonDataStudy //document is first broken into JsonStudyData
{
    public List<Study_Guide> study_guide; // List of study guides
}

[System.Serializable]
public class Study_Guide //JsonStudyData is then broken into Study_Guides
{
    public int unit; // Unit number for this study guide
    public string topic; // Topic of the study guide
    public List<Lesson> lesson; // List of lessons under this study guide
}

[System.Serializable]
public class Lesson //Study_Guides are then broken down into Lessons
{
    public int lesson; // Lesson number
    public string text; // Lesson content
}
