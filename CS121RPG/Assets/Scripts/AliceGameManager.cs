using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AliceGameManager : MonoBehaviour
{
    public int timeRemaining = 360;
    int minutes;
    int seconds;
    public GameObject timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(getTime(timeRemaining));
        timer.GetComponent<TMP_Text>().GetComponent<TMPro.TextMeshProUGUI>().text = getTime(timeRemaining);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    string getTime(int time){
        seconds = time % 60;
        minutes =  time / 60;
        string output = $"{minutes}:{seconds.ToString("00")}";
        return output;
    }
}
