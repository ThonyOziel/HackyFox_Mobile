using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// Dinámica 6: Mochila - Arrastrar items seguros

public class Dinamica6Controller : MonoBehaviour
{
    [Header("UI References")]
    public Button backButton;

    [Header("Items")]
    public List<DraggableItem> allItems = new List<DraggableItem>();

    [Header("Backpack")]
    public BackpackZone backpackZone;

    [Header("Lesson Data")]
    public int idLeccion = 1;
    public string incorrectAdvice = "¡Cuidado! Ese objeto no es seguro para guardar información personal.";
    public string successAdvice = "¡Excelente! Sabes qué objetos son seguros para proteger tu información.";

    private int totalSafeItems;
    private int correctItemsPlaced = 0;

    void Start()
    {
        Debug.Log("Inicializando Dinámica 6");

        // Contar items seguros
        totalSafeItems = 0;
        foreach (var item in allItems)
        {
            item.SetController(this);
            if (item.isSafeItem)
                totalSafeItems++;
        }

        Debug.Log($"Total de items seguros: {totalSafeItems}");

        // Configurar botones
        if (backButton != null)
            backButton.onClick.AddListener(OnBackButtonClicked);
    }

    
    /// Se llama cuando un item se suelta en la mochila
    public void OnItemDroppedOnBackpack(DraggableItem item)
    {
        Debug.Log($"Item en mochila: {item.itemName}");

        if (item.isSafeItem)
        {
            // Item correcto
            Debug.Log("Item seguro!");
            correctItemsPlaced++;

            // Hacer desaparecer el item
            item.Disappear();

            // Verificar si completó todos
            CheckCompletion();
        }
        else
        {
            // Item incorrecto
            Debug.Log("Item no seguro!");

            // Regresar a posición original
            item.ReturnToOriginalPosition();

            // Mostrar FeedbackPanel
            ShowIncorrectFeedback();
        }
    }

    void ShowIncorrectFeedback()
    {
        if (FeedbackManager.Instance != null)
        {
            FeedbackManager.Instance.ShowFeedback(incorrectAdvice, OnRetry);
        }
    }

    void CheckCompletion()
    {
        Debug.Log($"Progreso: {correctItemsPlaced}/{totalSafeItems}");

        if (correctItemsPlaced >= totalSafeItems)
        {
            Debug.Log("¡Todos los items seguros en la mochila!");
            OnAllItemsCorrect();
        }
    }

    void OnAllItemsCorrect()
    {
        // TODO: Guardar progreso en BD
        Debug.Log("Guardando progreso...");

        // TODO: Mostrar pantalla de felicitación
        SceneManager.LoadScene("GoodFeedBack");
    }

    void OnRetry()
    {
        Debug.Log("Reintentar dinámica");
        ResetDinamica();
    }

    void ResetDinamica()
    {
        correctItemsPlaced = 0;

        // Regresar todos los items y hacerlos visibles
        foreach (var item in allItems)
        {
            item.gameObject.SetActive(true);
            item.ReturnToOriginalPosition();
        }
    }

    void OnBackButtonClicked()
    {
        Debug.Log("Volver al menú de lecciones");
        // TODO: Mostrar diálogo de confirmación
    }

    void OnDestroy()
    {
        if (backButton != null)
            backButton.onClick.RemoveListener(OnBackButtonClicked);
    }
}