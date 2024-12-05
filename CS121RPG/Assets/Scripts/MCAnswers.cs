using UnityEngine;
using UnityEngine.UI;

/*
This Script is attached to the buttons in the QuizGame and allows them to function.    
*/

public class MCAnswers : MonoBehaviour
{
    public Button answerButton; //this is set to the button component of the object
    public bool isCorrect = false; //Initialized to false, but public so the QuizManager (QuizManagerMC.cs) can map the correct answer.
    public QuizManagerMC quizManager;

    /*
    Start() is called when the scene is first loaded. This attaches an eventlistener to the buttons to trigger Answer()
    */
    void Start(){
        if(answerButton != null){
            answerButton.onClick.AddListener(Answer); //onClick event, call Answer()
        }
    }

    /*
    This function verifies if the clicked button is mapped to the correct answer, and reacts accordingly.
    */
    public void Answer(){
        if(isCorrect){
            quizManager.correct(); //Call the Quiz Manager's correct() function.
        }
        else {
            quizManager.loseLife(); //Call the quiz Manager's loseLife() function.
        }
    }
}
