using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bienvenida : MonoBehaviour
{
    [Header("Botones")]
    public Button BtnLogin;
    public Button BtnRegistro;
    public Button BtnSalir;

    void Start()
    {
        BtnLogin.onClick.AddListener(AbrirInicioSesion);
        BtnRegistro.onClick.AddListener(AbrirRegistro);
        BtnSalir.onClick.AddListener(SalirApp);
    }

    void AbrirInicioSesion()
    {
        SceneManager.LoadScene("InicioDeSesion");
    }

    void AbrirRegistro()
    {
        SceneManager.LoadScene("Registro");
    }

    void SalirApp()
    {
        
        Application.Quit();

     
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}




