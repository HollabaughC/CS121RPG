using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

/* 
This Script is attached to a gameObject in the Drag/Drop (Alice-like) scene. The purpose is to take a .py file and parse it into a series of Line objects.
The Line Objects contain:
    -The text content (string)
    -The level of indent (int)
    -Whether or not the line is a comment (boolean)
*/

public class ParsePyScript : MonoBehaviour
{
    List<Line> Lines;
    static string filePath = "pythontest.py"; //the name of the file itself is given as it is combined with the StreamingAssets path below.
    static string newPath = Path.Combine(Application.streamingAssetsPath, filePath); //StreamingAssets is a path that Unity can set after building the .exe, a relative path is messed up normally.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start() {
        Lines = new List<Line>();
        readFile();
    }

    /* 
    This function uses StreamReader to read in the file, and parse it into Line objects.
    */
    void readFile(){
        try {
            using (StreamReader reader = new StreamReader(newPath)) {
                string line;
                while ((line = reader.ReadLine()) != null) { //an empty line would not be null, so this goes until the end of the file.
                    if(!String.IsNullOrEmpty(line)){ //IsNullOrEmpty would allow this to skip empty lines in the file.
                        Lines.Add(new Line(line)); //Create a new Line object and add it to the list.
                    }
                }
            }
        }catch (Exception e) {
            Debug.Log($"There was an issue with your file: {e.Message}");
        }
    }
    /*
    This function is public and is referenced in other scripts to read the file.
    */
    public List<Line> GetLines() {
        return Lines; 
    }


}

/*
This class is Serializable so that it can be added to a List. It is the constructor for the Line Object and used similarly to a factory.    
*/
[System.Serializable]
public class Line {
    public string text;
    public int indentLevel = 0;
    public bool comment = false;

    /*
    This constructor takes text input and creates the object.
    */
    public Line(string input){
        indentLevel = getIndent(input); 
        text = input.TrimStart();
        if(text.Length > 0 && text[0] == '#') //if the first character is a "#", python's comment character. 
            comment = true;
    }

    /*
    getIndent() counts the number of white spaces in front of a line, and returns it's index level based on how many tabs would fit in it.
    */
    int getIndent(string line) {
        int count = 0;
        foreach (char c in line) {
            if (c == ' ') { //some files seem to use an indent as 4 spaces, others use it as \t, so these if conditions are used to assess that. 
                count++;
            }
            else if (c == '\t') {
                count += 4;  
            }
            else {
                break; 
            }
        }
        int level = count / 4; //int num = intNum / intNum2 returns the truncated quotient of the division problem.
        return level;
    }

    /*
    This function was used for testing purposes in the editor and has no output for the user of the executable.
    For each line it was used to print the contents, indent level, and whether or not it was a comment.
    */
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
