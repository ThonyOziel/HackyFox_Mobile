using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DatosUsuarios
{
    public static List<Usuario> listaUsuarios = new List<Usuario>();

    // Se ejecuta automáticamente al iniciar el juego
    static DatosUsuarios()
    {
        CargarUsuarios();
    }

    // Método para leer el archivo CSV y llenar la lista
    public static void CargarUsuarios()
    {
        listaUsuarios.Clear();

        string ruta = Path.Combine(Application.streamingAssetsPath, "Tabla_Usuarios.csv");

        if (!File.Exists(ruta))
        {
            Debug.LogError("Archivo Tabla_Usuarios.csv no encontrado en StreamingAssets.");
            return;
        }

        string[] filas = File.ReadAllLines(ruta);

        // Saltamos la primera fila (encabezados)
        for (int i = 1; i < filas.Length; i++)
        {
            string fila = filas[i].Trim();
            if (string.IsNullOrEmpty(fila)) continue;

            string[] col = fila.Split(',');
            if (col.Length < 4) continue;

            int id = int.Parse(col[0]);
            string correo = col[1];
            string pass = col[2];
            string fecha = col[3];

            Usuario u = new Usuario(id, correo, pass, fecha);
            listaUsuarios.Add(u);
        }

        Debug.Log("Usuarios cargados: " + listaUsuarios.Count);
    }

    // Método para guardar un nuevo usuario en el archivo CSV
    public static void GuardarUsuario(Usuario nuevo)
    {
        string ruta = Path.Combine(Application.streamingAssetsPath, "Tabla_Usuarios.csv");
        string nuevaLinea = $"{nuevo.id_usuario},{nuevo.correo},{nuevo.contraseña},{nuevo.fecha_registro}";
        File.AppendAllText(ruta, "\n" + nuevaLinea);
    }
}
