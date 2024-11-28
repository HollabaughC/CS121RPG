using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextSizing : MonoBehaviour
{
    private Vector3 originalScale;
    private TMP_Text tmp;
    void Awake()
    {
        originalScale = transform.localScale;
        tmp = GetComponent<TMP_Text>();
    }

    public void UpdateScale() {
        transform.localScale = originalScale;
    }
    public void UpdateText(string text, int indent){
        string indents = "";
        for(int i = 0; i < indent; i++){
            indents += "   ";
        }
        tmp.text = (indents + text);
    }
}
