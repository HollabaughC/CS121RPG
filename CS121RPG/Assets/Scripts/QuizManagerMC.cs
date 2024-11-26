using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public class QuizManagerMC : MonoBehaviour
{
    public JSONQuestionParser JQP;
    public GameObject[] options = new GameObject[4];
    public List<int> qIndexOptions;
    public int qIndex;
    public int uIndex = 0;
    public Text qText;
    public GameObject hintPanel;
    public int hintCount = 3;
    public int bonusQuestions = 3;
    
    
    void Start(){
        JQP.Start();
        hintPanel.SetActive(false);
        hintCount = PlayerPrefs.GetInt("Hint");
        uIndex = PlayerPrefs.GetInt("Unit");
        bonusQuestions = 3;
        generateQuestionList();
    }

    public void correct(){
        if(qIndexOptions.Count > 0)
            qIndexOptions.RemoveAt(qIndex);
        if(qIndexOptions.Count > 0)
            generateQuestions();
        else {
            if(bonusQuestions > 0 && uIndex >= 1){
                bonusQuestions--;
                generateQuestions();
            }
            else{
            //Debug.Log("You Finished This Unit!");
            PlayerPrefs.SetInt("Hint", hintCount);
            PlayerPrefs.SetInt("Unit", ++uIndex);
            SceneManager.LoadScene("SampleScene");
            //generateQuestionList();
            }
        }
    }
    
    void setAnswers(int unit, int question) {
        if(qIndexOptions.Count == 0){
            //qText.text = JQP.data.unit[qData.unit].question[qData.question].text;
            for(int i = 0; i < options.Length; i++){
                options[i].GetComponent<MCAnswers>().isCorrect = false;
                options[i].transform.GetChild(0).GetComponent<Text>().text = JQP.data.unit[unit].question[question].answers[i];
                if(i == JQP.data.unit[unit].question[question].correct){
                    options[i].GetComponent<MCAnswers>().isCorrect = true;
                }
            }
        }
        else {
            for(int i = 0; i < options.Length; i++){
                options[i].GetComponent<MCAnswers>().isCorrect = false;
                options[i].transform.GetChild(0).GetComponent<Text>().text = JQP.data.unit[unit].question[question].answers[i];
                if(i == JQP.data.unit[unit].question[question].correct){
                    options[i].GetComponent<MCAnswers>().isCorrect = true;
                }
            }
        }
    }

    void generateQuestions() {
        if(qIndexOptions.Count == 0){
            var qData = getExtraQuestionNums();
            qText.text = JQP.data.unit[qData.unit].question[qData.question].text;
            setAnswers(qData.unit, qData.question);
        }
        else {
            qIndex = Random.Range(0, qIndexOptions.Count-1);
            qText.text = JQP.data.unit[uIndex].question[qIndexOptions[qIndex]].text;
            setAnswers(uIndex, qIndexOptions[qIndex]);
        }
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
        hintPanel.SetActive(true);
        hintPanel.GetComponent<Button>().onClick.AddListener(() => {Debug.Log("clicked"); hintPanel.SetActive(false);});
    }
    public (int unit, int question) getExtraQuestionNums() {
        int unit = getUnit();
        int question = Random.Range(0,5); 
        return (unit, question);
    }
    public int getUnit() {
        int[] units = Enumerable.Range(1, uIndex).ToArray();
        int roll = Random.Range(1,101);
        int sum = 0;
        for(int i = 0; i < units.Length; i++){
            sum += units[i];
        }
        float cumsum = 0.0f;
        for(int i = 0; i < units.Length; i++){
            float temp = (float)units[i]/sum * 100;
            cumsum += temp;
            if (cumsum > roll) {
                return i;
            }
        }
        return units.Length - 1;
    }
}
