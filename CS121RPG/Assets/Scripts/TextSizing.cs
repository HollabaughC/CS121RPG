using UnityEngine;
using UnityEngine.UI;
using TMPro;


/*
This script is attached to the codeblock prefab to keep it's text at the right scale to fit in the block.
*/
public class TextSizing : MonoBehaviour
{
    private Vector3 originalScale; //In Unity, scales are stored as 3d vectors.
    private TMP_Text tmp;
    
    /*
    Awake() is called when the prefab is instantiated (spawned).
    */
    void Awake()
    {
        originalScale = transform.localScale; //get the current scale of the text.
        tmp = GetComponent<TMP_Text>(); //get the TextMeshPro component.
    }

    /*
    This function changes the text's scale back to the original.
    */
    public void UpdateScale() {
        transform.localScale = originalScale;
    }

    /*
    This function adds indents to the front of the text string based on IndentLevel.
    */
    public void UpdateText(string text, int indent){
        string indents = "";
        for(int i = 0; i < indent; i++){
            indents += "   "; //adds extra spaces per indentLevel
        }
        tmp.text = (indents + text); //add the indents to text.
    }
}
