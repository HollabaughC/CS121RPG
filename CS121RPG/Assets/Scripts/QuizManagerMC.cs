using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

/*
This script is attached to a hidden GameObject in the Quiz game that controls the main moving parts of the scene.    
*/

public class QuizManagerMC : MonoBehaviour
{
    public JSONQuestionParser JQP;
    public GameObject[] options = new GameObject[4];
    public List<int> qIndexOptions;
    public int qIndex;
    public int uIndex = 0;
    public Text qText;
    public GameObject hintPanel;
    public int hintCount = 3;
    public int bonusQuestions = 3;
    public LifeCount lifeCount;
    
    /*
    Start() is called when the scene is initialized, as the QuizManager is an object that is enabled in the scene as it opens.
    */

    void Start(){
        JQP.Start(); //Calling the Start() function of the JQP object to account for asychronous Start() timings occaisionally not populating the JQP before being used.
        lifeCount.livesLeft = 3; //Set the amount of lives for the LifeCount.cs
        hintPanel.SetActive(false); //The HintPanel is enabled by default, so this hides it.
        hintCount = PlayerPrefs.GetInt("Hint"); //Sets hint count to the PlayerPref number, overwriting the default setting
        uIndex = PlayerPrefs.GetInt("Unit"); //Sets hint count to the PlayerPref number, overwriting the default setting
        bonusQuestions = 3; //Resets the bonusQuestions each time the scene is loaded.
        generateQuestionList(); //Starts running the game.
    }

    /*
    correct() is called by MCAnswers.cs when the user selects a correct answer. It generates a new question or bonus question, and if the user has completed everything,
    it updates the PlayerPrefs and send the user back to the Main Room.
    */
    public void correct(){
        if(qIndexOptions.Count > 0) //remove the current question from the list of index options.
            qIndexOptions.RemoveAt(qIndex);
        if(qIndexOptions.Count > 0) //if there are still more options for the user this unit, ask another question.
            generateQuestions();
        else { //after the user has completed the unit...
            if(bonusQuestions > 0 && uIndex >= 1){ //if they have completed at least one unit, and have more bnous questions, ask a bonus question.
                bonusQuestions--;
                generateQuestions();
            }
            else{ //this condition is only hit if the user completeed the full quiz, with bonus questions (if applicable).
                PlayerPrefs.SetInt("Hint", hintCount); //Update the PlayerPref hint count
                PlayerPrefs.SetInt("Unit", ++uIndex); //Update the PlayerPref unit, incrementing it
                PlayerPrefs.SetInt("QuizDone", 1); //Update the PlayerPref QuizDone (which acts like a boolean for the Drag/Drop minigame)
                if(uIndex % 4 != 0){ //Every 4 units, the Drag/Drop game should be triggered, which requires the day not to be incremented.
                    PlayerPrefs.SetInt("DayCount", PlayerPrefs.GetInt("DayCount") + 1); //increment the PlayerPref DayCount
                }
                SceneManager.LoadScene("SampleScene"); //Load the Main Room
            }
        }
    }
    
