using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// Dinámica 3: Detective de Contraseñas - Quiz de casos
public class Dinamica3Controller : MonoBehaviour
{
    [System.Serializable]
    public class CaseData
    {
        public Sprite caseImage;
        public string questionText;
        public bool correctAnswerIsYes; // true = SI es correcto, false = NO es correcto
        public string advice; // Consejo si se equivoca
    }

    [Header("UI References")]
    public Button backButton;
    public Image caseImage;
    public TextMeshProUGUI questionText;
    public Button btnYes;
    public Button btnNo;
    public GameObject hackyObject; 

    [Header("Relax Panel")]
    public GameObject relaxPanel;
    public Button continueButton;

    [Header("Case Data")]
    public Sprite folderSprite; // Imagen del folder inicial
    public List<CaseData> cases = new List<CaseData>(); // 3 casos

    [Header("Animation Settings")]
    public float swipeAnimationDuration = 0.5f;
    public float swipeDistance = 1500f;
    public float hackyAnimationDuration = 1.5f; // Duración de la animación de Hacky

    [Header("Lesson Data")]
    public int idLeccion = 1;

    private int currentCaseIndex = -1; // -1 = pregunta inicial "¿Estás listo?"
    private bool isAnimating = false;
    private bool buttonsEnabled = true;
    private Vector3 hackyStartPosition;
    private Vector3 hackyTargetPosition;

    void Start()
    {
        Debug.Log("Inicializando Dinámica 3");

        // Verificar que haya 3 casos
        if (cases.Count != 3)
        {
            Debug.LogError("Debes configurar exactamente 3 casos en el Inspector!");
        }

        // Configurar animación de Hacky
        if (hackyObject != null)
        {
            hackyStartPosition = hackyObject.transform.localPosition;
            // Hacky bajará hasta desaparecer (2000 pixels abajo)
            hackyTargetPosition = new Vector3(hackyStartPosition.x, hackyStartPosition.y - 2000f, hackyStartPosition.z);
        }

        // Configurar botones (deshabilitados al inicio)
        buttonsEnabled = false;
        SetButtonsInteractable(false);

        if (btnYes != null)
            btnYes.onClick.AddListener(() => OnAnswer(true));

        if (btnNo != null)
            btnNo.onClick.AddListener(() => OnAnswer(false));

        if (backButton != null)
            backButton.onClick.AddListener(OnBackButtonClicked);

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueFromRelax);

        // Ocultar relax panel
        if (relaxPanel != null)
            relaxPanel.SetActive(false);

        // Mostrar pregunta inicial
        ShowInitialQuestion();

