using UnityEngine;

public class Scroll : MonoBehaviour
{
    [SerializeField] private float velocidad = 8f;       // velocidad de scroll
    [SerializeField] private Transform suelo;            // suelo vinculado

    void Update()
    {
        Vector3 desplazamiento = Vector2.up * velocidad * Time.deltaTime;

        transform.Translate(desplazamiento, Space.World);      // mueve fondo
        if (suelo != null)
            suelo.Translate(desplazamiento, Space.World);      // mueve suelo
    }
}
