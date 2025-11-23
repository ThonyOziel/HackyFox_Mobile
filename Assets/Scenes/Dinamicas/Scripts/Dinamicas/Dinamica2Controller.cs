using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// Dinámica 2: Compartir o no compartir - 3 opciones
public class Dinamica2Controller : MonoBehaviour
{
    [Header("UI References")]
    public Button backButton;
    public TextMeshProUGUI titleText;

    [Header("Botones de Opciones")]
    public Button btnIncorrecto1;
    public Button btnIncorrecto2;
    public Button btnCorrecto;

    [Header("Animation Settings")]
    public float animationDuration = 0.8f;
    public float delayBetweenCards = 0.2f;
    public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Lesson Data")]
    public int idLeccion = 1;
    public string adviceText = "Esa opción no es adecuada para compartir en internet.";

    private Vector3[] cardStartPositions = new Vector3[3];
    private Vector3[] cardTargetPositions = new Vector3[3];
    private GameObject[] cards = new GameObject[3];
    private bool isAnimating = false;
    private bool buttonsEnabled = false;

    void Start()
    {
        Debug.Log("Inicializando Dinámica 2");

        // Configurar título
        if (titleText != null)
            titleText.text = "Lección 1: Escudo Seguro";

        // Guardar referencias de las tarjetas
        if (btnIncorrecto1 != null) cards[0] = btnIncorrecto1.gameObject;
        if (btnIncorrecto2 != null) cards[1] = btnIncorrecto2.gameObject;
        if (btnCorrecto != null) cards[2] = btnCorrecto.gameObject;

        // Configurar posiciones para animación
        SetupCardAnimations();

        // Configurar listeners de botones
        if (btnIncorrecto1 != null)
            btnIncorrecto1.onClick.AddListener(OnIncorrectAnswer);

        if (btnIncorrecto2 != null)
            btnIncorrecto2.onClick.AddListener(OnIncorrectAnswer);

        if (btnCorrecto != null)
            btnCorrecto.onClick.AddListener(OnCorrectAnswer);

        if (backButton != null)
            backButton.onClick.AddListener(OnBackButtonClicked);

        // Iniciar animación de entrada
        StartCoroutine(AnimateCardsIn());
    }

    void SetupCardAnimations()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null)
            {
                // Guardar posición objetivo (actual)
                cardTargetPositions[i] = cards[i].transform.localPosition;

                // Posición inicial (fuera de la pantalla, a la izquierda)
                cardStartPositions[i] = new Vector3(
                    cardTargetPositions[i].x - 1500f,
                    cardTargetPositions[i].y,
                    cardTargetPositions[i].z
                );

                // Mover a posición inicial
                cards[i].transform.localPosition = cardStartPositions[i];
            }
        }
    }

    IEnumerator AnimateCardsIn()
    {
        isAnimating = true;
        buttonsEnabled = false;

        // Animar cada tarjeta con un pequeño delay entre ellas
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null)
            {
                StartCoroutine(AnimateSingleCard(i));
                yield return new WaitForSeconds(delayBetweenCards);
            }
        }

        // Esperar a que termine la última animación
        yield return new WaitForSeconds(animationDuration);

        isAnimating = false;
        buttonsEnabled = true;
        Debug.Log("Animación completada - Botones habilitados");
    }

    IEnumerator AnimateSingleCard(int cardIndex)
    {
        if (cards[cardIndex] == null) yield break;

        float elapsed = 0f;
        Vector3 startPos = cardStartPositions[cardIndex];
        Vector3 targetPos = cardTargetPositions[cardIndex];

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            float curveValue = animationCurve.Evaluate(t);

            cards[cardIndex].transform.localPosition = Vector3.Lerp(startPos, targetPos, curveValue);
            yield return null;
        }

        cards[cardIndex].transform.localPosition = targetPos;
    }

    void OnCorrectAnswer()
    {
        if (!buttonsEnabled || isAnimating) return;

        Debug.Log("Respuesta correcta!");

        SceneManager.LoadScene("GoodFeedBack");

        SetButtonsInteractable(false);
    }

    void OnIncorrectAnswer()
    {
        if (!buttonsEnabled || isAnimating) return;

        Debug.Log("Respuesta incorrecta");

        // Verificar que el FeedbackManager existe
        if (FeedbackManager.Instance == null)
        {
            Debug.LogError("FeedbackManager.Instance es NULL!");
            return;
        }

        // Mostrar panel de feedback
        FeedbackManager.Instance.ShowFeedback(adviceText, OnRetry);

        // Deshabilitar botones
        SetButtonsInteractable(false);
    }

    void OnRetry()
    {
        Debug.Log("Reintentar dinámica");

        // Habilitar botones
        SetButtonsInteractable(true);

        // Reiniciar animación
        ResetAnimation();
    }

    void ResetAnimation()
    {
        // Mover tarjetas a posición inicial
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] != null)
            {
                cards[i].transform.localPosition = cardStartPositions[i];
            }
        }

        // Animar de nuevo
        StartCoroutine(AnimateCardsIn());
    }

    void SetButtonsInteractable(bool interactable)
    {
        buttonsEnabled = interactable;

        if (btnIncorrecto1 != null)
            btnIncorrecto1.interactable = interactable;
        if (btnIncorrecto2 != null)
            btnIncorrecto2.interactable = interactable;
        if (btnCorrecto != null)
            btnCorrecto.interactable = interactable;
    }

    void OnBackButtonClicked()
    {
        Debug.Log("Volver al menú de lecciones");
        // TODO: Mostrar diálogo de confirmación
        // TODO: Cargar escena del menú
    }

    void OnDestroy()
    {
        // Limpiar listeners
        if (btnIncorrecto1 != null)
            btnIncorrecto1.onClick.RemoveListener(OnIncorrectAnswer);
        if (btnIncorrecto2 != null)
            btnIncorrecto2.onClick.RemoveListener(OnIncorrectAnswer);
        if (btnCorrecto != null)
            btnCorrecto.onClick.RemoveListener(OnCorrectAnswer);
        if (backButton != null)
            backButton.onClick.RemoveListener(OnBackButtonClicked);
    }
}