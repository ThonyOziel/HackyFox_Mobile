using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    private const float PROGRESO_MAX = 16.5f;

    void Start()
    {
        if (BtnRegresar != null)
            BtnRegresar.onClick.AddListener(RegresarAPerfilProgreso);

        foreach (var medalla in medallas)
            ConfigurarMedalla(medalla);
    }

    void ConfigurarMedalla(MedallaUI medalla)
    {
        float avLecc  = PlayerPrefs.GetFloat($"AvanceLeccion{medalla.idLeccion}", 0f);
        float avRelam = PlayerPrefs.GetFloat($"AvanceRelampago{medalla.idLeccion}", 0f);
        float avDin   = PlayerPrefs.GetFloat($"AvanceDinamica{medalla.idLeccion}", 0f);

        float total = Mathf.Clamp(avLecc + avRelam + avDin, 0f, PROGRESO_MAX);
        bool completada = total >= PROGRESO_MAX;

        SetAlpha(medalla.ImagenCandado, completada ? 0f : 1f);
        SetAlpha(medalla.ImagenMedalla, completada ? 1f : 0f);
    }

    void SetAlpha(CanvasGroup cg, float a)
    {
        if (cg == null) return;
        cg.alpha = a;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    void RegresarAPerfilProgreso()
    {
        SceneManager.LoadScene("Scenes/Central/PerfilProgreso");
    }
}
