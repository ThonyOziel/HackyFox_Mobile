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

    private List<string[]> todasLasLecciones = new List<string[]>();

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
                Debug.Log("BtnRegresar_0 presionado → Cargando escena Lecciones");
                SceneManager.LoadScene("Lecciones");
            });
        }
        else
        {
            Debug.LogError("BtnRegresar_0 no está asignado en el Inspector");
        }

        if (FeedBackContinuarBtn_0 != null)
        {
            FeedBackContinuarBtn_0.onClick.RemoveAllListeners();
            FeedBackContinuarBtn_0.onClick.AddListener(() =>
            {
                Debug.Log("FeedBackContinuarBtn_0 presionado → Cargando escena RetoRelampago");
                SceneManager.LoadScene("RetoRelampago");
            });
        }
        else
        {
            Debug.LogError("FeedBackContinuarBtn_0 no está asignado en el Inspector");
        }
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
        for (int i = 1; i < lineas.Length; i++) // saltar encabezado
        {
            if (string.IsNullOrWhiteSpace(lineas[i])) continue;
            string[] campos = ParseCSVLine(lineas[i]);
            if (campos.Length >= 6)
                todasLasLecciones.Add(campos);
        }

        Debug.Log("Lecciones cargadas: " + todasLasLecciones.Count);
    }

    IEnumerator DemoRecorrido()
    {
        for (int i = 0; i < todasLasLecciones.Count; i++)
        {
            ActualizarTextos(todasLasLecciones[i]);
            Debug.Log($"Mostrando lección #{i + 1}: {nomLeccionTMP.text}");
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
        {
            rawFields[i] = rawFields[i].Trim().Trim('"').Replace("\"\"", "\"");
        }

        return rawFields;
    }
}
