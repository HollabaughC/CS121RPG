using UnityEngine;
using UnityEngine.UI;

public class SubmitButton : MonoBehaviour
{
    public Button submit;
    public AliceGameManager game;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if(submit != null){
            submit.onClick.AddListener(checkAnswer);
        }
    }

    void checkAnswer() {
        game.checkAnswer();
    }

}
