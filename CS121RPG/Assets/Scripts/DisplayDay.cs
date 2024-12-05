using UnityEngine;

public class DisplayDay : MonoBehaviour
{
    // GUI display for DayCount and Unit values
    private void OnGUI()
    {
        // Retrieve values from PlayerPrefs
        int dayCount = PlayerPrefs.GetInt("DayCount", 0); // Default to 0 if not set
        int unit = PlayerPrefs.GetInt("Unit", 1);         // Default to 1 if not set

        // Create the text to display
        string displayText = $"DayCount: {dayCount}\nUnit: {unit}";

        // Define the position and size of the text box
        float boxWidth = 150f;
        float boxHeight = 60f;
        float boxX = 10f;  // Distance from the left
        float boxY = 10f;  // Distance from the top

        // Create the box in the upper left corner
        GUI.Box(new Rect(boxX, boxY, boxWidth, boxHeight), displayText);
    }
}
