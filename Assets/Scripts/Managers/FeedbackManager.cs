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
    public Image foxImage;

    private Action onRetryCallback;

    void Awake()
    {
        Debug.Log(" FeedbackManager - Awake iniciado");

        // Patrón Singleton - solo una instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
            Debug.Log(" FeedbackManager Instance creada");
        }
        else
        {
            Debug.Log(" FeedbackManager ya existe, destruyendo duplicado");
            Destroy(gameObject);
            return;
        }

        // Verificar referencias
        if (feedbackPanel == null)
            Debug.LogError(" feedbackPanel es NULL en FeedbackManager");
        if (adviceText == null)
            Debug.LogError(" adviceText es NULL en FeedbackManager");
        if (retryButton == null)
            Debug.LogError(" retryButton es NULL en FeedbackManager");

        // Configurar botón
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(OnRetryClicked);
            Debug.Log(" Listener agregado a retryButton");
        }

        // Ocultar panel al inicio
        if (feedbackPanel != null)
        {
            feedbackPanel.SetActive(false);
            Debug.Log(" FeedbackPanel ocultado al inicio");
        }
    }

   
    /// Muestra el panel de retroalimentación con mensaje personalizado
    public void ShowFeedback(string advice, Action onRetry = null)
    {
        if (feedbackPanel == null) return;

        // Configurar texto
        if (adviceText != null)
            adviceText.text = advice;

        // Guardar callback
        onRetryCallback = onRetry;

        // Mostrar panel
        feedbackPanel.SetActive(true);
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

    void OnDestroy()
    {
        if (retryButton != null)
            retryButton.onClick.RemoveListener(OnRetryClicked);
    }
}