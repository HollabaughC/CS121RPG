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
    public GameObject hintPanel;
    public int hintCount;
    
    void Start(){
        JQP.Start();
        hintCount = 3;
        generateQuestionList();
    }

    public void correct(){
        qIndexOptions.RemoveAt(qIndex);
        if(qIndexOptions.Count > 0)
            generateQuestions();
        else {
            Debug.Log("You Finished This Unit!");
            uIndex++;
            generateQuestionList();
            if(uIndex >= JQP.data.unit.Count)
                Debug.Log("You have finished all Units!");
        }
    }
    
    void setAnswers() {
        for(int i = 0; i < options.Length; i++){
            options[i].GetComponent<MCAnswers>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = JQP.data.unit[uIndex].question[qIndexOptions[qIndex]].answers[i];
            if(i == JQP.data.unit[uIndex].question[qIndexOptions[qIndex]].correct){
                options[i].GetComponent<MCAnswers>().isCorrect = true;
            }
        }
    }

    void generateQuestions() {
        qIndex = Random.Range(0, qIndexOptions.Count-1);
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
    public void getHint() {
        hintCount--;
        hintPanel.transform.GetChild(0).GetComponent<Text>().text = JQP.data.unit[uIndex].question[qIndexOptions[qIndex]].hint;
    }
}
