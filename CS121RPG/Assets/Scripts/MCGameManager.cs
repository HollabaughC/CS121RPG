using UnityEngine;
using System.Collections.Generic; 

public class MCGameManager : MonoBehaviour
{
    public GameObject question;
    public List<GameObject> answers = new List<GameObject>();
    private bool isAnswered = false;
    private List<Dictionary> q_list = new List<Dictionary>();
    private int q_num;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Text questionText = question.GetComponent<Text>();
        string[] names = ['Answer0', 'Answer1','Answer2', 'Answer3'];
        foreach(string name in names) {
            GameObject a = GameObject.Find(name);
            answers.add(g);
        }
        getDicts(1);
        q_num = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isAnswered){
            isAnswered = false;
            Dictionary q = q_list[q_num++];
            string q_text = q.Key;
            string[] a_texts = q.Value;
            for(int i = 0; i < answers.length; i++){
                answers[i].text = a_texts[i]; //this line needs to be changed to access the actual text object. 
            }
        }
    }

    void getDicts(int unit) {
        //fetch questions 10 questions from unit
        int num_q = 1;
        for(int i = 0; i < num_q; i++){
            //for loop for multiple questions
            string question = "This is question 1";
            string[] answers = ['Answer0', 'Answer1', 'Answer2', 'Answer3'];
            Dictionary<string, string> q_dict = new Dictionary<string, string>();
            q_dict.add(question, answers);
            q_list.add(q_dict);
        }

    }
}
