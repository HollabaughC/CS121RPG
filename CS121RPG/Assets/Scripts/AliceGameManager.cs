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
    private float snapSensitivity = 1.1f;
    public ParsePyScript parser;
    private List<Line> lines;
    public GameObject commentLine;
    public GameObject emptyLine;
    public GameObject codeBlock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parser.Start();
        Debug.Log(getTime(timeRemaining));
        //timer.GetComponent<TMP_Text>().GetComponent<TMPro.TextMeshProUGUI>().text = getTime(timeRemaining);
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
        }
    }

    void dragObject(){
        objSelected.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 10.0f));
    }

    void dropObject(){
        for(int i = 0; i < snapPoints.Count; i++){
            if(Vector3.Distance(snapPoints[i].transform.position, objSelected.transform.position) < snapSensitivity){
                objSelected.transform.position = new Vector3(snapPoints[i].transform.position.x, snapPoints[i].transform.position.y, snapPoints[i].transform.position.z -0.1f);
                break;
            }
        }
        objSelected = null;
    }
    string getTime(int time){
        seconds = time % 60;
        minutes =  time / 60;
        string output = $"{minutes}:{seconds.ToString("00")}";
        return output;
    }

    void makePieces(Vector3 startingPosition, bool shuffle) {
        if(!shuffle){
            for(int i = 0; i < lines.Count; i++){
                if(lines[i].comment){
                    Instantiate(commentLine, startingPosition, transform.rotation);
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
                    Instantiate(codeBlock, startingPosition, transform.rotation);
                }
                startingPosition = startingPosition - new Vector3(0, 0.5f, 0);
            }
        }
    }
}
