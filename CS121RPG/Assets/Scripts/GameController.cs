using UnityEngine;
using UnityEngine.SceneManagement;

/*
This script controls the Main Room scene of the game, it is attached to a hidden GameObject within that scene.    
*/

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public GameObject arrow;
    public int hint; //starts at 3

    public int unit;
    public int aliceUnit;
    public int day; //start at 1

    public int tetris_high_score;
    public string option;
    public int studyCount;

    /*
    Awake() is invoked automatically once the Object is added, in this case, when the Scene is loaded. 
    */
    private void Awake() {
        //Lines 22, 32, and 34 are used to set the initial values of PlayerPrefs (used like global variables). "Init" serves to save the user's progress each time the scene is loaded.
        if(PlayerPrefs.GetInt("Init") != 13) { //This is only ran the first time the user starts up the game.
            PlayerPrefs.SetInt("Hint", hint); //set the hint count (initally to 3).
            PlayerPrefs.SetInt("Unit", unit); //set the unit count (initially 0).
            PlayerPrefs.SetInt("QuizDone", 0); //boolean value for whether or not the quiz was done.
            PlayerPrefs.SetInt("TetrisScore", tetris_high_score); //set high_score of tetris, (initially 0).
            PlayerPrefs.SetInt("AliceUnit", 0); //set Drag/Drop (Alice) unit to 0.
            PlayerPrefs.SetInt("DayCount", day); //set the current day the player is on (initially 1).
            PlayerPrefs.SetInt("HintThreshold", 1); //sets the amount of times the player needs to study to increase their hintcount (initially 1).
            PlayerPrefs.SetInt("StudyCount", 0); //sets the amount of times the player has studied since gaining their last hint (initally 0).
            PlayerPrefs.SetString("OptionChosen", ""); //creates an empty string for the game to later check if "study" or "game" was chosen.
        }
        StartDay(); //Start the day for the player.
        PlayerPrefs.SetInt("Init", 13); //Prevents the playerpreferences from getting set a second time to default values.
    }
    
    /*
    This function initializes values for verifying which day it is, and which activity the player is allowed to do.
    */
    void StartDay() {
        hint = PlayerPrefs.GetInt("Hint");
        unit = PlayerPrefs.GetInt("Unit");
        day = PlayerPrefs.GetInt("DayCount");
        option = PlayerPrefs.GetString("OptionChosen");
        studyCount = PlayerPrefs.GetInt("StudyCount");
        manageDay(); //call the function to decide what to do this day.
    }

    /*
    manageDay() sets the arrows indicating what the player is allowed to do, as well as increment hintcount if they earned another hint. 
    */
    void manageDay() {
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("Arrow"); //Select all of the arrows currently existing.
        foreach (GameObject obj in objectsToDestroy) { //This loop destroys all of the arrows previously existing.
            Destroy(obj);
        }
        if(PlayerPrefs.GetInt("DayCount") % 4 == 0){ //if the day count is a multiple of 4, the user needs to take a MC Quiz to advance the game.
            GameObject quizArrow = Instantiate(arrow, new Vector3 (1.5f, 0.5f, 0), Quaternion.Euler(new Vector3 (0, 0, 90f))); //spawn an arrow for the player
            if((PlayerPrefs.GetInt("QuizDone") == 1) && (PlayerPrefs.GetInt("Unit")  % 4 == 0)) { //if the quiz is done and it is the 4th unit (for Drag/Drop [Alice] Game)
                Destroy(quizArrow); //Destroy the quizArrow to prevent confusion.
                GameObject aliceArrow = Instantiate(arrow, new Vector3 (0.15f, -3.5f, 0), transform.rotation); //Create a new arrow for the game.
            }
        }
        else { //if it is not a Quiz Day (any day not a multiple of 4)
            PlayerPrefs.SetInt("QuizDone", 0); //reset QuizDone to prevent confusion for Drag/Drop
            GameObject tetrisArrow = Instantiate(arrow, new Vector3 (0.15f, -3.5f, 0), transform.rotation); //Spawn an arrow for tetris
            GameObject studyArrow = Instantiate(arrow, new Vector3 (-1.5f, 1.75f, 0), Quaternion.Euler(new Vector3 (0, 0, 180f))); //Spawn an arrow for Studying
            if(PlayerPrefs.GetString("OptionChosen") == "Study") { //if the user chose to Study, check fi they have studied enough.
                checkThreshold();
            }
            else if (PlayerPrefs.GetString("OptionChosen") == "Game") { //otherwise, halve their current studythreshold to earn a new hint.
                int threshold = PlayerPrefs.GetInt("StudyThreshold"); //get current threshold.
                threshold = (int) threshold / 2; //halve the current threshold.
                if(threshold <= 0){ //set minimum value to 1.
                    threshold = 1;
                }
                PlayerPrefs.SetInt("StudyThreshold", threshold); //set the new PlayerPref
                checkThreshold(); //check if the user now exceeds the threshold.
            }
        } 
    }

    /*
    This function checks whether the user has studied enough to earn a new hint. If they have, it also changes the requirement, as well as resets their StudyCount.
    */
    void checkThreshold() {
        if(studyCount >= PlayerPrefs.GetInt("StudyThreshold")){ //if the user has studied more than the threshold (which is grabbed here in case it just changed).
            PlayerPrefs.SetInt("Hint", PlayerPrefs.GetInt("Hint") + 1); //Increase hint count.
            PlayerPrefs.SetInt("StudyThreshold", PlayerPrefs.GetInt("StudyThreshold") + 1); //Increase threshold.
            PlayerPrefs.SetInt("StudyCount", 0); //Reset StudyCount
        }
    }
    
    /*
    Update is automatically called every frame. This function allows the user to exit the game.
    */
    void Update() {

        if(Input.GetKeyDown(KeyCode.Escape)) { //exit the game if user presses ESC.

            Application.Quit();

        }
        
    }

}