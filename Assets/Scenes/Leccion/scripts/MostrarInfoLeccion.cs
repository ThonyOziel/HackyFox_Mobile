using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class MostrarInfoLeccion : MonoBehaviour
{
    [Header("Elementos de texto en pantalla")]
    public TMP_Text nomLeccionTMP;
    public TMP_Text tituloTMP;
    public TMP_Text subtituloTMP;
    public TMP_Text infoTMP;
    public TMP_Text consejoTMP;

    [Header("Archivo CSV")]
    public string nombreArchivo = "TablaLecciones.csv";

    private List<string[]> todasLasLecciones = new List<string[]>();

    void Start()
    {
        CargarLecciones();
        StartCoroutine(DemoRecorrido()); // inicia la demostración automática
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
            yield return new WaitForSeconds(2f); // espera 2 segundos antes de pasar a la siguiente
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