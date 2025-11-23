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
        if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasena))
        {
            Debug.Log("Debe completar todos los campos.");
            return;
        }

        // Buscar usuario en la lista
        var user = DatosUsuarios.listaUsuarios.Find(u =>
            u.correo.Trim() == usuario.Trim() && u.contraseña.Trim() == contrasena.Trim()
        );

        if (user == null)
        {
            Debug.Log("Revisa que tu usuario y contraseña sean correctos.");
            return;
        }

        Debug.Log("Inicio de sesión exitoso.");

        // Guardamos el correo del usuario actual para usarlo en otras escenas
        UsuarioActual.correo = user.correo;

        // Detectar si es la primera vez (no tiene mascota)
        if (string.IsNullOrEmpty(user.nombre_mascota))
        {
            SceneManager.LoadScene("Alias"); // primera vez ? elegir nombre de mascota
        }
        else
        {
            SceneManager.LoadScene("Home"); // ya tiene mascota ? ir al Home
        }
    }

    public void Volver()
    {
        SceneManager.LoadScene("Bienvenida");
    }
}


