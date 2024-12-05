using UnityEngine;
using UnityEngine.UI;

/*
This script is attached to an "empty" GameObject which contains the user's Lives for the Quiz Game. It removes a heart if the user answers wrong, 
and if there are no more hearts, causes the player to lose.    
*/
public class LifeCount : MonoBehaviour
{
    public Image[] lives; 
    public int livesLeft; //Index of lives the user has. (initally 3) 
    public QuizManagerMC qmc;
    /*
    This function is called only through the Quiz Button's onClick event, (MCAnswers.cs, Answers()) which triggers QuizManagerMC.cs' loseLife() function, which calls this.
    */
    public void loseLife() {
        livesLeft--;
        lives[livesLeft].enabled = false; //remove the corresponding heart
        if(livesLeft <= 0){ //If the user has no more lives, they lose.
            qmc.loseGame();
        }
    }
}
