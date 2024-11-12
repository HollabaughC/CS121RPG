using UnityEngine;
using UnityEngine.UI;

public class MCHint : MonoBehaviour
{
    public Button hintButton;
    public Button hintPanel;
    public QuizManagerMC quizManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(answerButton != null){
            hintButton.onClick.AddListener(displayHint);
        }
    }

    void displayHint() {
        if(quizManager.hintCount > 0){
            hintButton.transform.GetChild(0).GetComponent<Text>().text = "Hints: " + quizManager.hintCount;
            if(hintPanel != null){
                quizManager.getHint();
                hintPanel.enabled = true;
                hintPanel.onClick.AddListener(() => {hintPanel.enabled = false;});
            }
        }
    }

}