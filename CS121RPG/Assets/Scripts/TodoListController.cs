using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

/*
This script is attached to the dropdown in the Drag/Drop(Alice-like) minigame to update the options to be the TODO list.
*/

public class TodoListController : MonoBehaviour
{
    public List<string> options;
    TMP_Dropdown todoList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start() {
        todoList = GetComponent<TMP_Dropdown>(); //get the TextMeshPro-Dropdown component
    }

    /*
    This function is used to change the text of the options to fit the TODOs.
    */
    public void setOptions(){
        todoList.ClearOptions(); //Clear the current options.
        todoList.AddOptions(options); //Add the list of options to the list.
    }
}
