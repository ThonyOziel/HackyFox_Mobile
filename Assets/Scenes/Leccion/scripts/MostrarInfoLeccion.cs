using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MostrarInfoLeccion : MonoBehaviour
{
    [Header("Elementos de texto en pantalla")]
    public TMP_Text nomLeccionTMP;
    public TMP_Text tituloTMP;
    public TMP_Text subtituloTMP;
    public TMP_Text infoTMP;
    public TMP_Text consejoTMP;

    [Header("Botones de navegación")]
    public Button BtnRegresar_0;          // Regresa a pantalla Lecciones
    public Button FeedBackContinuarBtn_0; // Avanza a RetoRelampago

    [Header("Archivo CSV")]
    public string nombreArchivo = "TablaLecciones.csv";
    public int idLeccionActual = 1;

    private List<string[]> todasLasLecciones = new List<string[]>();

    private const float PROGRESO_MAX = 16.5f;
    private const float VALOR_MODULO = 5.5f;

    void Start()
    {
        CargarLecciones();
        StartCoroutine(DemoRecorrido());

        ConfigurarBotones();
    }

    void ConfigurarBotones()
    {
        if (BtnRegresar_0 != null)
        {
            BtnRegresar_0.onClick.RemoveAllListeners();
            BtnRegresar_0.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Lecciones");
            });
        }

        if (FeedBackContinuarBtn_0 != null)
        {
            FeedBackContinuarBtn_0.onClick.RemoveAllListeners();
            FeedBackContinuarBtn_0.onClick.AddListener(() =>
            {
                float avLecc = PlayerPrefs.GetFloat($"AvanceLeccion{idLeccionActual}", 0f);
                avLecc = Mathf.Min(avLecc + VALOR_MODULO, PROGRESO_MAX);
                PlayerPrefs.SetFloat($"AvanceLeccion{idLeccionActual}", avLecc);
                PlayerPrefs.Save();

                Debug.Log($"Progreso teoría L{idLeccionActual}: {avLecc}/{PROGRESO_MAX}");

                SceneManager.LoadScene("RetoRelampago");
            });
        }
    }

    void CargarLecciones()
    {
        string ruta = Path.Combine(Application.streamingAssetsPath, nombreArchivo);
        if (!File.Exists(ruta)) return;

        string[] lineas = File.ReadAllLines(ruta);
        for (int i = 1; i < lineas.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lineas[i])) continue;
            string[] campos = ParseCSVLine(lineas[i]);
            if (campos.Length >= 6)
                todasLasLecciones.Add(campos);
        }
    }

    IEnumerator DemoRecorrido()
    {
        for (int i = 0; i < todasLasLecciones.Count; i++)
        {
            ActualizarTextos(todasLasLecciones[i]);
            yield return new WaitForSeconds(2f);
        }
    }

    void ActualizarTextos(string[] campos)
    {
        nomLeccionTMP.text = campos[1];
        tituloTMP.text = campos[2];
        subtituloTMP.text = campos[3];
        infoTMP.text = campos[4];
        consejoTMP.text = campos[5];
    }

    string[] ParseCSVLine(string line)
    {
        var pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        string[] rawFields = Regex.Split(line, pattern);

        for (int i = 0; i < rawFields.Length; i++)
            rawFields[i] = rawFields[i].Trim().Trim('"').Replace("\"\"", "\"");

        return rawFields;
    }
}
