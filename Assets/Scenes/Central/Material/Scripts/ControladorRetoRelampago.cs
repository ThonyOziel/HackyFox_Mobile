using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class ControladorRetoRelampago : MonoBehaviour
{
    [Header("Elementos UI")]
    public Button BtnRegresar;
    public TextMeshProUGUI NomLeccionTMP;
    public TextMeshProUGUI TituloCuestionario;
    public TextMeshProUGUI TextBtn1;
    public TextMeshProUGUI TextBtn2;
    public TextMeshProUGUI TextBtn3;

    [Header("Archivo CSV")]
    public string nombreArchivo = "TablaRetoRelam.csv";
    public int idLeccionActual = 1; // se puede cambiar dinámicamente

    private List<string[]> todosLosRetos = new List<string[]>();

    void Start()
    {
        BtnRegresar.onClick.AddListener(RegresarALecciones);
        CargarRetos();
        MostrarRetoPorID(idLeccionActual);
    }

    void RegresarALecciones()
    {
        SceneManager.LoadScene("Lecciones");
    }

    void CargarRetos()
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
            if (campos.Length >= 8)
                todosLosRetos.Add(campos);
        }

        Debug.Log("Retos cargados: " + todosLosRetos.Count);
    }

    void MostrarRetoPorID(int idLeccion)
    {
        foreach (var reto in todosLosRetos)
        {
            if (int.TryParse(reto[1], out int id) && id == idLeccion)
            {
                NomLeccionTMP.text = reto[2];           // ← nombre de la lección
                TituloCuestionario.text = reto[3];      // ← pregunta
                TextBtn1.text = reto[4];                // ← respuesta 1
                TextBtn2.text = reto[5];                // ← respuesta 2
                TextBtn3.text = reto[6];                // ← respuesta 3
                Debug.Log($"Mostrando reto de lección {idLeccion}: {reto[3]}");
                return;
            }
        }

        Debug.LogWarning("No se encontró reto para la lección: " + idLeccion);
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
