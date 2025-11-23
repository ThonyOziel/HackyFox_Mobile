using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Registro : MonoBehaviour
{
    [Header("Campos de texto")]
    public TMP_InputField inputUsuario;
    public TMP_InputField inputContrasena;
    public TMP_InputField inputConfirmar;

    public void Registrar()
    {
        string usuario = inputUsuario.text;
        string contrasena = inputContrasena.text;
        string confirmar = inputConfirmar.text;

        if (usuario == "" || contrasena == "" || confirmar == "")
        {
            Debug.Log("Debes completar todos los campos.");
            return;
        }

        if (contrasena != confirmar)
        {
            Debug.Log("Las contraseñas no coinciden.");
            return;
        }

        // ?? Evitar registros duplicados
        if (DatosUsuarios.listaUsuarios.Exists(u => u.correo == usuario))
        {
            Debug.Log("Este correo ya está registrado.");
            return;
        }

        // Crear nuevo usuario
        Usuario nuevo = new Usuario(
            DatosUsuarios.listaUsuarios.Count + 1,
            usuario,
            contrasena,
            System.DateTime.Now.ToString()
        );

        // Guardar en la "base de datos"
        DatosUsuarios.listaUsuarios.Add(nuevo);
        DatosUsuarios.GuardarUsuario(nuevo);

        Debug.Log("Registro completado.");

        // ?? Mandar al Home
        SceneManager.LoadScene("Home");
    }

    public void Volver()
    {
        SceneManager.LoadScene("Bienvenida");
    }
}


