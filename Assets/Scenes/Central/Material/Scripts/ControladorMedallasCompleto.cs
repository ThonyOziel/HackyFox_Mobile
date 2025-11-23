using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorMedallasCompleto : MonoBehaviour
{
    [System.Serializable]
    public class MedallaUI
    {
        public CanvasGroup ImagenCandado;   // CandadoLeccX
        public CanvasGroup ImagenMedalla;   // MedallaX
        public int idLeccion;               // 1 a 6
    }

    public MedallaUI[] medallas;
    public float duracionFade = 0.6f;

    [Header("Botón para regresar")]
    public Button BtnRegresar;

    void Start()
    {
        // Conectar botón regresar
        if (BtnRegresar != null)
            BtnRegresar.onClick.AddListener(RegresarAPerfilProgreso);

        foreach (var medalla in medallas)
        {
            ConfigurarMedalla(medalla);
        }
    }

    void ConfigurarMedalla(MedallaUI medalla)
    {
        int leccionAvance = PlayerPrefs.GetInt($"AvanceLeccion{medalla.idLeccion}", 0);
        int relampagoAvance = PlayerPrefs.GetInt($"AvanceRelampago{medalla.idLeccion}", 0);
        int dinamicaAvance = PlayerPrefs.GetInt($"AvanceDinamica{medalla.idLeccion}", 0);

        int total = Mathf.Clamp(leccionAvance + relampagoAvance + dinamicaAvance, 0, 100);
        bool completada = total >= 100;

        if (completada)
        {
            // Mostrar directamente la medalla sin animación
            SetAlpha(medalla.ImagenCandado, 0f);
            SetAlpha(medalla.ImagenMedalla, 1f);
        }
        else
        {
            // Mostrar directamente el candado sin animación
            SetAlpha(medalla.ImagenCandado, 1f);
            SetAlpha(medalla.ImagenMedalla, 0f);
        }
    }

    void SetAlpha(CanvasGroup cg, float a)
    {
        if (cg == null) return;
        cg.alpha = a;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    // ✅ Navegación hacia la pestaña PerfilProgreso (ruta completa)
    void RegresarAPerfilProgreso()
    {
        SceneManager.LoadScene("Scenes/Central/PerfilProgreso");
    }
}
