using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    public TextAsset csvFile; // Aquí arrastraremos Tabla_Usuarios.csv

    public List<Usuario> LeerUsuarios()
    {
        List<Usuario> lista = new List<Usuario>();

        string[] filas = csvFile.text.Split('\n');

        // Saltamos la primera fila (encabezados)
        for (int i = 1; i < filas.Length; i++)
        {
            string fila = filas[i].Trim();
            if (string.IsNullOrEmpty(fila)) continue;

            string[] col = fila.Split(',');

            int id = int.Parse(col[0]);
            string correo = col[1];
            string pass = col[2];
            string fecha = col[3];

            Usuario u = new Usuario(id, correo, pass, fecha);
            lista.Add(u);
        }

        return lista;
    }
}

