using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class ConsejoBtn : MonoBehaviour
{
    public GameObject globoTextoGO;       // Objeto del globo
    public TMP_Text globoTexto;           // Componente TextMeshPro dentro del globo
    public AudioClip sonidoConsejo;       // Clip de sonido
    string nombreArchivo = "TablaFrases.csv";
    private List<string> frases = new List<string>();

    void Start()
    {
        CargarFrases();
        globoTextoGO.SetActive(false);              // Oculta el globo
        globoTexto.gameObject.SetActive(false);     // Oculta el texto
    }

    void CargarFrases()
    {
        string ruta = Path.Combine(Application.streamingAssetsPath, nombreArchivo);
        if (File.Exists(ruta))
        {
            string[] lineas = File.ReadAllLines(ruta);
            for (int i = 1; i < lineas.Length; i++) // saltar encabezado
            {
                if (!string.IsNullOrWhiteSpace(lineas[i]))
                    frases.Add(lineas[i].Trim());
            }
        }
        else
        {
            Debug.LogError("Archivo CSV no encontrado en: " + ruta);
        }
    }

    public void MostrarConsejo()
    {
        if (frases.Count == 0) return;

        string frase = frases[Random.Range(0, frases.Count)];
        globoTexto.text = frase;

        globoTextoGO.SetActive(true);               // Muestra el globo
        globoTexto.gameObject.SetActive(true);      // Muestra el texto

        if (sonidoConsejo != null)
            AudioSource.PlayClipAtPoint(sonidoConsejo, transform.position);

        StopAllCoroutines();
        StartCoroutine(OcultarGloboTrasTiempo());
    }

    IEnumerator OcultarGloboTrasTiempo()
    {
        yield return new WaitForSeconds(10f);

        globoTextoGO.SetActive(false);              // Oculta el globo
        globoTexto.gameObject.SetActive(false);     // Oculta el texto
    }

    void OnMouseDown()
    {
        MostrarConsejo();
    }
}