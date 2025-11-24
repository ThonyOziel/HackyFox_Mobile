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
    public Button BtnRespuesta1;
    public Button BtnRespuesta2;
    public Button BtnRespuesta3;
    public TextMeshProUGUI NomLeccionTMP;
    public TextMeshProUGUI TituloCuestionario;
    public TextMeshProUGUI TextBtn1;
    public TextMeshProUGUI TextBtn2;
    public TextMeshProUGUI TextBtn3;

    [Header("Archivo CSV")]
    public string nombreArchivo = "TablaRetoRelam.csv";
    public int idLeccionActual = 1;

    private List<string[]> todosLosRetos = new List<string[]>();
    private int respuestaCorrecta = -1;

    private const float PROGRESO_MAX = 16.5f;
    private const float VALOR_MODULO = 5.5f;

    void Start()
    {
        BtnRegresar.onClick.AddListener(RegresarALecciones);
        CargarRetos();
        MostrarRetoPorID(idLeccionActual);

        BtnRespuesta1.onClick.AddListener(() => VerificarRespuesta(1));
        BtnRespuesta2.onClick.AddListener(() => VerificarRespuesta(2));
        BtnRespuesta3.onClick.AddListener(() => VerificarRespuesta(3));
    }

    void RegresarALecciones()
    {
        SceneManager.LoadScene("Lecciones");
    }

    void CargarRetos()
    {
        string ruta = Path.Combine(Application.streamingAssetsPath, nombreArchivo);
        if (!File.Exists(ruta)) return;

        string[] lineas = File.ReadAllLines(ruta);
        for (int i = 1; i < lineas.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lineas[i])) continue;
            string[] campos = ParseCSVLine(lineas[i]);
            if (campos.Length >= 8)
                todosLosRetos.Add(campos);
        }
    }

    void MostrarRetoPorID(int idLeccion)
    {
        foreach (var reto in todosLosRetos)
        {
            if (int.TryParse(reto[1], out int id) && id == idLeccion)
            {
                NomLeccionTMP.text = reto[2];
                TituloCuestionario.text = reto[3];
                TextBtn1.text = reto[4];
                TextBtn2.text = reto[5];
                TextBtn3.text = reto[6];

                if (int.TryParse(reto[7], out int valida))
                    respuestaCorrecta = valida;

                return;
            }
        }
    }

    void VerificarRespuesta(int respuestaSeleccionada)
    {
        if (respuestaSeleccionada == respuestaCorrecta)
        {
            float avRelam = PlayerPrefs.GetFloat($"AvanceRelampago{idLeccionActual}", 0f);
            avRelam = Mathf.Min(avRelam + VALOR_MODULO, PROGRESO_MAX);
            PlayerPrefs.SetFloat($"AvanceRelampago{idLeccionActual}", avRelam);
            PlayerPrefs.Save();

            Debug.Log($"Progreso reto L{idLeccionActual}: {avRelam}/{PROGRESO_MAX}");

            SceneManager.LoadScene("Dinamica1");
        }
        else
        {
            Debug.Log("Respuesta incorrecta, intenta de nuevo.");
        }
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
