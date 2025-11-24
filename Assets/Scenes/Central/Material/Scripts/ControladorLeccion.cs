using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System;

public class ControladorLeccion : MonoBehaviour
{
    [Header("Botones de fila")]
    public Button BtnFilaLeccion;
    public Button BtnFilaRelam;
    public Button BtnFilaDinamica;

    [Header("Iconos de fila")]
    public Image IconLeccion;
    public Image IconRelam;
    public Image IconDinamica;

    [Header("Sprites")]
    public Sprite spriteFlecha;
    public Sprite spriteCandado;

    [Header("Panel de advertencia")]
    public GameObject PanelTextBloq;
    public TextMeshProUGUI TextBloq;
    public Button BtnCerrarPanel;

    [Header("Botón de regreso")]
    public Button BtnRegresar;

    [Header("Elementos de texto")]
    public TextMeshProUGUI TituloTMP;
    public TextMeshProUGUI SubtituloTMP;
    public TextMeshProUGUI TeoriaTMP;
    public TextMeshProUGUI RetoTMP;
    public TextMeshProUGUI DinamicaTMP;

    [Header("Archivo CSV")]
    public string nombreArchivo = "TablaMenuLeccion.csv";
    public int idLeccionActual = 1;

    private List<string[]> todasLasLecciones = new List<string[]>();

    // Progreso modular
    private const float PROGRESO_MAX = 16.5f;
    private const float VALOR_MODULO = 5.5f;

    void Start()
    {
        BtnFilaLeccion.onClick.AddListener(AccionLeccion);
        BtnFilaRelam.onClick.AddListener(AccionRelam);
        BtnFilaDinamica.onClick.AddListener(AccionDinamica);
        BtnCerrarPanel.onClick.AddListener(() => PanelTextBloq.SetActive(false));
        BtnRegresar.onClick.AddListener(() => SceneManager.LoadScene("Home"));

        PanelTextBloq.SetActive(false);

        CargarLecciones();
        MostrarLeccionPorID(idLeccionActual);
        ActualizarEstadoFilas();

        VerificarCooldownYAvanceSiguienteLeccion();
    }

