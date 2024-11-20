using UnityEngine;

public class TextSizing : MonoBehaviour
{
    private Vector3 originalScale;
    void Awake()
    {
        originalScale = transform.localScale;
    }

    public void UpdateScale() {
        transform.localScale = originalScale;
    }
}
