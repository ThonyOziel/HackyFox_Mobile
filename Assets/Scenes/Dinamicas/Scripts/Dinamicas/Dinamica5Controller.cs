using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;


/// Dinámica 5: Drag & Drop - Identificar fotos seguras

public class Dinamica5Controller : MonoBehaviour
{
    [Header("UI References")]
    public Button backButton;
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;
    public Button continueButton;

    [Header("Photos")]
    public List<DraggablePhoto> allPhotos = new List<DraggablePhoto>();

    [Header("Drop Zones")]
    public DropZone safeZone;
    public DropZone unsafeZone;

    [Header("Lesson Data")]
    public int idLeccion = 1;
    public string successAdvice = "¡Excelente! Sabes identificar qué fotos son seguras para compartir.";
    public string failureAdvice = "Recuerda: No compartas fotos que muestren información personal como uniformes escolares, direcciones o lugares específicos.";

    private int totalPhotos;
    private int correctlyPlacedPhotos = 0;
    private bool allPhotosPlaced = false;

    void Start()
    {
        Debug.Log("Inicializando Dinámica 5");

        totalPhotos = allPhotos.Count;

        // Configurar cada foto
        foreach (var photo in allPhotos)
        {
            photo.SetController(this);
        }

        // Configurar botones
        if (backButton != null)
            backButton.onClick.AddListener(OnBackButtonClicked);

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueFromWarning);

        // Ocultar panel de advertencia
        if (warningPanel != null)
            warningPanel.SetActive(false);

        Debug.Log($"Total de fotos: {totalPhotos}");
    }

    /// <summary>
    /// Se llama cuando una foto se coloca en una zona
    /// </summary>
    public void OnPhotoPlaced(DraggablePhoto photo, DropZone zone)
    {
        Debug.Log($"Foto colocada: {photo.photoDescription} en zona {(zone.isSafeZone ? "Segura" : "No Segura")}");

        // Verificar si la colocación es correcta
        bool isCorrect = photo.isSafeToShare == zone.isSafeZone;

        if (isCorrect)
        {
            Debug.Log("Colocación correcta!");
            correctlyPlacedPhotos++;

            // Ocultar la foto (desaparecerla)
            photo.gameObject.SetActive(false);

            // Verificar si completó todas
            CheckCompletion();
        }
        else
        {
            Debug.Log("Colocación incorrecta!");

            // Mostrar advertencia
            ShowWarning(photo);
        }
    }

    void ShowWarning(DraggablePhoto photo)
    {
        if (warningPanel != null)
        {
            warningPanel.SetActive(true);

            if (warningText != null)
            {
                // Mensaje personalizado según el tipo de error
                if (photo.isSafeToShare && !photo.transform.IsChildOf(safeZone.transform))
                {
                    warningText.text = "Esta foto SÍ es segura para compartir. No revela información personal.";
                }
                else
                {
                    warningText.text = "¡Cuidado! Esta imagen podría revelar información personal.";
                }
            }

            // Regresar la foto a su posición original
            photo.ReturnToOriginalPosition();
        }
    }

    void OnContinueFromWarning()
    {
        if (warningPanel != null)
            warningPanel.SetActive(false);
    }

    void CheckCompletion()
    {
        Debug.Log($"Progreso: {correctlyPlacedPhotos}/{totalPhotos}");

        if (correctlyPlacedPhotos >= totalPhotos)
        {
            Debug.Log("¡Todas las fotos colocadas correctamente!");
            OnAllPhotosCorrect();
        }
    }

    void OnAllPhotosCorrect()
    {
        allPhotosPlaced = true;

        // TODO: Guardar progreso en BD
        Debug.Log("Guardando progreso...");

        // Mostrar feedback de éxito
        if (FeedbackManager.Instance != null)
        {
            // Modificar el FeedbackManager para mostrar mensaje de éxito también
            // O crear una pantalla de felicitación
            Debug.Log("Mostrar pantalla de felicitación");
        }
    }

    void OnBackButtonClicked()
    {
        Debug.Log("Volver al menú de lecciones");
        // TODO: Mostrar diálogo de confirmación
    }

    public void ResetDinamica()
    {
        correctlyPlacedPhotos = 0;
        allPhotosPlaced = false;

        // Regresar todas las fotos a su posición original y hacerlas visibles
        foreach (var photo in allPhotos)
        {
            photo.gameObject.SetActive(true);
            photo.ReturnToOriginalPosition();
        }
    }

    void OnDestroy()
    {
        if (backButton != null)
            backButton.onClick.RemoveListener(OnBackButtonClicked);
        if (continueButton != null)
            continueButton.onClick.RemoveListener(OnContinueFromWarning);
    }
}