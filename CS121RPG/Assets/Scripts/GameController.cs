using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static GameController Instance;
    public GameObject arrow;
    public int hint; //starts at 3

    public int unit;
    public int aliceUnit;
    public int day; //start at 1

    public int tetris_high_score;
    public string option;
    public int studyCount;

    private void Awake() {
        //Lines 27, 31, and 32 are commented for testing purposes, when actually playing the game, they should be uncommented.
        if(PlayerPrefs.GetInt("Init") != 13) { //On the first run of this code it should set these and if they are set, do not touch them. 
            PlayerPrefs.SetInt("Hint", hint);
            PlayerPrefs.SetInt("Unit", unit);
            PlayerPrefs.SetInt("QuizDone", 0); //boolean value for whether or not the quiz was done.
            PlayerPrefs.SetInt("TetrisScore", tetris_high_score);
            PlayerPrefs.SetInt("AliceUnit", 0);
            PlayerPrefs.SetInt("DayCount", day);
            PlayerPrefs.SetInt("HintThreshold", 1);
            PlayerPrefs.SetInt("StudyCount", 0);
            PlayerPrefs.SetString("OptionChosen", "");
        }
        StartDay();
        PlayerPrefs.SetInt("Init", 13);
    }
    
    void StartDay() {
        //called whenever scene is loaded
        hint = PlayerPrefs.GetInt("Hint");
        unit = PlayerPrefs.GetInt("Unit");
        day = PlayerPrefs.GetInt("DayCount");
        option = PlayerPrefs.GetString("OptionChosen");
        studyCount = PlayerPrefs.GetInt("StudyCount");
        manageDay();
    }
    void manageDay() {
        //disable all arrows
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("Arrow");
        foreach (GameObject obj in objectsToDestroy) {
            Destroy(obj);
        }
        if(PlayerPrefs.GetInt("DayCount") % 4 == 0){
            //allow quiz only, then alice and spawn arrows
            GameObject quizArrow = Instantiate(arrow, new Vector3 (1.5f, 0.5f, 0), Quaternion.Euler(new Vector3 (0, 0, 90f)));
            //spawn arrows for quiz
            if((PlayerPrefs.GetInt("QuizDone") == 1) && (PlayerPrefs.GetInt("Unit")  % 4 == 0)) { //if the quiz is done and it is the 4th unit
                while(aliceUnit == PlayerPrefs.GetInt("AliceUnit")){
                    GameObject aliceArrow = Instantiate(arrow, new Vector3 (0.15f, -3.5f, 0), transform.rotation);
                    //enable collider for alice
                } 
            }
        }
        else {
            //allow tetris or studying, and spawn arrows
            GameObject tetrisArrow = Instantiate(arrow, new Vector3 (0.15f, -3.5f, 0), transform.rotation);
            GameObject studyArrow = Instantiate(arrow, new Vector3 (-1.5f, 1.75f, 0), Quaternion.Euler(new Vector3 (0, 0, 180f)));
            if(PlayerPrefs.GetString("OptionChosen") == "Study") {
                checkThreshold();
            }
            else if (PlayerPrefs.GetString("OptionChosen") == "Game") {
                int threshold = PlayerPrefs.GetInt("StudyThreshold"); 
                threshold = (int) threshold / 2;
                if(threshold <= 0){
                    threshold = 1;
                }
                PlayerPrefs.SetInt("StudyThreshold", threshold);
                checkThreshold();
            }
        }
        PlayerPrefs.SetInt("QuizDone", 0); //reset QuizDone back to false after each day.
    }

    void checkThreshold() {
        //Check to see if the player has studied enough to get their next hint
        if(studyCount >= PlayerPrefs.GetInt("StudyThreshold")){
            PlayerPrefs.SetInt("Hint", PlayerPrefs.GetInt("Hint") + 1);
            PlayerPrefs.SetInt("StudyThreshold", PlayerPrefs.GetInt("StudyThreshold") + 1);
            PlayerPrefs.SetInt("StudyCount", 0);
        }
    }
    
}