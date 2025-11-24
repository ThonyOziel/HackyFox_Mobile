using UnityEngine;

public class AutoDestruir : MonoBehaviour
{
    [Header("Tiempo de vida en segundos")]
    public float tiempoVida = 5f; // puedes ajustar desde el Inspector

    void OnEnable()
    {
        // Cuando el objeto aparece, se programa su destrucción
        Invoke(nameof(DestruirObjeto), tiempoVida);
    }

    void DestruirObjeto()
    {
        Destroy(gameObject);
    }
}