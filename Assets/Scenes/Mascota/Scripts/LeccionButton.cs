using UnityEngine;

public class LeccionButton : MonoBehaviour
{
  
    public LeccionBarManager LeccionBarManager;

 

    void OnMouseDown()
    {

        // Rellenar la barra de comida
        if (LeccionBarManager != null)
            LeccionBarManager.Refill();

    }
}