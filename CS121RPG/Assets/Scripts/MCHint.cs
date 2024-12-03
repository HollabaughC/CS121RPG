using UnityEngine;
using UnityEngine.UI;

public class MCHint : MonoBehaviour
{
    public Button hintButton;
    public Button hintPanel;
    public GameObject hintPanelObject;
    public QuizManagerMC quizManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(hintButton != null){
            hintButton.onClick.AddListener(displayHint);
            hintButton.transform.GetChild(0).GetComponent<Text>().text = "Hints: " + quizManager.hintCount;
        }
    }

    public void displayHint() {
        if(quizManager.hintCount > 0 && hintPanelObject.activeSelf == false){
            if(hintPanel != null){
                quizManager.getHint();
            }
        }
        hintButton.transform.GetChild(0).GetComponent<Text>().text = "Hints: " + quizManager.hintCount;
    }

}
