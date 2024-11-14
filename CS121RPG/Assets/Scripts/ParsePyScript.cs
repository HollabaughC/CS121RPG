using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ParsePyScript : MonoBehaviour
{
    List<Line> Lines = new List<Line>();
    public string filePath = "Assets/Scripts/pythontest.py";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        readFile();
    }

    void readFile(){
        try {
            using (StreamReader reader = new StreamReader(filePath)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    //Line lineObject = new Line(line);
                    //Lines.Add(lineObject);
                    //Debug.Log(lineObject.printInfo());
                    Lines.Add(new Line(line));
                }
            }
        }catch (Exception e) {
            Debug.Log($"There was an issue with your file: {e.Message}");
        }
        foreach(Line line in Lines) {
            Debug.Log(line.printInfo());
        }
    }


}

[System.Serializable]
public class Line {
    string text;
    int indentLevel = 0;
    bool comment = false;

    public Line(string input){
        indentLevel = getIndent(input);
        text = input.TrimStart();
        if(text.Length > 0 && text[0] == '#')
            comment = true;
    }
    int getIndent(string line) {
        int count = 0;
        foreach (char c in line) {
            if (c == ' ') {
                count++;
            }
            else if (c == '\t') {
                count += 4;  
            }
            else {
                break; 
            }
        }
        int level = count / 4;
        return level;
    }
    public string printInfo() {
        string output = "";
        output = text;
        output += (" Indent Level: " + indentLevel);
        if(comment){
            output += " I am a comment too!";
        }
        return output;
    }
}
