using UnityEngine;
using UnityEngine.EventSystems;


/// Zona de la mochila donde se sueltan los items
public class BackpackZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item soltado en la mochila");
    }
}