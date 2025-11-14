using UnityEngine;

public class FoodButton : MonoBehaviour
{
    [Header("Manzana")]
    public Sprite appleSprite;
    public AudioClip appleSound;
    public FoodBarManager foodBarManager;

    [Header("Mascota")]
    public PetEat mascotaEat;

    void OnMouseDown()
    {
        // La mascota cierra los ojos
        mascotaEat.Comer(0.5f);

        // Rellenar la barra de comida
        if (foodBarManager != null)
            foodBarManager.Refill();

        //Crear manzana temporal
        GameObject apple = new GameObject("Apple");

        apple.transform.position = new Vector3(-1.37f, 0.36f, 0f);
        apple.transform.rotation = Quaternion.Euler(0f, 0f, 16.424f);
        apple.transform.localScale = new Vector3(0.18762f, 0.18762f, 0.18762f);

        // Sprite visual
        SpriteRenderer sr = apple.AddComponent<SpriteRenderer>();
        sr.sprite = appleSprite;
        sr.color = Color.white;
        sr.sortingOrder = 1;

        // Sonido cartoon
        AudioSource audio = apple.AddComponent<AudioSource>();
        audio.clip = appleSound;
        audio.playOnAwake = false;

        // Comportamiento de agitación + desvanecimiento
        AppleBehavior behavior = apple.AddComponent<AppleBehavior>();
        behavior.StartShakeAndFade();
    }
}