        // Iniciar animación de Hacky
        StartCoroutine(AnimateHackyDown());
    }

    void ShowInitialQuestion()
    {
        // Mostrar folder
        caseImage.sprite = folderSprite;

        // Pregunta inicial
        questionText.text = "¿Estás listo?";

        currentCaseIndex = -1;
    }

    IEnumerator AnimateHackyDown()
    {
        if (hackyObject == null) yield break;

        isAnimating = true;
        float elapsed = 0f;

        // Pequeña pausa antes de empezar
        yield return new WaitForSeconds(0.5f);

        // Animar Hacky bajando
        while (elapsed < hackyAnimationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / hackyAnimationDuration;

            // Usar una curva ease-in para que acelere al bajar
            float easeT = t * t;

            hackyObject.transform.localPosition = Vector3.Lerp(hackyStartPosition, hackyTargetPosition, easeT);
            yield return null;
        }

        // Asegurar posición final
        hackyObject.transform.localPosition = hackyTargetPosition;

        // Opcional: Ocultar completamente
        hackyObject.SetActive(false);

        isAnimating = false;
        buttonsEnabled = true;
        SetButtonsInteractable(true);

        Debug.Log("Animación de Hacky completada - Botones habilitados");
    }

    void SetButtonsInteractable(bool interactable)
    {
        if (btnYes != null)
            btnYes.interactable = interactable;
        if (btnNo != null)
            btnNo.interactable = interactable;
    }

    void OnAnswer(bool userAnsweredYes)
    {
        if (isAnimating || !buttonsEnabled) return;

        // Pregunta inicial: ¿Estás listo?
        if (currentCaseIndex == -1)
        {
            if (userAnsweredYes)
            {
                // Usuario está listo, pasar al primer caso
                Debug.Log("Usuario listo, iniciando casos");
                StartCoroutine(SwipeToCaseAnimated(0));
            }
            else
            {
                // Usuario no está listo, mostrar panel "Tranqui"
                Debug.Log("Usuario no está listo");
                ShowRelaxPanel();
            }
            return;
        }

        // Casos normales
        CaseData currentCase = cases[currentCaseIndex];
        bool isCorrect = userAnsweredYes == currentCase.correctAnswerIsYes;

        if (isCorrect)
        {
            Debug.Log($"Respuesta correcta en caso {currentCaseIndex + 1}");

            // Pasar al siguiente caso
            int nextCaseIndex = currentCaseIndex + 1;

            if (nextCaseIndex < cases.Count)
            {
                StartCoroutine(SwipeToCaseAnimated(nextCaseIndex));
            }
            else
            {
                // Completó todos los casos
                OnAllCasesCompleted();
            }
        }
        else
        {
            Debug.Log($"Respuesta incorrecta en caso {currentCaseIndex + 1}");

            // Mostrar feedback
            ShowIncorrectFeedback(currentCase.advice);
        }
    }

    IEnumerator SwipeToCaseAnimated(int caseIndex)
    {
        isAnimating = true;
        buttonsEnabled = false;

        // Animar salida (swipe hacia la izquierda)
        float elapsed = 0f;
        Vector3 startPos = caseImage.transform.localPosition;
        Vector3 targetPos = startPos + new Vector3(-swipeDistance, 0, 0);

        while (elapsed < swipeAnimationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / swipeAnimationDuration;
            caseImage.transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // Cambiar imagen y texto
        ShowCase(caseIndex);

        // Posicionar desde la derecha
        caseImage.transform.localPosition = startPos + new Vector3(swipeDistance, 0, 0);

        // Animar entrada (swipe hacia el centro)
        elapsed = 0f;
        Vector3 entryStart = caseImage.transform.localPosition;

        while (elapsed < swipeAnimationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / swipeAnimationDuration;
            caseImage.transform.localPosition = Vector3.Lerp(entryStart, startPos, t);
            yield return null;
        }

        caseImage.transform.localPosition = startPos;

        isAnimating = false;
        buttonsEnabled = true;
    }

    void ShowCase(int caseIndex)
    {
        if (caseIndex < 0 || caseIndex >= cases.Count) return;

        currentCaseIndex = caseIndex;
        CaseData caseData = cases[caseIndex];

        // Cambiar imagen y texto
        caseImage.sprite = caseData.caseImage;
        questionText.text = caseData.questionText;

        Debug.Log($"Mostrando caso {caseIndex + 1}: {caseData.questionText}");
    }

    void ShowRelaxPanel()
    {
        if (relaxPanel != null)
        {
            relaxPanel.SetActive(true);
            buttonsEnabled = false;
        }
    }

    void OnContinueFromRelax()
    {
        if (relaxPanel != null)
        {
            relaxPanel.SetActive(false);
            buttonsEnabled = true;
        }
    }

    void ShowIncorrectFeedback(string advice)
    {
        buttonsEnabled = false;

        if (FeedbackManager.Instance != null)
        {
            FeedbackManager.Instance.ShowFeedback(advice, OnRetry);
        }
    }

    void OnRetry()
    {
        Debug.Log("Reintentar dinámica");
        ResetDinamica();
    }

    void ResetDinamica()
    {
        buttonsEnabled = false;
        SetButtonsInteractable(false);

        // Volver a la pregunta inicial
        ShowInitialQuestion();

        // Resetear posición de imagen de caso
        caseImage.transform.localPosition = Vector3.zero;

        // Resetear Hacky y reiniciar animación
        if (hackyObject != null)
        {
            hackyObject.SetActive(true);
            hackyObject.transform.localPosition = hackyStartPosition;
            StartCoroutine(AnimateHackyDown());
        }
        else
        {
            buttonsEnabled = true;
            SetButtonsInteractable(true);
        }
    }

    void OnAllCasesCompleted()
    {
        Debug.Log("¡Todos los casos completados!");
        buttonsEnabled = false;
        Debug.Log("Mostrar pantalla de felicitación");
       // SceneManager.LoadScene("GoodFeedBack");
    }

    void OnBackButtonClicked()
    {
        Debug.Log("Volver al menú de lecciones");
        // TODO: Mostrar diálogo de confirmación
    }

    void OnDestroy()
    {
        if (btnYes != null)
            btnYes.onClick.RemoveAllListeners();
        if (btnNo != null)
            btnNo.onClick.RemoveAllListeners();
        if (backButton != null)
            backButton.onClick.RemoveListener(OnBackButtonClicked);
        if (continueButton != null)
            continueButton.onClick.RemoveListener(OnContinueFromRelax);
    }
}