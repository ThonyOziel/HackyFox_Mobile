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

        Debug.Log("Registro completado.");
        // Aquí luego guardaremos en base de datos si quieres
    }

    public void Volver()
    {
        SceneManager.LoadScene("Bienvenida");
    }
}

