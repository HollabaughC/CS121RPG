using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/* 
Custom JSON parser I wrote for this project 
C# does not have an innate function to parse a JSON file, therefore any file for this project must follow the format:
jsonObject.json (file)
{
    "unit" : 
    [{
        "unit": int,
        "question" : 
        [{
            "question" : int,
            "text" : string,
            "answers" : [string, string, string, string], 
            "correct" : int,
            "hint" : string
        }]
    }]
} 

to access an individual answer you would need to use data.unit[].question[].answers[]
*/
public class JSONQuestionParser : MonoBehaviour
{
    public JsonData data;
    static string jsonPath = "jsontest.json";
    static string newPath = Path.Combine(Application.streamingAssetsPath, jsonPath);

    public void Start() {
        string jsonString = File.ReadAllText(newPath);
        data = JsonUtility.FromJson<JsonData>(jsonString); //jsonUtility.FromJson() requires attributes to be case-sensitive.
    }
}



[System.Serializable]
public class Question //3. lastly, each question is split into its parts. 
{
    public int question;
    public string text;
    public List<string> answers;
    public int correct;
    public string hint;
}

[System.Serializable]
public class Unit //2. then Units are split into a List of Questions
{
    public int unit;
    public List<Question> question; 
}

[System.Serializable]
public class JsonData //1. first Json data is split into a List of Units. 
{
    public List<Unit> unit; 
}


