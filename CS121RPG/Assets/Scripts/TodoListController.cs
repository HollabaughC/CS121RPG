using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TodoListController : MonoBehaviour
{
    public List<string> options;
    TMP_Dropdown todoList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start() {
        todoList = GetComponent<TMP_Dropdown>();
    }
    public void setOptions(){
        todoList.ClearOptions();
        todoList.AddOptions(options);
    }
}
