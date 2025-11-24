using UnityEngine;

public class MenuButton : MonoBehaviour
{
    private SpriteRenderer buttonRenderer;
    private Color originalColor;

    void Start()
    {
        buttonRenderer = GetComponent<SpriteRenderer>();
        if (buttonRenderer != null) originalColor = buttonRenderer.color;
    }

    void OnMouseDown()
    {
        if (buttonRenderer != null)
            buttonRenderer.color = new Color(0.6f, 0.6f, 0.6f, 1f);

        if (FloatingMenuManager.Instance != null)
            FloatingMenuManager.Instance.OpenMenu();
    }

    void OnMouseUp()
    {
        if (buttonRenderer != null)
            buttonRenderer.color = originalColor;
    }
}
