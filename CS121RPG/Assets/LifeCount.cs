using UnityEngine;
using UnityEngine.UI;

public class LifeCount : MonoBehaviour
{
    public Image[] lives; 
    public int livesLeft; 
    public QuizManagerMC qmc;
    public void loseLife() {
        livesLeft--;
        lives[livesLeft].enabled = false;
        if(livesLeft <= 0){
            qmc.loseGame();
        }
    }
}
