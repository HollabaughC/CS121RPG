using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class AliceGameManager : MonoBehaviour
{
    public int timeRemaining = 360;
    int minutes;
    int seconds;
    public GameObject timer;
    GameObject objSelected = null;
    public List<GameObject> snapPoints;
    private float snapSensitivity = 0.5f;
    public ParsePyScript parser;
    private List<Line> lines;
    public GameObject commentLine;
    public GameObject emptyLine;
    public GameObject codeBlock;
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
        /*(spotsLeft == 0){
            //end the game
            int j = 0;
            for(int i = 0; i < lines.Count; i++){
                if(!lines[i].comment){
                    GameObject currSpot = snapPoints[j];
                    GameObject pointToCheck = 
                    if(string.Equals(lines[i].text, pointToCheck.transform.GetChild(0).GetComponent<TMP_Text>.text)){
                        Debug.Log("You did it!")
                    }
                }
            }
        }*/
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
        if(--timeRemaining == 0) CancelInvoke("Countdown");
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
            for(int i = 0; i < lines.Count; i++){
                if(lines[i].comment){
                    GameObject newComment = Instantiate(commentLine, startingPosition, transform.rotation);
                    newComment.transform.GetChild(0).GetComponent<TextSizing>().UpdateText(lines[i].text);
                }
                else { 
                    GameObject newObject = Instantiate(emptyLine, (startingPosition + new Vector3(0, 0, 0.1f)), transform.rotation);
                    snapPoints.Add(newObject);
                }
                startingPosition = startingPosition - new Vector3(0, 0.5f, 0);
            }
            makePieces(new Vector3(4, 4.5f, 1), true);
        }
        else {
            for(int i = 0; i < lines.Count; i++){
                if(!lines[i].comment){
                    GameObject newBlock = Instantiate(codeBlock, startingPosition, transform.rotation);
                    newBlock.transform.GetChild(0).GetComponent<TextSizing>().UpdateText(lines[i].text);
                    //Debug.Log("Added: " + newBlock.transform.GetChild(0).GetComponent<TMP_Text>().text);
                }
                startingPosition = startingPosition - new Vector3(0, 0.5f, 0);
            }
        }
    }
}
