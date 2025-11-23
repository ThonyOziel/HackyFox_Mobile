using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// Item arrastrable para meter en la mochila

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Item Data")]
    public bool isSafeItem; // true = seguro para meter en mochila, false = no seguro
    public string itemName;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    private Transform originalParent;
    private Dinamica6Controller controller;
    private int originalSiblingIndex;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        // Agregar CanvasGroup si no existe
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Guardar posición y padre original
        originalPosition = rectTransform.localPosition;
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
    }

    public void SetController(Dinamica6Controller ctrl)
    {
        controller = ctrl;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"Comenzó a arrastrar: {itemName}");

        // Hacer semi-transparente y desactivar raycast
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;

        // Mover al frente
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Seguir el cursor
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"Soltó: {itemName}");

        // Restaurar opacidad y raycast
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Verificar si se soltó sobre la mochila
        BackpackZone backpack = GetBackpackUnderPointer(eventData);

        if (backpack != null)
        {
            // Notificar al controlador
            if (controller != null)
            {
                controller.OnItemDroppedOnBackpack(this);
            }
        }
        else
        {
            // No se soltó en la mochila, regresar a posición original
            ReturnToOriginalPosition();
        }
    }

    BackpackZone GetBackpackUnderPointer(PointerEventData eventData)
    {
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            BackpackZone zone = result.gameObject.GetComponent<BackpackZone>();
            if (zone != null)
            {
                return zone;
            }
        }

        return null;
    }

    public void ReturnToOriginalPosition()
    {
        transform.SetParent(originalParent);
        transform.SetSiblingIndex(originalSiblingIndex);
        rectTransform.localPosition = originalPosition;
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
    }
}