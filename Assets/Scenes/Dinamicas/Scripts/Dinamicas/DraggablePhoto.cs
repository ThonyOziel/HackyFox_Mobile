using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// Permite arrastrar una foto y detectar dónde se suelta

public class DraggablePhoto : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Photo Data")]
    public bool isSafeToShare; // true = segura, false = no segura
    public string photoDescription;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    private Transform originalParent;
    private Dinamica5Controller controller;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        // Agregar CanvasGroup si no existe (para controlar raycast)
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Guardar posición y padre original
        originalPosition = rectTransform.localPosition;
        originalParent = transform.parent;
    }

    public void SetController(Dinamica5Controller ctrl)
    {
        controller = ctrl;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"Comenzó a arrastrar: {photoDescription}");

        // Hacer semi-transparente y desactivar raycast
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // Mover al frente (último hijo del canvas)
        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Seguir el cursor/dedo
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"Soltó: {photoDescription}");

        // Restaurar opacidad y raycast
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Verificar si se soltó sobre una zona válida
        DropZone dropZone = GetDropZoneUnderPointer(eventData);

        if (dropZone != null)
        {
            // Colocar en la zona
            transform.SetParent(dropZone.transform);
            rectTransform.anchoredPosition = Vector2.zero;

            // Notificar al controlador
            if (controller != null)
            {
                controller.OnPhotoPlaced(this, dropZone);
            }
        }
        else
        {
            // No se soltó en una zona válida, regresar a posición original
            ReturnToOriginalPosition();
        }
    }

    DropZone GetDropZoneUnderPointer(PointerEventData eventData)
    {
        // Hacer raycast para encontrar zona de drop
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            DropZone zone = result.gameObject.GetComponent<DropZone>();
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
        rectTransform.localPosition = originalPosition;
    }
}