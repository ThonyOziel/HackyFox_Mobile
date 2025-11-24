using UnityEngine;
using UnityEngine.SceneManagement; // ✅ necesario para cargar escenas

public class LeccionButton : MonoBehaviour
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

        // Bloquea SOLO el acceso al menú de lecciones
        LeccionBarManager bar = UnityEngine.Object.FindFirstObjectByType<LeccionBarManager>();
        if (bar != null && !bar.EstaDesbloqueada())
        {
            Debug.Log("Menú de Lecciones bloqueado por cooldown de 24 horas.");
            return;
        }

        // ✅ Aquí cargas la escena de lecciones
        SceneManager.LoadScene("Lecciones"); 
        // Asegúrate que el nombre coincida EXACTO con el que aparece en Build Settings
    }

    void OnMouseUp()
    {
        if (buttonRenderer != null)
            buttonRenderer.color = originalColor;
    }
}
