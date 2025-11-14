using UnityEngine;

public class PetJump : MonoBehaviour
{
    public float jumpHeight = 0.5f;
    public float jumpDuration = 0.3f;
    public AudioClip jumpSound;
    private AudioSource audioSource;

    public Sprite hackystill;   // ojos abiertos
    public Sprite jumpSprite;   // ojos cerrados

    private SpriteRenderer spriteRenderer;
    private Vector3 startPos;
    private bool isJumping = false;

    public FoodBarManager foodBarManager; // Referencia de la barra de comida

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.localPosition;
        spriteRenderer.sprite = hackystill;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(clickPos);

            if (hit != null && hit.transform == transform && !isJumping)
            {
                StartCoroutine(Jump());
            }
        }
    }

    System.Collections.IEnumerator Jump()
    {
        isJumping = true;
        spriteRenderer.sprite = jumpSprite;

        // Consumir comida al saltar
        if (foodBarManager != null)
            foodBarManager.ConsumirExtra(0.01f); // porcentaje que baja al saltar 0.2f == 20%

        float elapsed = 0f;
        if (jumpSound != null)
            audioSource.PlayOneShot(jumpSound);
        while (elapsed < jumpDuration)
        {
            float t = elapsed / jumpDuration;
            float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            transform.localPosition = startPos + new Vector3(0, yOffset, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = startPos;
        spriteRenderer.sprite = hackystill;
        isJumping = false;
    }
}