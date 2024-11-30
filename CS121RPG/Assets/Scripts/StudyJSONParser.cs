using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class StudyJSONParser : MonoBehaviour
{
    public JsonDataStudy data;
    private string jsonPath = "Assets/Scripts/studyjson.json";
    private string randomLessonText = "";

    public void Start() 
    {
        // Load and parse JSON data
        if (File.Exists(jsonPath))
        {
            string jsonString = File.ReadAllText(jsonPath);
            data = JsonUtility.FromJson<JsonDataStudy>(jsonString);
            Debug.Log("JSON Loaded Successfully.");
        }
        else
        {
            Debug.LogError("JSON file not found at " + jsonPath);
        }
    }

    private void Update()
    {
        // Check if the 'E' key is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Get the current unit from PlayerPrefs
            int currentUnit = PlayerPrefs.GetInt("Unit", 1); // Default to unit 1 if not set
            randomLessonText = GetRandomLessonFromUnit(currentUnit);
            Debug.Log("Random Lesson Selected: " + randomLessonText);

            // Increment the day in PlayerPrefs
            int currentDay = PlayerPrefs.GetInt("Day", 0); // Default to day 0 if not set
            PlayerPrefs.SetInt("Day", currentDay + 1); // Increment day by 1
            Debug.Log("Day incremented: " + (currentDay + 1));
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
            GUI.Box(new Rect(boxX, boxY, boxWidth, boxHeight), "Random Lesson: " + randomLessonText);
        }
    }
}

[System.Serializable]
public class JsonDataStudy
{
    public List<Study_Guide> study_guide;
}

[System.Serializable]
public class Study_Guide 
{
    public int unit;
    public string topic;
    public List<Lesson> lesson;
}

[System.Serializable]
public class Lesson 
{
    public int lesson;
    public string text;
}
