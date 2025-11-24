using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dinamica1Controller : MonoBehaviour
{
    [Header("UI References")]
    public GameObject phoneObject;
    public Button correctButton;
    public Button incorrectButton;
    public Button backButton;

    [Header("Texts")]
    public TextMeshProUGUI questionTitle;
    public TextMeshProUGUI questionText;

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
        Debug.Log("✅ Dinamica1Controller - Start iniciado");

        if (phoneObject != null)
        {
            phoneTargetPosition = phoneObject.transform.localPosition;
            phoneStartPosition = new Vector3(phoneTargetPosition.x, phoneTargetPosition.y - 2000f, phoneTargetPosition.z);
            phoneObject.transform.localPosition = phoneStartPosition;
        }

        if (correctButton != null)
            correctButton.onClick.AddListener(OnCorrectAnswer);

        if (incorrectButton != null)
            incorrectButton.onClick.AddListener(OnIncorrectAnswer);

        if (backButton != null)
            backButton.onClick.AddListener(OnBackButtonClicked);

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
        SceneManager.LoadScene("GoodFeedBack");
    }

    void OnIncorrectAnswer()
    {
        if (isAnimating) return;

        if (FeedbackManager.Instance == null)
        {
            Debug.LogError("FeedbackManager.Instance es NULL!");
            return;
        }

        FeedbackManager.Instance.ShowFeedback(adviceText, OnRetry);
        SetButtonsInteractable(false);
    }

    void OnRetry()
    {
        SetButtonsInteractable(true);
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
        Debug.Log("Usuario quiere regresar a Lecciones");
        SceneManager.LoadScene("Lecciones"); // ← Regresa a la pantalla de Lecciones
    }

    void OnDestroy()
    {
        if (correctButton != null)
            correctButton.onClick.RemoveListener(OnCorrectAnswer);
        if (incorrectButton != null)
            incorrectButton.onClick.RemoveListener(OnIncorrectAnswer);
        if (backButton != null)
            backButton.onClick.RemoveListener(OnBackButtonClicked);
    }
}
