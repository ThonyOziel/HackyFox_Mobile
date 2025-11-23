using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// Manager global del menú flotante
/// Se puede usar en cualquier escena de la aplicación
public class FloatingMenuManager : MonoBehaviour
{
    public static FloatingMenuManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject menuPanel;
    public Button btnUsuario;
    public Button btnLecciones;
    public Button btnMascota;
    public Button btnRegresar;
    public Button btnClose;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Configurar botones
        if (btnUsuario != null)
            btnUsuario.onClick.AddListener(OnUsuarioClicked);

        if (btnLecciones != null)
            btnLecciones.onClick.AddListener(OnLeccionesClicked);

        if (btnMascota != null)
            btnMascota.onClick.AddListener(OnMascotaClicked);

        if (btnRegresar != null)
            btnRegresar.onClick.AddListener(OnRegresarClicked);

        if (btnClose != null)
            btnClose.onClick.AddListener(OnCloseClicked);

        // Ocultar menú al inicio
        if (menuPanel != null)
            menuPanel.SetActive(false);
    }


    /// Abrir/cerrar el menú
    public void ToggleMenu()
    {
        if (menuPanel != null)
        {
            bool isActive = menuPanel.activeSelf;
            menuPanel.SetActive(!isActive);
            Debug.Log($"Menú {(isActive ? "cerrado" : "abierto")}");
        }
    }


    /// Cerrar el menú
    public void CloseMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
            Debug.Log("Menú cerrado");
        }
    }

 
    /// Abrir el menú
    public void OpenMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(true);
            Debug.Log("Menú abierto");
        }
    }

    void OnUsuarioClicked()
    {
        Debug.Log("Botón Usuario presionado");
        CloseMenu();
        SceneManager.LoadScene("Home");
    }

    void OnLeccionesClicked()
    {
        Debug.Log("Botón Lecciones presionado");
        

        SceneManager.LoadScene("Lecciones"); ;
    }

    void OnMascotaClicked()
    {
        Debug.Log("Botón Mascota presionado");
       

        SceneManager.LoadScene("MenuRopa");
    }

    void OnRegresarClicked()
    {
        Debug.Log("Botón Regresar presionado");
        
    }

    void OnCloseClicked()
    {
        Debug.Log("Botón Cerrar presionado");
        SceneManager.LoadScene("Lecciones");
    }

    void OnDestroy()
    {
        if (btnUsuario != null)
            btnUsuario.onClick.RemoveListener(OnUsuarioClicked);
        if (btnLecciones != null)
            btnLecciones.onClick.RemoveListener(OnLeccionesClicked);
        if (btnMascota != null)
            btnMascota.onClick.RemoveListener(OnMascotaClicked);
        if (btnRegresar != null)
            btnRegresar.onClick.RemoveListener(OnRegresarClicked);
        if (btnClose != null)
            btnClose.onClick.RemoveListener(OnCloseClicked);
    }
}