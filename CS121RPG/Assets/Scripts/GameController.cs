using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static GameController Instance;

    public int hint = 3;

    public int unit = 1;

    private void Awake() {
    // start of new code
    if (Instance != null)
    {
        Destroy(gameObject);
        return;
    }
    // end of new code

    Instance = this;
    DontDestroyOnLoad(gameObject);
    }
}