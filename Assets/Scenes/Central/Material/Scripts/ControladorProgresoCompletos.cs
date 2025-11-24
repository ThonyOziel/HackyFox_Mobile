using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorProgresoCompleto : MonoBehaviour
{
    [System.Serializable]
    public class ProgresoUI
    {
        public GameObject BarraLeccion;         // BarraLeccionX
        public Button FlechaDesplegable;        // FlechaDesplegableX
        public GameObject ContenidoBarra;       // ContenidoBarraX
        public RectTransform FlechaIcono;       // ícono de flecha
        public Transform BarraAmarilla;         // ProgresoX
        public CanvasGroup CandadoProgreso;     // CandadoProgresoX
        public int idLeccion;                   // 1 a 6
    }

    [Header("Botón para regresar")]
    public Button BtnRegresar;

    public ProgresoUI[] progresos;
    public float duracionFade = 0.6f;

    private const float PROGRESO_MAX = 16.5f;

    void Start()
    {
        if (BtnRegresar != null)
            BtnRegresar.onClick.AddListener(RegresarAPerfilProgreso);

        foreach (var p in progresos)
            ConfigurarProgreso(p);
    }

    void ConfigurarProgreso(ProgresoUI p)
    {
        bool anteriorTerminada = p.idLeccion == 1
            ? true
            : EstaLeccionTerminada(p.idLeccion - 1);

        float avLecc  = PlayerPrefs.GetFloat($"AvanceLeccion{p.idLeccion}", 0f);
        float avRelam = PlayerPrefs.GetFloat($"AvanceRelampago{p.idLeccion}", 0f);
        float avDin   = PlayerPrefs.GetFloat($"AvanceDinamica{p.idLeccion}", 0f);
        float total   = Mathf.Clamp(avLecc + avRelam + avDin, 0f, PROGRESO_MAX);
        float prog    = total / PROGRESO_MAX;

        if (p.BarraLeccion != null)
            p.BarraLeccion.SetActive(true);

        if (p.ContenidoBarra != null)
            p.ContenidoBarra.SetActive(false);

        if (p.FlechaDesplegable != null)
            p.FlechaDesplegable.gameObject.SetActive(anteriorTerminada);

        if (p.FlechaIcono != null)
            p.FlechaIcono.localRotation = Quaternion.Euler(0, 0, 180f);

        if (p.FlechaDesplegable != null)
        {
            p.FlechaDesplegable.onClick.RemoveAllListeners();
            p.FlechaDesplegable.onClick.AddListener(() =>
            {
                if (!anteriorTerminada) return;

                bool activo = p.ContenidoBarra != null && !p.ContenidoBarra.activeSelf;
                if (p.ContenidoBarra != null)
                    p.ContenidoBarra.SetActive(activo);

                if (p.FlechaIcono != null)
                    p.FlechaIcono.localRotation = Quaternion.Euler(0, 0, activo ? 0f : 180f);
            });
        }

        if (p.BarraAmarilla != null)
        {
            Vector3 escala = p.BarraAmarilla.localScale;
            escala.x = prog;
            p.BarraAmarilla.localScale = escala;
        }

        SetAlpha(p.CandadoProgreso, anteriorTerminada ? 0f : 1f);
    }

    bool EstaLeccionTerminada(int idLeccion)
    {
        float avLecc  = PlayerPrefs.GetFloat($"AvanceLeccion{idLeccion}", 0f);
        float avRelam = PlayerPrefs.GetFloat($"AvanceRelampago{idLeccion}", 0f);
        float avDin   = PlayerPrefs.GetFloat($"AvanceDinamica{idLeccion}", 0f);
        float total   = Mathf.Clamp(avLecc + avRelam + avDin, 0f, PROGRESO_MAX);
        return total >= PROGRESO_MAX;
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
