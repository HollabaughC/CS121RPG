using UnityEngine;
using UnityEngine.UI;

public class MCAnswers : MonoBehaviour
{
    public Button answerButton;
    public bool isCorrect = false;
    public QuizManagerMC quizManager;
    void Start(){
        if(answerButton != null){
            answerButton.onClick.AddListener(Answer);
        }
    }
    public void Answer(){
        if(isCorrect){
            quizManager.correct();
            Debug.Log("Correct");
        }
        else {
            Debug.Log("wrong answer");
        }
    }
}
