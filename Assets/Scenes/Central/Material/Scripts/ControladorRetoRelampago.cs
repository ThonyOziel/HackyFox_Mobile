using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ControladorRetoRelampago : MonoBehaviour
{
    public Button BtnRegresar;
    public TextMeshProUGUI TituloCuestionario;
    public TextMeshProUGUI TextBtn1;
    public TextMeshProUGUI TextBtn2;
    public TextMeshProUGUI TextBtn3;

    void Start()
    {
        BtnRegresar.onClick.AddListener(RegresarALecciones);
        CargarContenido();
    }

    void RegresarALecciones()
    {
        SceneManager.LoadScene("Lecciones");
    }

    void CargarContenido()
    {
        TituloCuestionario.text = "¿Qué es la ciberseguridad?";
        TextBtn1.text = "Algo que te protege cuando usas internet";
        TextBtn2.text = "Un videojuego";
        TextBtn3.text = "Un superhéroe que cuida tu compu";
    }
}
