using UnityEngine;

public class LeccionButton : MonoBehaviour
{
  
    public LeccionBarManager LeccionBarManager;
    [Header("Visual")]
    private SpriteRenderer buttonRenderer;
    private Color originalColor;

    void Start()
    {
        //SpriteRenderer feedback visual
        buttonRenderer = GetComponent<SpriteRenderer>();
        if (buttonRenderer != null)
            originalColor = buttonRenderer.color;

    }
    void OnMouseDown()
    {
        // Efecto visual de apretado: baja saturación y contraste
        if (buttonRenderer != null)
            buttonRenderer.color = new Color(0.6f, 0.6f, 0.6f, 1f); // gris tenue 

        // Rellenar la barra de comida
        if (LeccionBarManager != null)
            LeccionBarManager.Refill();

    }

    void OnMouseUp()
    {
        // Restaurar color original al soltar
        if (buttonRenderer != null)
            buttonRenderer.color = originalColor;
    }
}