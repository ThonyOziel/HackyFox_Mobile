using UnityEngine;

public class PetEat : MonoBehaviour
{
    public Sprite ojosAbiertos;
    public Sprite ojosCerrados;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Comer(float duracion)
    {
        StopAllCoroutines();
        StartCoroutine(CerrarOjosTemporal(duracion));
    }

    private System.Collections.IEnumerator CerrarOjosTemporal(float duracion)
    {
        sr.sprite = ojosCerrados;
        yield return new WaitForSeconds(duracion);
        sr.sprite = ojosAbiertos;
    }
}
