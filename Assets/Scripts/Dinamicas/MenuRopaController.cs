using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;


/// Controlador del menú de cambio de ropa de Hacky
public class MenuRopaController : MonoBehaviour
{
    [Header("UI References")]
    public Image currentHackyImage;      // Imagen de Hacky con ropa actual
    public Image currentRopaImage;       // Imagen de la ropa (burbuja)
    public Button btnBack;               // Botón ?
    public Button btnNext;               // Botón ?
    public Button btnMenu;               // Botón para abrir menú flotante

    [Header("Outfits")]
    public List<Sprite> hackyOutfits = new List<Sprite>(); // 10 sprites de Hacky con diferentes ropas
    public List<Sprite> ropaIcons = new List<Sprite>();    // 10 sprites de iconos de ropa (opcional)

    [Header("Menu Flotante")]
    public GameObject floatingMenu;      // Referencia al menú flotante

    private int currentOutfitIndex = 0;

    void Start()
    {
        Debug.Log("Inicializando Menú de Ropa");

        // Verificar que haya 10 outfits
        if (hackyOutfits.Count != 10)
        {
            Debug.LogWarning($"Se esperaban 10 outfits, pero hay {hackyOutfits.Count}");
        }

        // Configurar botones
        if (btnBack != null)
            btnBack.onClick.AddListener(OnBackClicked);

        if (btnNext != null)
            btnNext.onClick.AddListener(OnNextClicked);

        if (btnMenu != null)
            btnMenu.onClick.AddListener(OnMenuClicked);

        // Ocultar menú flotante al inicio
        if (floatingMenu != null)
            floatingMenu.SetActive(false);

        // Mostrar primer outfit
        ShowCurrentOutfit();
    }

    void OnBackClicked()
    {
        Debug.Log("Botón Back presionado");

        // Ir al outfit anterior (circular)
        currentOutfitIndex--;
        if (currentOutfitIndex < 0)
            currentOutfitIndex = hackyOutfits.Count - 1;

        ShowCurrentOutfit();
    }

    void OnNextClicked()
    {
        Debug.Log("Botón Next presionado");

        // Ir al siguiente outfit (circular)
        currentOutfitIndex++;
        if (currentOutfitIndex >= hackyOutfits.Count)
            currentOutfitIndex = 0;

        ShowCurrentOutfit();
    }

    void ShowCurrentOutfit()
    {
        if (currentOutfitIndex < 0 || currentOutfitIndex >= hackyOutfits.Count)
            return;

        // Cambiar imagen de Hacky
        if (currentHackyImage != null)
        {
            currentHackyImage.sprite = hackyOutfits[currentOutfitIndex];
            Debug.Log($"Mostrando outfit {currentOutfitIndex + 1}/{hackyOutfits.Count}");
        }

        // Cambiar icono de ropa (si existe)
        if (currentRopaImage != null && ropaIcons.Count > currentOutfitIndex)
        {
            currentRopaImage.sprite = ropaIcons[currentOutfitIndex];
        }

        // TODO: Guardar la selección en PlayerPrefs o BD
        // PlayerPrefs.SetInt("HackyOutfit", currentOutfitIndex);
    }

    void OnMenuClicked()
    {
        Debug.Log("Botón Menu presionado");

        // Toggle del menú flotante
        if (floatingMenu != null)
        {
            bool isActive = floatingMenu.activeSelf;
            floatingMenu.SetActive(!isActive);
        }
        else if (FloatingMenuManager.Instance != null)
        {
            // Si usamos el manager global
            FloatingMenuManager.Instance.ToggleMenu();
        }
    }

    void OnDestroy()
    {
        if (btnBack != null)
            btnBack.onClick.RemoveListener(OnBackClicked);
        if (btnNext != null)
            btnNext.onClick.RemoveListener(OnNextClicked);
        if (btnMenu != null)
            btnMenu.onClick.RemoveListener(OnMenuClicked);
    }
}