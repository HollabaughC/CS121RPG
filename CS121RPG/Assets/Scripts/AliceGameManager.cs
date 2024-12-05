using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class AliceGameManager : MonoBehaviour
{
    public int timeRemaining = 360;
    public GameObject timer;
    GameObject objSelected = null;
    public List<GameObject> snapPoints;
    private float snapSensitivity = 0.5f;
    public ParsePyScript parser;
    private List<Line> lines;
    public GameObject commentLine;
    public GameObject emptyLine;
    public GameObject codeBlock;
    public TodoListController todo;
    int spotsLeft = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parser.Start();
        Debug.Log(getTime(timeRemaining));
        InvokeRepeating("Countdown", 1.0f, 1.0f);
        lines = parser.GetLines();
        snapPoints = new List<GameObject>();
        Vector3 playAreaPos = new Vector3 (-4, 4.5f, 1);
        makePieces(playAreaPos, false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            CheckHitObject();
        }
        if(Input.GetMouseButton(0) && objSelected != null){
            dragObject();
        }
        if(Input.GetMouseButtonUp(0) && objSelected != null){
            dropObject();
        }
    }

    void CheckHitObject(){
        RaycastHit2D hit2d = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if(hit2d.collider != null && hit2d.transform.gameObject.tag == "Codeblock"){
            objSelected = hit2d.transform.gameObject;
            for(int i = 0; i < snapPoints.Count; i++){
                if(Distance2D(snapPoints[i].transform.position, objSelected.transform.position) == 0 && snapPoints[i].GetComponent<BoxController>().isHolding == true){
                    snapPoints[i].GetComponent<BoxController>().isHolding = false;
                    spotsLeft--;
                    break;
            }
        }
        }
    }

    void dragObject(){
        objSelected.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 10.0f));
    }

    void dropObject(){
        for(int i = 0; i < snapPoints.Count; i++){
            if(Distance2D(snapPoints[i].transform.position, objSelected.transform.position) < snapSensitivity && snapPoints[i].GetComponent<BoxController>().isHolding == false){
                objSelected.transform.position = new Vector3(snapPoints[i].transform.position.x, snapPoints[i].transform.position.y, snapPoints[i].transform.position.z -0.1f);
                snapPoints[i].GetComponent<BoxController>().isHolding = true;
                spotsLeft++;
                break;
            }
        }
        objSelected = null;
    }

    float Distance2D(Vector3 vec1, Vector3 vec2) {
        Vector2 a = new Vector2(vec1.x, vec1.y);
        Vector2 b = new Vector2(vec2.x, vec2.y);
        return Vector2.Distance(a,b);

    }

    void Countdown() {
        if(--timeRemaining == 0) {
            CancelInvoke("Countdown");
            endGame(false);
        }
        timer.transform.GetChild(3).GetComponent<TMP_Text>().text = getTime(timeRemaining);
        timer.GetComponent<Slider>().value = timeRemaining;
    }
    string getTime(int time){
        int seconds = time % 60;
        int minutes =  time / 60;
        string output = $"{minutes}:{seconds.ToString("00")}";
        return output;
    }

    void makePieces(Vector3 startingPosition, bool shuffle) {
        if(!shuffle){
            List<string> options = new List<string>();
            int commentCount = 0;
            for(int i = 0; i < lines.Count; i++){
                if(lines[i].comment){
                    GameObject newComment = Instantiate(commentLine, startingPosition, transform.rotation);
                    options.Add(commentCount + ": "+ lines[i].text);
                    newComment.transform.GetChild(0).GetComponent<TextSizing>().UpdateText("#TODO: " + commentCount++, lines[i].indentLevel);
                }
                else { 
                    GameObject newObject = Instantiate(emptyLine, (startingPosition + new Vector3(0, 0, 0.1f)), transform.rotation);
                    snapPoints.Add(newObject);
                }
                startingPosition = startingPosition - new Vector3(0, 0.5f, 0);
            }
            for(int j= 0; j < options.Count; j++){
                Debug.Log("option: " + options[j]);
            }
            todo.Start();
            todo.options = options;
            todo.setOptions();
            makePieces(new Vector3(4, 4.5f, 1), true);
        }
        else {
            for(int i = 0; i < lines.Count; i++){
                if(!lines[i].comment){
                    float horizontalShift = Random.Range(-5.0f, 5.0f);
                    horizontalShift /= 5;
                    float verticalShift = Random.Range(-10.0f, 10.0f);
                    verticalShift /= 2;
                    Vector3 positionToPlace = startingPosition + new Vector3(horizontalShift, verticalShift, 0);
                    GameObject newBlock = Instantiate(codeBlock, positionToPlace, transform.rotation);
                    newBlock.transform.GetChild(0).GetComponent<TextSizing>().UpdateText(lines[i].text, lines[i].indentLevel);
                    //Debug.Log("Added: " + newBlock.transform.GetChild(0).GetComponent<TMP_Text>().text);
                }
                startingPosition = startingPosition - new Vector3(0, 0.5f, 0);
            }
        }
    }

    public void checkAnswer(){
        int commentCount = 0;
        bool correct = true;
        if(spotsLeft == snapPoints.Count){
            for(int i = 0; i < snapPoints.Count; i++){
                if(lines[i+commentCount].comment) commentCount++;
                Vector3 posToCheck = snapPoints[i].transform.position;
                RaycastHit2D hit = Physics2D.Raycast(posToCheck, Vector2.zero);  // Cast a ray at the point with no direction (point check)
                if (hit.collider != null) {
                    if (hit.collider.CompareTag("Codeblock")){
                        GameObject selectedObject = hit.collider.gameObject;
                        Debug.Log("Checking: " +lines[i+commentCount].text);
                        if (selectedObject.transform.GetChild(0).GetComponent<TMP_Text>().text.TrimStart() != lines[i+commentCount].text) {
                            correct = false;
                            selectedObject.transform.position = (selectedObject.transform.position + new Vector3(10, 0, 0));
                            snapPoints[i].GetComponent<BoxController>().isHolding = false;
                            spotsLeft--;
                        }
                    }
                }
            }
            if(correct){
                //Debug.Log("You Win!");
                endGame(true);
            }
        }
    }
    void endGame(bool won){
        if(won){
            PlayerPrefs.SetInt("AliceUnit", PlayerPrefs.GetInt("AliceUnit", 0) + 1);
            PlayerPrefs.SetInt("DayCount", PlayerPrefs.GetInt("DayCount") + 1);
            PlayerPrefs.SetInt("QuizDone", 0);
        }
        SceneManager.LoadScene("SampleScene");
    }

}