    /*
    This function is used to set the text for answer buttons, and set the correct answer.
    */
    void setAnswers(int unit, int question) {
        if(qIndexOptions.Count == 0){ //This is used for bonus questions
            for(int i = 0; i < options.Length; i++){ // for each answer
                options[i].GetComponent<MCAnswers>().isCorrect = false; //set it to false by default
                options[i].transform.GetChild(0).GetComponent<Text>().text = JQP.data.unit[unit].question[question].answers[i]; //get the correct answer text from the unit and question
                if(i == JQP.data.unit[unit].question[question].correct){ //set the correct answer to true.
                    options[i].GetComponent<MCAnswers>().isCorrect = true;
                }
            }
        }
        else { //For non-bonus questions
            for(int i = 0; i < options.Length; i++){
                options[i].GetComponent<MCAnswers>().isCorrect = false; //set all of the answers to false intially.
                options[i].transform.GetChild(0).GetComponent<Text>().text = JQP.data.unit[unit].question[question].answers[i]; //Update it's text
                if(i == JQP.data.unit[unit].question[question].correct){ //if this answer is correct, set it to correct.
                    options[i].GetComponent<MCAnswers>().isCorrect = true;
                }
            }
        }
    }
    /*
    This function is used to generate the current question, it checks if the user is on bonus questions, and gnerates accoridingly.
    */
    void generateQuestions() {
        if(qIndexOptions.Count == 0){ //This checks if the user is on bonus questions.
            var qData = getExtraQuestionNums(); //var type is used here as qData has two values like a python tuple.
            qText.text = JQP.data.unit[qData.unit].question[qData.question].text; //set the question text based on the returned results.
            setAnswers(qData.unit, qData.question); //Set the answers based on qData's unit and question.
        }
        else {
            qIndex = Random.Range(0, qIndexOptions.Count-1); //Generate a random index for qIndexOptions (of remaining indexes).
            qText.text = JQP.data.unit[uIndex].question[qIndexOptions[qIndex]].text; //Update the questions text.
            setAnswers(uIndex, qIndexOptions[qIndex]); //Set the answers and add correct functionality.
        }
    }
    /*
    This function is called when the scene starts, and creates a list of index options for the question bank (before bonus).
    The index option are used to randomize the questions, as they cannot be removed from the JSON file so they can be recycled for future units.
    */
    void generateQuestionList() {
        qIndexOptions = new List<int>();
        for(int i = 0; i < JQP.data.unit[uIndex].question.Count; i++){
            qIndexOptions.Add(i);
        }
        generateQuestions(); //After the list of options is generated, choose a question.        
    }

    /*
    This function is called when the user clicks on the hint button, it reduces the hint count and makes the hint appear.
    */
    public void getHint() {
        hintCount--;
        hintPanel.transform.GetChild(0).GetComponent<Text>().text = JQP.data.unit[uIndex].question[qIndexOptions[qIndex]].hint; //Change the text component of the hintPanel
        hintPanel.SetActive(true); //Enable the panel.
        hintPanel.GetComponent<Button>().onClick.AddListener(() => {Debug.Log("clicked"); hintPanel.SetActive(false);}); //Disable the panel on click
    }

    /*
    This function is used for creating a set of extra questions with a similar data structure to python's tuple.
    */
    public (int unit, int question) getExtraQuestionNums() {
        int unit = getUnit(); //get the unit of the question using normalization
        int question = Random.Range(0,5); //pick a random question
        return (unit, question); 
    }

    /*
    This function is used to generate each unit of the bonus questions.
    It uses normalization to give higher priority to the most recent units, with lower chances for beginning units.
    */
    public int getUnit() {
        int[] units = Enumerable.Range(1, uIndex).ToArray(); //Generate an array of past units. Range is not inclusive of the second value.
        int roll = Random.Range(1,101); //Choose a random number between 1 and 100.
        int sum = 0; //Sum of the units, used for normalization
        for(int i = 0; i < units.Length; i++){
            sum += units[i];
        }
        float cumsum = 0.0f; //cumulative summation is used to see which "bracket" the roll falls into.
        for(int i = 0; i < units.Length; i++){
            float temp = (float)units[i]/sum * 100; //temp is representative of the normalized value of the current unit.
            cumsum += temp; //temp is added to cumsum, which is used to check against roll.
            if (cumsum > roll) { //if the cumulative summation is higher than the roll, return the current value (as the unit).
                return i;
            }
        }
        return units.Length - 1; //if nothing is found, return the latest unit, although this should never be hit.
    }

    /*
    This function is used to make the player lose a life if they click on the wrong answer.
    */
    public void loseLife(){
        lifeCount.loseLife();
    }

    /*
    This function is only called if the player loses all of their lives. It sends them back to the main scene and updates the hint count and unit to be representative of their current values.
    */
    public void loseGame(){
        PlayerPrefs.SetInt("Hint", hintCount);
        PlayerPrefs.SetInt("Unit", uIndex);
        SceneManager.LoadScene("SampleScene");
    }

    /*
    Update is called by the UnityEngine every frame. This is just used to allow the player to exit the game when pressing ESC.
    */
    void Update() {

        if(Input.GetKeyDown(KeyCode.Escape)) { //exit the game

            Application.Quit();

        }
        
    }

}
