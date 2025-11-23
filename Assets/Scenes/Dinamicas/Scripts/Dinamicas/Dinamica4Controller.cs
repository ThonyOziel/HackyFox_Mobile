using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// Dinámica 4: Detectives del buzón - Identificar correos seguros
public class Dinamica4Controller : MonoBehaviour
{
    [System.Serializable]
    public class EmailData
    {
        public Sprite emailSprite;
        public bool isGoodEmail; // true = correo bueno, false = correo malo
    }

    [Header("UI References")]
    public Button backButton;
    public Image currentEmailImage;
    public Button btnAceptar;
    public Button btnDenegar;
    public TextMeshProUGUI progressText;

    [Header("Lives System")]
    public GameObject[] lifeIcons; // Array de 3 imágenes de vidas

    [Header("Email Data")]
    public List<EmailData> emails = new List<EmailData>(); // Configurar en Inspector

    [Header("Animation Settings")]
    public float swipeAnimationDuration = 0.5f;
    public float swipeDistance = 1500f;

    [Header("Lesson Data")]
    public int idLeccion = 1;
    public string adviceText = "Recuerda verificar el remitente y no abrir correos sospechosos.";

    private int currentEmailIndex = 0;
    private int livesRemaining = 3;
    private bool isAnimating = false;
    private bool buttonsEnabled = true;

    void Start()
    {
        Debug.Log("Inicializando Dinámica 4");

        // Verificar que haya 7 correos configurados
        if (emails.Count != 7)
        {
            Debug.LogError("Debes configurar exactamente 7 correos en el Inspector!");
        }

        // Configurar botones
        if (btnAceptar != null)
            btnAceptar.onClick.AddListener(() => OnEmailResponse(true));

        if (btnDenegar != null)
            btnDenegar.onClick.AddListener(() => OnEmailResponse(false));

        if (backButton != null)
            backButton.onClick.AddListener(OnBackButtonClicked);

        // Mostrar primer correo
        ShowCurrentEmail();
        UpdateUI();
    }

    void ShowCurrentEmail()
    {
        if (currentEmailIndex < emails.Count)
        {
            currentEmailImage.sprite = emails[currentEmailIndex].emailSprite;
            Debug.Log($"Mostrando correo {currentEmailIndex + 1}/{emails.Count}");
        }
    }

    void OnEmailResponse(bool userAccepted)
    {
        if (isAnimating || !buttonsEnabled) return;

        EmailData currentEmail = emails[currentEmailIndex];
        bool isCorrect = false;

        // Verificar si la respuesta es correcta
        if (currentEmail.isGoodEmail)
        {
            // Correo bueno: debe ACEPTAR
            isCorrect = userAccepted;
        }
        else
        {
            // Correo malo: debe DENEGAR
            isCorrect = !userAccepted;
        }

        if (isCorrect)
        {
            Debug.Log("Respuesta correcta!");
            // Animar y pasar al siguiente correo
            StartCoroutine(SwipeEmailOut(true));
        }
        else
        {
            Debug.Log("Respuesta incorrecta!");
            // Perder una vida
            LoseLife();
        }
    }

    IEnumerator SwipeEmailOut(bool moveRight)
    {
        isAnimating = true;
        buttonsEnabled = false;

        float elapsed = 0f;
        Vector3 startPos = currentEmailImage.transform.localPosition;
        Vector3 targetPos = startPos + new Vector3(moveRight ? swipeDistance : -swipeDistance, 0, 0);

        // Animar deslizamiento
        while (elapsed < swipeAnimationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / swipeAnimationDuration;
            currentEmailImage.transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Mover al siguiente correo
        currentEmailIndex++;

        // Verificar si terminó
        if (currentEmailIndex >= emails.Count)
        {
            // ¡Completó todos los correos!
            OnAllEmailsCompleted();
        }
        else
        {
            // Resetear posición y mostrar siguiente correo
            currentEmailImage.transform.localPosition = startPos;
            ShowCurrentEmail();
            UpdateUI();
            buttonsEnabled = true;
        }

        isAnimating = false;
    }

    void LoseLife()
    {
        livesRemaining--;
        Debug.Log($"Vida perdida. Vidas restantes: {livesRemaining}");

        // Actualizar visualización de vidas
        UpdateLivesDisplay();

        if (livesRemaining <= 0)
        {
            // Game Over - Mostrar FeedbackPanel
            OnGameOver();
        }
        else
        {
            // Mostrar advertencia temporal
            StartCoroutine(ShowWarning());
        }
    }

    IEnumerator ShowWarning()
    {
        buttonsEnabled = false;

        // TODO: Mostrar mensaje temporal "Te quedan X intentos"
        Debug.LogWarning($"Te quedan {livesRemaining} intentos más");

        yield return new WaitForSeconds(1.5f);

        buttonsEnabled = true;
    }

    void UpdateLivesDisplay()
    {
        // Actualizar visualización de vidas (ocultar las perdidas)
        for (int i = 0; i < lifeIcons.Length; i++)
        {
            if (lifeIcons[i] != null)
            {
                lifeIcons[i].SetActive(i < livesRemaining);
            }
        }
    }

    void UpdateUI()
    {
        // Actualizar texto de progreso
        if (progressText != null)
        {
            progressText.text = $"Correo {currentEmailIndex + 1}/{emails.Count}";
        }
    }

    void OnAllEmailsCompleted()
    {
        Debug.Log("¡Todos los correos completados!");
        buttonsEnabled = false;

        //SceneManager.LoadScene("GoodFeedBack");
    }

    void OnGameOver()
    {
        Debug.Log("Game Over - 3 errores");
        buttonsEnabled = false;

        // Mostrar FeedbackPanel
        if (FeedbackManager.Instance != null)
        {
            FeedbackManager.Instance.ShowFeedback(adviceText, OnRetry);
        }
    }

    void OnRetry()
    {
        Debug.Log("Reintentar dinámica");

        // Reiniciar variables
        currentEmailIndex = 0;
        livesRemaining = 3;
        buttonsEnabled = true;

        // Resetear UI
        UpdateLivesDisplay();
        UpdateUI();
        ShowCurrentEmail();

        // Resetear posición de imagen
        currentEmailImage.transform.localPosition = Vector3.zero;
    }

    void OnBackButtonClicked()
    {
        Debug.Log("Volver al menú de lecciones");
        // TODO: Mostrar diálogo de confirmación
    }

    void OnDestroy()
    {
        if (btnAceptar != null)
            btnAceptar.onClick.RemoveAllListeners();
        if (btnDenegar != null)
            btnDenegar.onClick.RemoveAllListeners();
        if (backButton != null)
            backButton.onClick.RemoveListener(OnBackButtonClicked);
    }
}