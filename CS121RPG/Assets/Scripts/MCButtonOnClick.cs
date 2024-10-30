using UnityEngine;

public class MCButtonOnClick : MonoBehaviour
{
    public Text question;
    //public List<Text> answersText = new List<Text>();
    public List<Button> buttonList = new List<Button>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //answersText = Resources.LoadAll<Text>("answer").ToList();
        buttonList = Resources.LoadAll<buttonList>("answer").ToList();
        foreach(Button btn in buttonList){
            btn.onClick.AddListener(ButtonOnClick);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ButtonOnClick() {
        Debug.Log("You clicked the button");
    }
}
