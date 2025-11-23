using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class PetJump : MonoBehaviour
{
    [Header("Salto normal")]
    public float jumpHeight = 0.5f;               // Altura del salto normal
    public float jumpDuration = 0.3f;             // Duración del salto normal
    public AudioClip jumpSound;                   // Sonido del salto

    [Header("Sprites")]
    public Sprite hackystill;                     // ojos abiertos
    public Sprite jumpSprite;                     // ojos cerrados

    [Header("Minijuego")]
    public int jumpThreshold = 10;                // número de saltos para activar el minijuego
    public float specialJumpHeight = 10f;         // altura del salto especial
    public float specialJumpDuration = 1.5f;      // duración del salto especial
    public string miniGameSceneName = "MiniGame"; // nombre de la escena a cargar

    [Header("Referencias")]
    public FoodBarManager foodBarManager;         // Referencia de la barra de comida

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Vector3 startPos;
    private bool isJumping = false;

    // Contador de saltos consecutivos
    private int consecutiveJumps = 0;

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
            foodBarManager.ConsumirExtra(0.01f); // porcentaje que baja al saltar

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

        // Incrementar contador de saltos
        consecutiveJumps++;

        // activar minijuego si se alcanza el umbral
        if (consecutiveJumps >= jumpThreshold)
        {
            StartCoroutine(SpecialExit());
        }
    }

    System.Collections.IEnumerator SpecialExit()
    {
        // Salto exagerado fuera de pantalla
        float elapsed = 0f;
        Vector3 start = transform.localPosition;

        while (elapsed < specialJumpDuration)
        {
            float yOffset = Mathf.Lerp(0, specialJumpHeight, elapsed / specialJumpDuration);
            transform.localPosition = start + new Vector3(0, yOffset, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        //Cambio de escena al minijuego
        SceneManager.LoadScene(miniGameSceneName);
    }

    //Método público para reiniciar el contador desde otros botones
    public void ResetJumpCounter()
    {
        consecutiveJumps = 0;
    }
}