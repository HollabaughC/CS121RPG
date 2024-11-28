using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
public class StudyJSONParser : MonoBehaviour
{
    public JsonDataStudy data;
    string jsonPath = "Assets/Scripts/studyjson.json";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start() {
        string jsonString = File.ReadAllText(jsonPath);
        data = JsonUtility.FromJson<JsonDataStudy>(jsonString); //jsonUtility.FromJson() requires attributes to be case-sensitive.
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
