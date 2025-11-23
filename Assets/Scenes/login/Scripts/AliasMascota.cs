using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class AliasMascota : MonoBehaviour
{
    public TMP_InputField inputNombreMascota;

    public void ConfirmarNombre()
    {
        string nombre = inputNombreMascota.text;

        if (string.IsNullOrEmpty(nombre))
        {
            Debug.Log("Debes ingresar un nombre para tu mascota.");
            return;
        }

        // Buscar usuario actual por correo
        Usuario actual = DatosUsuarios.listaUsuarios.Find(u => u.correo == UsuarioActual.correo);

        if (actual != null)
        {
            // Guardar el nombre en memoria
            actual.nombre_mascota = nombre;

            // Reescribir el archivo CSV con todos los usuarios actualizados
            DatosUsuarios.ActualizarArchivo();

            Debug.Log("Mascota guardada: " + nombre);
        }

        // Ir al Home
        SceneManager.LoadScene("Home");
    }
}
