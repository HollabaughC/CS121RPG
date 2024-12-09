using UnityEngine;
using UnityEngine.UI;

/*
This script is attached to the submit button in the Drag/Drop (Alice-like) minigame.
*/
public class SubmitButton : MonoBehaviour
{
    public Button submit;
    public AliceGameManager game;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if(submit != null){
            submit.onClick.AddListener(checkAnswer); //add an event listener to the button component
        }
    }

    /*
    checkAnswer() handles the on-click event for the submit button, calling the Manager's checkAnswer() function.
    */
    void checkAnswer() {
        game.checkAnswer();
    }

}
