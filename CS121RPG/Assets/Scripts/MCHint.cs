using UnityEngine;
using UnityEngine.UI;

/*
This script is attached to the Hint Button GameObject and serves to manage the user's hints.    
*/

public class MCHint : MonoBehaviour
{
    public Button hintButton;
    public Button hintPanel;
    public GameObject hintPanelObject;
    public QuizManagerMC quizManager;

    /*
    Start() is called when the scene is loaded.
    */
    void Start()
    {
        if(hintButton != null){
            hintButton.onClick.AddListener(displayHint); //Adds an onClick event to the button component to displayHint()
            hintButton.transform.GetChild(0).GetComponent<Text>().text = "Hints: " + quizManager.hintCount; //Changes the button's text to reflect the amount of hints.
        }
    }

    /*
    This function checks to see if the user should consume another Hint, and if so, called quizManager's getHint(). It also updates the Button's text component.
    */
    public void displayHint() {
        if(quizManager.hintCount > 0 && hintPanelObject.activeSelf == false){ //if there are hints left, and there is not an active hint being shown.
            if(hintPanel != null){
                quizManager.getHint(); //call QuizManagerMC.cs' getHint().
            }
        }
        hintButton.transform.GetChild(0).GetComponent<Text>().text = "Hints: " + quizManager.hintCount; //update text component to reflect hintCount
    }

}
