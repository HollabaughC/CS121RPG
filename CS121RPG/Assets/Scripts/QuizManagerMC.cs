using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuizManagerMC : MonoBehaviour
{
    public List<MCQuestions> qList;
    public GameObject[] options = new GameObject[4];
    public int qIndex;
    public Text qText;
    
    void Start(){
        generateQuestions();
    }

    public void correct(){
        qList.RemoveAt(qIndex);
        generateQuestions();
    }
    void setAnswer() {
        for(int i = 0; i < options.Length; i++){
            options[i].GetComponent<MCAnswers>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = qList[qIndex].answers[i];
            if(i == qList[qIndex].correctAnswer){
                options[i].GetComponent<MCAnswers>().isCorrect = true;
            }
        }
    }

    void generateQuestions() {
        qIndex = Random.Range(0, qList.Count);
        qText.text = qList[qIndex].question;
        setAnswer();
    }
}
