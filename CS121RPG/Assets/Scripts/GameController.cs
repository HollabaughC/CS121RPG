using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static GameController Instance;

    public int hint = 3;

    public int unit = 0;
    public int aliceUnit = 0;
    public int day = 1; //start at 1

    public int tetris_high_score = 0;
    public string option;
    public int studyCount;

    private void Awake() {
    // start of new code
    if (Instance != null)
    {
        Destroy(gameObject);
        return;
    }
    // end of new code

    Instance = this;
    DontDestroyOnLoad(gameObject);
    //Lines 27, 31, and 32 are commented for testing purposes, when actually playing the game, they should be uncommented.
    //if(PlayerPrefs.GetInt("Init") != 13) { //On the first run of this code it should set these and if they are set, do not touch them. 
        PlayerPrefs.SetInt("Hint", hint);
        PlayerPrefs.SetInt("Unit", unit);
        PlayerPrefs.SetInt("QuizDone", 0); //boolean value for whether or not the quiz was done.
        PlayerPrefs.SetInt("TetrisScore", tetris_high_score);
        PlayerPrefs.SetInt("AliceUnit", 0);
        PlayerPrefs.SetInt("DayCount", day);
        PlayerPrefs.SetInt("HintThreshold", 1);
        PlayerPrefs.SetInt("StudyCount", 0);
        PlayerPrefs.SetString("OptionChosen", "");
    //}
    //PlayerPrefs.SetInt("Init", 13);
    }
    void OnEnable() {
        //called whenever scene is loaded
        hint = PlayerPrefs.GetInt("Hint");
        unit = PlayerPrefs.GetInt("Unit");
        //day = PlayerPrefs.GetInt("DayCount");
        option = PlayerPrefs.GetString("OptionChosen");
        studyCount = PlayerPrefs.GetInt("StudyCount");
        manageDay();
    }
    void manageDay() {
        //disable all arrows
        if(day % 4 == 0){
            //allow quiz only, then alice and spawn arrows
            //spawn arrows for quiz
            if((PlayerPrefs.GetInt("QuizDone") == 1) && (unit  % 4 == 0)) { //if the quiz is done and it is the 4th unit
                while(aliceUnit == PlayerPrefs.GetInt("AliceUnit")){
                    //spawn arrow for alice
                    //enable collider for alice
                } 
            }
        }
        else {
            //allow tetris or studying, and spawn arrows
            if(option == "Study") {
                checkThreshold();
            }
            else if (option == "Game") {
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
            PlayerPrefs.SetInt("Hint", ++hint);
            PlayerPrefs.SetInt("StudyThreshold", PlayerPrefs.GetInt("StudyThreshold") + 1);
            PlayerPrefs.SetInt("StudyCount", 0);
        }
    }
    
}