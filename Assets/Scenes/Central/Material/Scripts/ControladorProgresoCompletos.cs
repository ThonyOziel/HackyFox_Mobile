using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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

        int avLecc  = PlayerPrefs.GetInt($"AvanceLeccion{p.idLeccion}", 0);
        int avRelam = PlayerPrefs.GetInt($"AvanceRelampago{p.idLeccion}", 0);
        int avDin   = PlayerPrefs.GetInt($"AvanceDinamica{p.idLeccion}", 0);
        int total   = Mathf.Clamp(avLecc + avRelam + avDin, 0, 100);
        float prog  = total / 100f;

        // Mostrar la fila
        if (p.BarraLeccion != null)
            p.BarraLeccion.SetActive(true);

        // Ocultar contenido desplegable por defecto
        if (p.ContenidoBarra != null)
            p.ContenidoBarra.SetActive(false);

        // Flecha visible solo si la lección está desbloqueada
        if (p.FlechaDesplegable != null)
            p.FlechaDesplegable.gameObject.SetActive(anteriorTerminada);

        // Rotación inicial de flecha
        if (p.FlechaIcono != null)
            p.FlechaIcono.localRotation = Quaternion.Euler(0, 0, 180f);

        // Click de flecha
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

        // Actualizar barra de progreso
        if (p.BarraAmarilla != null)
        {
            Vector3 escala = p.BarraAmarilla.localScale;
            escala.x = prog;
            p.BarraAmarilla.localScale = escala;
        }

        // CandadoProgresoX: visible si la anterior NO está terminada
        SetAlpha(p.CandadoProgreso, anteriorTerminada ? 0f : 1f);
    }

    bool EstaLeccionTerminada(int idLeccion)
    {
        int avLecc  = PlayerPrefs.GetInt($"AvanceLeccion{idLeccion}", 0);
        int avRelam = PlayerPrefs.GetInt($"AvanceRelampago{idLeccion}", 0);
        int avDin   = PlayerPrefs.GetInt($"AvanceDinamica{idLeccion}", 0);
        int total   = Mathf.Clamp(avLecc + avRelam + avDin, 0, 100);
        return total >= 100;
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
