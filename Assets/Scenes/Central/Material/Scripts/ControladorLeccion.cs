using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ControladorLeccion : MonoBehaviour
{
    [Header("Botones de fila")]
    public Button BtnFilaLeccion;
    public Button BtnFilaRelam;
    public Button BtnFilaDinamica;

    [Header("Iconos de fila")]
    public Image IconLeccion;
    public Image IconRelam;
    public Image IconDinamica;

    [Header("Sprites")]
    public Sprite spriteFlecha;
    public Sprite spriteCandado;

    [Header("Panel de advertencia")]
    public GameObject PanelTextBloq;
    public TextMeshProUGUI TextBloq;
    public Button BtnCerrarPanel;

    void Start()
    {
        BtnFilaLeccion.onClick.AddListener(AccionLeccion);
        BtnFilaRelam.onClick.AddListener(AccionRelam);
        BtnFilaDinamica.onClick.AddListener(AccionDinamica);

        BtnCerrarPanel.onClick.AddListener(() => PanelTextBloq.SetActive(false));
        PanelTextBloq.SetActive(false);

        ActualizarEstadoFilas();
    }

    void AccionLeccion()
    {
        PlayerPrefs.SetInt("AvanceLeccion1", 40);
        PlayerPrefs.SetInt("RelamDesbloqueado1", 1);

        ActualizarEstadoFilas();
        Debug.Log("Lección 1 completada: ¿Qué es ciberseguridad?");
    }

    void AccionRelam()
    {
        if (PlayerPrefs.GetInt("RelamDesbloqueado1", 0) != 1)
        {
            MostrarBloqueo("Debes completar la lección primero");
            return;
        }

        PlayerPrefs.SetInt("AvanceRelampago1", 30);
        PlayerPrefs.SetInt("DinamicaDesbloqueada1", 1);

        ActualizarEstadoFilas();
        Debug.Log("Reto relámpago 1 completado");

        SceneManager.LoadScene("RetoRelampago");
    }

    void AccionDinamica()
    {
        if (PlayerPrefs.GetInt("DinamicaDesbloqueada1", 0) != 1)
        {
            MostrarBloqueo("Debes completar el reto primero");
            return;
        }

        PlayerPrefs.SetInt("AvanceDinamica1", 30);
        PlayerPrefs.SetInt("Leccion2Desbloqueada", 1); // desbloquea la siguiente

        Debug.Log("Dinámica 1 completada: ¡Cuidado!");
    }

    void MostrarBloqueo(string mensaje)
    {
        PanelTextBloq.SetActive(true);
        TextBloq.text = mensaje;
    }

    void ActualizarEstadoFilas()
    {
        bool relam = PlayerPrefs.GetInt("RelamDesbloqueado1", 0) == 1;
        bool dinamica = PlayerPrefs.GetInt("DinamicaDesbloqueada1", 0) == 1;

        BtnFilaLeccion.interactable = true;
        BtnFilaRelam.interactable = true;
        BtnFilaDinamica.interactable = true;

        IconLeccion.sprite = spriteFlecha;
        IconRelam.sprite = relam ? spriteFlecha : spriteCandado;
        IconDinamica.sprite = dinamica ? spriteFlecha : spriteCandado;
    }
}