    void CargarLecciones()
    {
        string ruta = Path.Combine(Application.streamingAssetsPath, nombreArchivo);
        if (!File.Exists(ruta))
        {
            Debug.LogError("Archivo CSV no encontrado en: " + ruta);
            return;
        }

        string[] lineas = File.ReadAllLines(ruta);
        for (int i = 1; i < lineas.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lineas[i])) continue;
            string[] campos = ParseCSVLine(lineas[i]);
            if (campos.Length >= 6)
                todasLasLecciones.Add(campos);
        }

        Debug.Log("Lecciones cargadas: " + todasLasLecciones.Count);
    }

    void MostrarLeccionPorID(int idLeccion)
    {
        foreach (var leccion in todasLasLecciones)
        {
            if (int.TryParse(leccion[0], out int id) && id == idLeccion)
            {
                TituloTMP.text = leccion[1];
                SubtituloTMP.text = leccion[2];
                TeoriaTMP.text = leccion[3];
                RetoTMP.text = leccion[4];
                DinamicaTMP.text = leccion[5];
                Debug.Log($"Mostrando lección {idLeccion}: {leccion[1]}");
                return;
            }
        }
        Debug.LogWarning("No se encontró contenido para la lección: " + idLeccion);
    }

    void AccionLeccion()
    {
        float avLecc = PlayerPrefs.GetFloat($"AvanceLeccion{idLeccionActual}", 0f);
        avLecc = Mathf.Min(avLecc + VALOR_MODULO, PROGRESO_MAX);
        PlayerPrefs.SetFloat($"AvanceLeccion{idLeccionActual}", avLecc);
        PlayerPrefs.Save();

        Debug.Log($"Progreso teoría L{idLeccionActual}: {avLecc}/{PROGRESO_MAX}");

        ActualizarEstadoFilas();
        SceneManager.LoadScene("Leccion");

        RevisarFinalizacionLeccion();
    }

    void AccionRelam()
    {
        if (!LeccionDesbloqueadaParaReto())
        {
            MostrarBloqueo("Debes completar la lección primero");
            return;
        }

        float avRelam = PlayerPrefs.GetFloat($"AvanceRelampago{idLeccionActual}", 0f);
        avRelam = Mathf.Min(avRelam + VALOR_MODULO, PROGRESO_MAX);
        PlayerPrefs.SetFloat($"AvanceRelampago{idLeccionActual}", avRelam);
        PlayerPrefs.Save();

        Debug.Log($"Progreso reto L{idLeccionActual}: {avRelam}/{PROGRESO_MAX}");

        ActualizarEstadoFilas();
        SceneManager.LoadScene("RetoRelampago");

        RevisarFinalizacionLeccion();
    }

    void AccionDinamica()
    {
        if (!LeccionDesbloqueadaParaDinamica())
        {
            MostrarBloqueo("Debes completar el reto primero");
            return;
        }

        float avDin = PlayerPrefs.GetFloat($"AvanceDinamica{idLeccionActual}", 0f);
        avDin = Mathf.Min(avDin + VALOR_MODULO, PROGRESO_MAX);
        PlayerPrefs.SetFloat($"AvanceDinamica{idLeccionActual}", avDin);
        PlayerPrefs.Save();

        Debug.Log($"Progreso dinámica L{idLeccionActual}: {avDin}/{PROGRESO_MAX}");

        SceneManager.LoadScene("Dinamica1");

        RevisarFinalizacionLeccion();
    }

    bool LeccionDesbloqueadaParaReto()
    {
        float avLecc = PlayerPrefs.GetFloat($"AvanceLeccion{idLeccionActual}", 0f);
        return avLecc >= VALOR_MODULO;
    }

    bool LeccionDesbloqueadaParaDinamica()
    {
        float avRelam = PlayerPrefs.GetFloat($"AvanceRelampago{idLeccionActual}", 0f);
        return avRelam >= VALOR_MODULO;
    }

    void RevisarFinalizacionLeccion()
    {
        float total = GetTotalProgreso(idLeccionActual);
        if (total >= PROGRESO_MAX)
        {
            PlayerPrefs.SetFloat($"TotalLeccion{idLeccionActual}", PROGRESO_MAX);
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

    float GetTotalProgreso(int id)
    {
        float avLecc = PlayerPrefs.GetFloat($"AvanceLeccion{id}", 0f);
        float avRelam = PlayerPrefs.GetFloat($"AvanceRelampago{id}", 0f);
        float avDin = PlayerPrefs.GetFloat($"AvanceDinamica{id}", 0f);
        return Mathf.Clamp(avLecc + avRelam + avDin, 0f, PROGRESO_MAX);
    }

    void MostrarBloqueo(string mensaje)
    {
        PanelTextBloq.SetActive(true);
        TextBloq.text = mensaje;
    }

    void ActualizarEstadoFilas()
    {
        BtnFilaLeccion.interactable = true;
        BtnFilaRelam.interactable = LeccionDesbloqueadaParaReto();
        BtnFilaDinamica.interactable = LeccionDesbloqueadaParaDinamica();

        IconLeccion.sprite = spriteFlecha;
        IconRelam.sprite = BtnFilaRelam.interactable ? spriteFlecha : spriteCandado;
        IconDinamica.sprite = BtnFilaDinamica.interactable ? spriteFlecha : spriteCandado;
    }

    void VerificarCooldownYAvanceSiguienteLeccion()
    {
        var bar = UnityEngine.Object.FindFirstObjectByType<LeccionBarManager>();

        if (bar != null && bar.EstaDesbloqueada())
        {
            if (PlayerPrefs.GetInt($"Leccion{idLeccionActual}Completa", 0) == 1)
            {
                int siguiente = idLeccionActual + 1;
                MostrarLeccionPorID(siguiente);
                Debug.Log($"Cooldown terminado. Mostrar siguiente lección del CSV: {siguiente}");
            }
        }
    }

    string[] ParseCSVLine(string line)
    {
        var pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        string[] rawFields = Regex.Split(line, pattern);

        for (int i = 0; i < rawFields.Length; i++)
        {
            rawFields[i] = rawFields[i].Trim().Trim('"').Replace("\"\"", "\"");
        }

        return rawFields;
    }
}
