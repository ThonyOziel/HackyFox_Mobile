using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Dinamica1Controller : MonoBehaviour
{
    [Header("UI References")]
    public GameObject phoneObject;           // El objeto phone_0
    public Button correctButton;             // Botón correcto
    public Button incorrectButton;           // Botón incorrecto
    public Button backButton;                // Botón de regresar

    [Header("Texts")]
    public TextMeshProUGUI questionTitle;    // Título "¡CUIDADO!"
    public TextMeshProUGUI questionText;     // Pregunta

    [Header("Animation Settings")]
    public float animationDuration = 1f;
    public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Lesson Data")]
    public int idLeccion = 1;
    public string adviceText = "Recuerda que tu información personal no debes compartirla con nadie en internet!";

    private Vector3 phoneStartPosition;
    private Vector3 phoneTargetPosition;
    private bool isAnimating = false;

    void Start()
    {
        Debug.Log("?? Dinamica1Controller - Start iniciado");

        // Configurar posiciones para animación
        if (phoneObject != null)
        {
            phoneTargetPosition = phoneObject.transform.localPosition;
            phoneStartPosition = new Vector3(phoneTargetPosition.x, phoneTargetPosition.y - 2000f, phoneTargetPosition.z);
            phoneObject.transform.localPosition = phoneStartPosition;
            Debug.Log("Teléfono configurado");
        }
        else
        {
            Debug.LogError("phoneObject es NULL");
        }

        // Asignar listeners a los botones
        if (correctButton != null)
        {
            correctButton.onClick.AddListener(OnCorrectAnswer);
            Debug.Log("Listener agregado a correctButton");
        }
        else
        {
            Debug.LogError("correctButton es NULL");
        }

        if (incorrectButton != null)
        {
            incorrectButton.onClick.AddListener(OnIncorrectAnswer);
            Debug.Log("? Listener agregado a incorrectButton");
        }
        else
        {
            Debug.LogError("? incorrectButton es NULL");
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
            Debug.Log("Listener agregado a backButton");
        }
        else
        {
            Debug.LogError("backButton es NULL");
        }

        // Iniciar animación del teléfono
        StartCoroutine(AnimatePhone());
    }

    IEnumerator AnimatePhone()
    {
        isAnimating = true;
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationDuration;
            float curveValue = animationCurve.Evaluate(t);

            phoneObject.transform.localPosition = Vector3.Lerp(phoneStartPosition, phoneTargetPosition, curveValue);
            yield return null;
        }

        phoneObject.transform.localPosition = phoneTargetPosition;
        isAnimating = false;
    }

    void OnCorrectAnswer()
    {
        if (isAnimating) return;

        Debug.Log("Respuesta correcta!");

        // Deshabilitar botones
        SetButtonsInteractable(false);

        // Guardar progreso en BD
        if (ProgresoManager.Instance != null)
        {
            ProgresoManager.Instance.CompletarDinamica(idLeccion);
        }

        GoodFeedbackController.Show(
            lessonId: idLeccion,
            message: "¡Excelente! Has identificado correctamente el mensaje sospechoso.",
            nextScene: SceneLoader.MENU_LECCIONES // O la siguiente dinámica
        );
    }

    void OnIncorrectAnswer()
    {
        Debug.Log("BOTÓN INCORRECTO PRESIONADO!!!");

        if (isAnimating) return;

        Debug.Log("Respuesta incorrecta - Botón presionado");

        // Verificar que el FeedbackManager existe
        if (FeedbackManager.Instance == null)
        {
            Debug.LogError("FeedbackManager.Instance es NULL! Asegúrate de tener el FeedbackManager en la escena.");
            return;
        }

        Debug.Log("feedbackManager encontrado, mostrando feedback...");

        // Llamar al FeedbackManager global
        FeedbackManager.Instance.ShowFeedback(adviceText, OnRetry);

        // Deshabilitar botones mientras se muestra el feedback
        SetButtonsInteractable(false);
    }

    void OnRetry()
    {
        // Esta función se ejecuta cuando el usuario presiona "Intentarlo de nuevo"

        // Rehabilitar botones
        SetButtonsInteractable(true);

        // Reiniciar animación del teléfono
        phoneObject.transform.localPosition = phoneStartPosition;
        StartCoroutine(AnimatePhone());
    }

    void SetButtonsInteractable(bool interactable)
    {
        if (correctButton != null)
            correctButton.interactable = interactable;
        if (incorrectButton != null)
            incorrectButton.interactable = interactable;
    }

    void OnBackButtonClicked()
    {
        // TODO: Mostrar diálogo de confirmación
        Debug.Log("Usuario quiere regresar - mostrar confirmación");

        // Si confirma, regresar al menú sin guardar progreso
    }

    void OnDestroy()
    {
        // Limpiar listeners
        if (correctButton != null)
            correctButton.onClick.RemoveListener(OnCorrectAnswer);
        if (incorrectButton != null)
            incorrectButton.onClick.RemoveListener(OnIncorrectAnswer);
        if (backButton != null)
            backButton.onClick.RemoveListener(OnBackButtonClicked);
    }
}