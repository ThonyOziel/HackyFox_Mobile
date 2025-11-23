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

        // Validación simple
        if (usuario == "" || contrasena == "")
        {
            Debug.Log("Debe completar todos los campos.");
            return;
        }

        
        var user = DatosUsuarios.listaUsuarios.Find(u =>
            u.correo == usuario && u.contraseña == contrasena
        );

        if (user == null)
        {
            Debug.Log("Revisa que tu usuario y contraseña sean correctos.");
            return;
        }

        Debug.Log("Inicio de sesión exitoso.");

       
        SceneManager.LoadScene("Home");
    }

    public void Volver()
    {
        SceneManager.LoadScene("Bienvenida");
    }
}


