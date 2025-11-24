using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ControladorPerfilProgreso : MonoBehaviour
{
    [System.Serializable]
    public class LeccionUI
    {
        public GameObject BarraLeccion;
        public Button FlechaDesplegable;
        public GameObject ContenidoBarra;
        public RectTransform FlechaIcono;
        public Transform BarraAmarilla;
        public CanvasGroup CandadoProgreso;
        public CanvasGroup CandadoLeccion;
        public CanvasGroup MedallaLeccion;
        public int idLeccion;
    }

    public Button BtnVerTodoMedallas;
    public Button BtnVerTodoProgresos;
    public Button BtnInicio;

    public LeccionUI[] lecciones;
    public float duracionFade = 0.6f;

    private const float PROGRESO_MAX = 16.5f;

    void Start()
    {
        if (BtnVerTodoMedallas != null)
            BtnVerTodoMedallas.onClick.AddListener(IrAVerTodoMedallas);

        if (BtnVerTodoProgresos != null)
            BtnVerTodoProgresos.onClick.AddListener(IrAVerTodoProgresos);

        if (BtnInicio != null)
            BtnInicio.onClick.AddListener(IrAHome);

        for (int i = 0; i < lecciones.Length; i++)
            ConfigurarLeccion(lecciones[i]);
    }

    void ConfigurarLeccion(LeccionUI leccion)
    {
        bool anteriorTerminada = leccion.idLeccion == 1
            ? true
            : EstaLeccionTerminada(leccion.idLeccion - 1);

        float avLecc  = PlayerPrefs.GetFloat($"AvanceLeccion{leccion.idLeccion}", 0f);
        float avRelam = PlayerPrefs.GetFloat($"AvanceRelampago{leccion.idLeccion}", 0f);
        float avDin   = PlayerPrefs.GetFloat($"AvanceDinamica{leccion.idLeccion}", 0f);
        float total   = Mathf.Clamp(avLecc + avRelam + avDin, 0f, PROGRESO_MAX);
        float prog    = total / PROGRESO_MAX;
        bool estaTerminada = total >= PROGRESO_MAX;

        leccion.BarraLeccion.SetActive(true);
        leccion.ContenidoBarra.SetActive(false);

        leccion.FlechaDesplegable.gameObject.SetActive(anteriorTerminada);

        if (leccion.FlechaIcono != null)
            leccion.FlechaIcono.localRotation = Quaternion.Euler(0, 0, 180f);

        leccion.FlechaDesplegable.onClick.RemoveAllListeners();
        leccion.FlechaDesplegable.onClick.AddListener(() =>
        {
            if (!anteriorTerminada) return;

            bool activo = !leccion.ContenidoBarra.activeSelf;
            leccion.ContenidoBarra.SetActive(activo);

            if (leccion.FlechaIcono != null)
                leccion.FlechaIcono.localRotation = Quaternion.Euler(0, 0, activo ? 0f : 180f);
        });

        // Actualiza barra (0..1)
        if (leccion.BarraAmarilla != null)
        {
            Vector3 escala = leccion.BarraAmarilla.localScale;
            escala.x = prog;
            leccion.BarraAmarilla.localScale = escala;
        }

        SetAlpha(leccion.CandadoProgreso, anteriorTerminada ? 0f : 1f);
        SetAlpha(leccion.CandadoLeccion, estaTerminada ? 0f : 1f);
        SetAlpha(leccion.MedallaLeccion, estaTerminada ? 1f : 0f);

        if (estaTerminada)
            StartCoroutine(FadeCandadoLeccionAHaciaMedalla(leccion));
    }

    bool EstaLeccionTerminada(int idLeccion)
    {
        float avLecc  = PlayerPrefs.GetFloat($"AvanceLeccion{idLeccion}", 0f);
        float avRelam = PlayerPrefs.GetFloat($"AvanceRelampago{idLeccion}", 0f);
        float avDin   = PlayerPrefs.GetFloat($"AvanceDinamica{idLeccion}", 0f);
        float total   = Mathf.Clamp(avLecc + avRelam + avDin, 0f, PROGRESO_MAX);
        return total >= PROGRESO_MAX;
    }

    IEnumerator FadeCandadoLeccionAHaciaMedalla(LeccionUI leccion)
    {
        CanvasGroup fadeOut = leccion.CandadoLeccion;
        CanvasGroup fadeIn  = leccion.MedallaLeccion;

        float t = 0f;
        while (t < duracionFade)
        {
            float alpha = t / duracionFade;
            if (fadeOut != null) fadeOut.alpha = Mathf.Lerp(fadeOut.alpha, 0f, alpha);
            if (fadeIn  != null) fadeIn.alpha  = Mathf.Lerp(fadeIn.alpha, 1f, alpha);
            t += Time.deltaTime;
            yield return null;
        }
        if (fadeOut != null) fadeOut.alpha = 0f;
        if (fadeIn  != null) fadeIn.alpha  = 1f;
    }

    void SetAlpha(CanvasGroup cg, float a)
    {
        if (cg == null) return;
        cg.alpha = a;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public void IrAVerTodoMedallas()
    {
        SceneManager.LoadScene("Scenes/Central/MedallasCompleto");
    }

    public void IrAVerTodoProgresos()
    {
        SceneManager.LoadScene("Scenes/Central/ProgresoCompleto");
    }

    public void IrAHome()
    {
        SceneManager.LoadScene("Home");
    }
}
