using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject feedbackPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI adviceText;
    public Button retryButton;
    public Button homeButton;        
    public Image foxImage;

    private Action onRetryCallback;

    void Awake()
    {
        Debug.Log("FeedbackManager - Awake iniciado");

        // Patrón Singleton - solo una instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
            Debug.Log("FeedbackManager Instance creada");
        }
        else
        {
            Debug.Log("FeedbackManager ya existe, destruyendo duplicado");
            Destroy(gameObject);
            return;
        }

        // Verificar referencias
        if (feedbackPanel == null)
            Debug.LogError("feedbackPanel es NULL en FeedbackManager");
        if (adviceText == null)
            Debug.LogError("adviceText es NULL en FeedbackManager");
        if (retryButton == null)
            Debug.LogError("retryButton es NULL en FeedbackManager");

        // Configurar botones
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(OnRetryClicked);
            Debug.Log("Listener agregado a retryButton");
        }

        if (homeButton != null)
        {
            homeButton.onClick.AddListener(OnHomeClicked);
            Debug.Log("Listener agregado a homeButton");
        }

        // Ocultar panel al inicio
        if (feedbackPanel != null)
        {
            feedbackPanel.SetActive(false);
            Debug.Log("FeedbackPanel ocultado al inicio");
        }
    }

    /// Muestra el panel de retroalimentación con mensaje personalizado
    public void ShowFeedback(string advice, Action onRetry = null)
    {
        Debug.Log($"ShowFeedback llamado con mensaje: {advice}");

        if (feedbackPanel == null)
        {
            Debug.LogError("feedbackPanel es NULL, no se puede mostrar");
            return;
        }

        // Configurar texto
        if (adviceText != null)
        {
            adviceText.text = advice;
            Debug.Log("Texto configurado");
        }
        else
        {
            Debug.LogError("adviceText es NULL");
        }

        // Guardar callback
        onRetryCallback = onRetry;

        // Mostrar panel
        feedbackPanel.SetActive(true);
        Debug.Log("FeedbackPanel activado (SetActive true)");
    }


    /// Oculta el panel de retroalimentación
    public void HideFeedback()
    {
        if (feedbackPanel != null)
            feedbackPanel.SetActive(false);
    }

    void OnRetryClicked()
    {
        // Ocultar panel
        HideFeedback();

        // Ejecutar callback si existe
        onRetryCallback?.Invoke();
    }

    void OnHomeClicked()
    {
        Debug.Log("Botón Home presionado en FeedbackPanel");

        // Ocultar el feedback
        HideFeedback();

        // Abrir el menú flotante
        if (FloatingMenuManager.Instance != null)
        {
            FloatingMenuManager.Instance.OpenMenu();
        }
        else
        {
            Debug.LogWarning("FloatingMenuManager no encontrado");
        }
    }

    void OnDestroy()
    {
        if (retryButton != null)
            retryButton.onClick.RemoveListener(OnRetryClicked);
        if (homeButton != null)
            homeButton.onClick.RemoveListener(OnHomeClicked);
    }
}