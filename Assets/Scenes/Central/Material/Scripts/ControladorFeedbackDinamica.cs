using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorFeedbackDinamicaTouch : MonoBehaviour
{
    [Header("Configuración")]
    public int idLeccionActual = 1;

    private const float PROGRESO_MAX = 16.5f;
    private const float VALOR_MODULO = 5.5f;

    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("Este objeto necesita un BoxCollider para detectar el touch.");
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider == boxCollider)
                    {
                        SumarProgresoYFinalizar();
                    }
                }
            }
        }
    }

    void SumarProgresoYFinalizar()
    {
        float avDin = PlayerPrefs.GetFloat($"AvanceDinamica{idLeccionActual}", 0f);
        avDin = Mathf.Min(avDin + VALOR_MODULO, PROGRESO_MAX);
        PlayerPrefs.SetFloat($"AvanceDinamica{idLeccionActual}", avDin);
        PlayerPrefs.Save();

        Debug.Log($"Progreso dinámica L{idLeccionActual}: {avDin}/{PROGRESO_MAX}");

        RevisarFinalizacionLeccion();

        // ✅ Redirigir a la pantalla de Home
        SceneManager.LoadScene("Home");
    }

    void RevisarFinalizacionLeccion()
    {
        float avLecc  = PlayerPrefs.GetFloat($"AvanceLeccion{idLeccionActual}", 0f);
        float avRelam = PlayerPrefs.GetFloat($"AvanceRelampago{idLeccionActual}", 0f);
        float avDin   = PlayerPrefs.GetFloat($"AvanceDinamica{idLeccionActual}", 0f);

        float total = Mathf.Clamp(avLecc + avRelam + avDin, 0f, PROGRESO_MAX);

        if (total >= PROGRESO_MAX)
        {
            PlayerPrefs.SetInt($"Leccion{idLeccionActual}Completa", 1);
            PlayerPrefs.Save();

            var bar = UnityEngine.Object.FindFirstObjectByType<LeccionBarManager>();
            if (bar != null)
            {
                bar.StartCooldown();
                Debug.Log($"Cooldown 24h iniciado para L{idLeccionActual}");
            }

            Debug.Log($"Lección {idLeccionActual} completada (100%)");
        }
    }
}
