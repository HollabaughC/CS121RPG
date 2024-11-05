using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuizManagerMC : MonoBehaviour
{
    public JSONQuestionParser JQP;
    public GameObject[] options = new GameObject[4];
    public List<int> qIndexOptions;
    public int qIndex;
    public int uIndex = 0;
    public Text qText;
    
    void Start(){
        generateQuestionList();
    }

    public void correct(){
        qIndexOptions.RemoveAt(qIndex);
        if(qIndexOptions.Count > 0)
            generateQuestions();
        else
            Debug.Log("You Finished!");
    }
    
    void setAnswers() {
        for(int i = 0; i < options.Length; i++){
            options[i].GetComponent<MCAnswers>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = JQP.data.unit[uIndex].question[qIndexOptions[qIndex]].answers[i];
            Debug.Log("setting answer to " + JQP.data.unit[uIndex].question[qIndexOptions[qIndex]].answers[i]);
            if(i == JQP.data.unit[uIndex].question[qIndexOptions[qIndex]].correct){
                options[i].GetComponent<MCAnswers>().isCorrect = true;
            }
        }
    }

    void generateQuestions() {
        qIndex = Random.Range(0, qIndexOptions.Count);
        qText.text = JQP.data.unit[uIndex].question[qIndexOptions[qIndex]].text;
        setAnswers();
    }
    void generateQuestionList() {
        qIndexOptions = new List<int>();
        for(int i = 0; i < JQP.data.unit[uIndex].question.Count; i++){
            qIndexOptions.Add(i);
        }
        generateQuestions();        
    }
}
