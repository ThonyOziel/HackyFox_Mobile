using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class InicioDeSesion : MonoBehaviour
{
    [Header("Campos de texto")]
    public TMP_InputField inputUsuario;
    public TMP_InputField inputContrasena;

    public void IniciarSesion()
    {
        string usuario = inputUsuario.text;
        string contrasena = inputContrasena.text;

        // Validación simple (luego conectamos con tu BD si quieres)
        if (usuario == "" || contrasena == "")
        {
            Debug.Log("Debe completar todos los campos.");
            return;
        }

        Debug.Log("Inicio de sesión exitoso.");
        // Cambia a otra escena si corresponde
        // SceneManager.LoadScene("NombreEscenaPrincipal");
    }

    public void Volver()
    {
        SceneManager.LoadScene("Bienvenida");
    }
}

