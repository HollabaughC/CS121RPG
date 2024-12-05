using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

/*
This script is attached to a hidden GameObject in the Drag and Drop/Alice-like game. It manages important parts of the scene, such as spawning in the pieces, running the timer,
allowing users to drag certain objects, and allowing thew user to "win" the game, or keep trying until time runs out. 
*/

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
        parser.Start(); //This line ensures that the parser is ran before this script continues, to make sure the script is read correctly.
        InvokeRepeating("Countdown", 1.0f, 1.0f); //This starts the timer by invoking the Countdown function every second, starting at the first second.
        lines = parser.GetLines(); //This sets the output from the parser to be used in this script.
        snapPoints = new List<GameObject>(); //snapPoints refers to a list of GameObjects that act as snapping points for the codeblocks.
        Vector3 playAreaPos = new Vector3 (-4, 4.5f, 1); //this is used to set the intial position to spawn blocks.
        makePieces(playAreaPos, false); //This function spawns the blocks.
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){ //if user clicks the screen, see if an object is found.
            CheckHitObject();
        }
        if(Input.GetMouseButton(0) && objSelected != null){ //if user is holding the mouse down, move the object.
            dragObject();
        }
        if(Input.GetMouseButtonUp(0) && objSelected != null){ //If the user lifts the button up, drop the object.
            dropObject();
        }

        if(Input.GetKeyDown(KeyCode.Escape)) { //exit the game when user presses ESC

            Application.Quit();

        }

    }

    /*
    This function is called when the user clicks on the screen, it checks if an object with a "Codeblock" tag is clicked on, and if so, allows it to move.
    */
    void CheckHitObject(){
        RaycastHit2D hit2d = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition)); //RaycastHit2D is used to check if there is an object where the mouse is.
        if(hit2d.collider != null && hit2d.transform.gameObject.tag == "Codeblock"){ //if the Raycast hit something, it would be found in hit2d.collider, the second condition checks the tag.
            objSelected = hit2d.transform.gameObject; //set the found object to objSelected
            for(int i = 0; i < snapPoints.Count; i++){ //This for loop is used to check if the item is being dragged out of a snapPoint, as they can only hold one object.
                if(Distance2D(snapPoints[i].transform.position, objSelected.transform.position) == 0 && snapPoints[i].GetComponent<BoxController>().isHolding == true){ //if the objSelected is on top of a snapPoint and is being "held".
                    snapPoints[i].GetComponent<BoxController>().isHolding = false;
                    spotsLeft--;
                    break;
            }
        }
        }
    }

    /*
    This function drags the selected object following the mouses' position on the screen. 
    */
    void dragObject(){
        objSelected.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 10.0f));
        //Camera is used to find the position relative to world view.
    }

    /*
    This function drops the held object when use lifts the key, and if it is near a snapPoint, it snaps to the point and becomes "held"
    */
    void dropObject(){
        for(int i = 0; i < snapPoints.Count; i++){
            if(Distance2D(snapPoints[i].transform.position, objSelected.transform.position) < snapSensitivity && snapPoints[i].GetComponent<BoxController>().isHolding == false){ //if the object is close enough to a point, and the point is not holding anything.
                objSelected.transform.position = new Vector3(snapPoints[i].transform.position.x, snapPoints[i].transform.position.y, snapPoints[i].transform.position.z -0.1f); //set the object on top of the snapPoint
                snapPoints[i].GetComponent<BoxController>().isHolding = true; 
                spotsLeft++;
                break;
            }
        }
        objSelected = null; //reset objSelected to null.
    }

    /*
    Distance2D is a helper function as in a 2D game, the Z axis does not matter much.  
    */
    float Distance2D(Vector3 vec1, Vector3 vec2) {
        Vector2 a = new Vector2(vec1.x, vec1.y);
        Vector2 b = new Vector2(vec2.x, vec2.y);
        return Vector2.Distance(a,b);

    }

    /*
    While not directly referenced, this function is called on Repeat by the InvokeRepeating above.
    */
    void Countdown() {
        if(--timeRemaining == 0) { //if time runs out, end the game without winnning.
            CancelInvoke("Countdown");
            endGame(false);
        }
        timer.transform.GetChild(3).GetComponent<TMP_Text>().text = getTime(timeRemaining); //Grab the 3rd(index) child of the timer, grab it's TMP_Text component, and change its text.
        timer.GetComponent<Slider>().value = timeRemaining; //Increment the slider bar
    }

    /*
    Helper Function for formatting the time displayed on the timer.
    */
    string getTime(int time){ 
        int seconds = time % 60;
        int minutes =  time / 60;
        string output = $"{minutes}:{seconds.ToString("00")}"; //format the time with minutes/seconds and make sure the seconds has 2 digits only.
        return output;
    }

    /*
    This function is called during Start(), and then again by itself. It uses recursion to place 2 sets of pieces, as well as populate snapPoints. After it calls itself, it is not called again.
    */
    void makePieces(Vector3 startingPosition, bool shuffle) {
        if(!shuffle){ //this is called only from Start(), and is only called once.
            List<string> options = new List<string>(); //this List is used for the Comment mapping
            int commentCount = 0; //this acts as an index for comments.
            for(int i = 0; i < lines.Count; i++){ //iterate through lines
                if(lines[i].comment){ //if it is a comment
                    GameObject newComment = Instantiate(commentLine, startingPosition, transform.rotation); //spawn a new comment line in the right spot.
                    options.Add(commentCount + ": "+ lines[i].text); //add the comment to the options.
                    newComment.transform.GetChild(0).GetComponent<TextSizing>().UpdateText("#TODO: " + commentCount++, lines[i].indentLevel); //Set the text component of the comment line to generic TODO.
                }
                else { //otherwise, not a comment line
                    GameObject newObject = Instantiate(emptyLine, (startingPosition + new Vector3(0, 0, 0.1f)), transform.rotation); //spawn an empty snapPoint
                    snapPoints.Add(newObject); //add this object to the List.
                }
                startingPosition = startingPosition - new Vector3(0, 0.5f, 0); //increment the starting position
            }
            todo.Start(); //Invokes the TODO list button to populate it.
            todo.options = options; //set its options.
            todo.setOptions(); //populate the list
            makePieces(new Vector3(4, 4.5f, 1), true); //calls this function again to make the codeblocks.
        }
        else { //this condition is only hit on the second call of this function.
            for(int i = 0; i < lines.Count; i++){ 
                if(!lines[i].comment){ //for non-comment lines (as they are not made into codeblocks).
                    float horizontalShift = Random.Range(-5.0f, 5.0f); //Give the codeblocks random horizontal and vertical shift to randomize their positions. 
                    horizontalShift /= 5;
                    float verticalShift = Random.Range(-10.0f, 10.0f);
                    verticalShift /= 2;
                    Vector3 positionToPlace = startingPosition + new Vector3(horizontalShift, verticalShift, 0); //new position to place in
                    GameObject newBlock = Instantiate(codeBlock, positionToPlace, transform.rotation); //create a codeblock in that spot
                    newBlock.transform.GetChild(0).GetComponent<TextSizing>().UpdateText(lines[i].text, lines[i].indentLevel); //change the codeblock's text component.
                }
                startingPosition = startingPosition - new Vector3(0, 0.5f, 0); //increment starting position
            }
        }
    }

    /*
    This function is invoked when the user clicks the Submit Button, it verifies their input using a Ray to see which gameObjects are hit, and if they are Codeblocks, verify they are in the right order.
    */
    public void checkAnswer(){
        int commentCount = 0; //used as an index for how many comments have been found.
        bool correct = true; //set the condition to true, which if anything is found wrong, then change it.
        if(spotsLeft == snapPoints.Count){ //if all of the snapPoints are filled.
            for(int i = 0; i < snapPoints.Count; i++){ //iterate through snap points.
                if(lines[i+commentCount].comment) commentCount++; //if the item in lines at this index is a comment, increment comment count (as snapPoints.Count != lines.Count)
                Vector3 posToCheck = snapPoints[i].transform.position; //set position for the ray to the position of the snapPoint.
                RaycastHit2D hit = Physics2D.Raycast(posToCheck, Vector2.zero);  // Cast a ray at the point with no direction (point check)
                if (hit.collider != null) { //if something is found
                    if (hit.collider.CompareTag("Codeblock")){ //if the item found has the "codeblock tag"
                        GameObject selectedObject = hit.collider.gameObject; 
                        if (selectedObject.transform.GetChild(0).GetComponent<TMP_Text>().text.TrimStart() != lines[i+commentCount].text) { //if the selected object has the same text as the corresponding line (i+comment count)
                            correct = false; //prevet user from completing the game.
                            selectedObject.transform.position = (selectedObject.transform.position + new Vector3(10, 0, 0)); //move the found object off of the spot to indicate it is wrong.
                            snapPoints[i].GetComponent<BoxController>().isHolding = false; //the snapPoint is no longer "holding" an object.
                            spotsLeft--;
                        }
                    }
                }
            }
            if(correct){ //if everything is correct, the user wins.
                endGame(true);
            }
        }
    }

    /*
    This function is invoked when the user either submits all correct answers, or runs out of time. If they won, it progresses the game, otherwise, it just loads you back into the Main Room.
    */
    void endGame(bool won){
        if(won){
            PlayerPrefs.SetInt("AliceUnit", PlayerPrefs.GetInt("AliceUnit", 0) + 1); //Change the Unit of the Drag/Drop game (Alice-like), added for future implementations which may have multiple questions.
            PlayerPrefs.SetInt("DayCount", PlayerPrefs.GetInt("DayCount") + 1); //Increment the day, in case the game goes past the first "Lab"
            PlayerPrefs.SetInt("QuizDone", 0); //resets QuizDone so the user cannot do the "Lab" again.
        }
        SceneManager.LoadScene("SampleScene"); //Loads Main Room.
    }

}
