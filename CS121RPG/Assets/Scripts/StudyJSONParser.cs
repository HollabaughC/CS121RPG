using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
public class StudyJSONParser : MonoBehaviour
{
    public JsonDataStudy data;
    string jsonPath = "Assets/Scripts/studyjson.json";
    bool userIn = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start() {
        string jsonString = File.ReadAllText(jsonPath);
        data = JsonUtility.FromJson<JsonDataStudy>(jsonString); //jsonUtility.FromJson() requires attributes to be case-sensitive.
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("user entered!");
        userIn = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player exits the trigger area
        if (other.CompareTag("Player"))
        {
            Debug.Log("player left :(");
            userIn = false;
        }
    }
    void Update() {
        if(PlayerPrefs.GetInt("DayCount") % 4 != 0){
            if(userIn && Input.GetKeyDown(KeyCode.E)){
                Debug.Log("#Studyinnnn");
            }
        }
    }
    
}

[System.Serializable]
public class JsonDataStudy
{
    public List<Study_Guide> study_guide; 
}

[System.Serializable]
public class Study_Guide {
    public int unit;
    public string topic;
    public List<Lesson> lesson;
}

[System.Serializable]
public class Lesson {
    public int lesson;
    public string text;
}
