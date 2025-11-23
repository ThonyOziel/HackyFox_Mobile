using UnityEngine;
using UnityEngine.EventSystems;


/// Zona donde se pueden soltar las fotos
public class DropZone : MonoBehaviour, IDropHandler
{
    [Header("Zone Type")]
    public bool isSafeZone; // true = zona segura, false = zona no segura

    public Transform photosContainer; // Contenedor donde se colocan las fotos

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"Foto soltada en zona: {(isSafeZone ? "Segura" : "No Segura")}");
    }

    public Transform GetPhotosContainer()
    {
        return photosContainer != null ? photosContainer : transform;
    }
}