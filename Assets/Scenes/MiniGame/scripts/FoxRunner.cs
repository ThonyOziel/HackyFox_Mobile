using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FoxRunner : MonoBehaviour
{
    [Header("Fuerza de salto horizontal")]
    [SerializeField] private float sideForce = 10f; // fuerza lateral

    [Header("Limite horizontal")]
    [SerializeField] private float limiteX = 5f; // limite en X

    [Header("Sprites")]
    [SerializeField] private Sprite idleSprite; // sprite normal
    [SerializeField] private Sprite slideSprite; // sprite usado al recibir daño

    [Header("Tambaleo horizontal")]
    [SerializeField] private float tambaleoAmplitud = 0.25f; // amplitud
    [SerializeField] private float tambaleoFrecuencia = 4f; // frecuencia

    [Header("Deteccion de input touch")]
    [SerializeField] private float tapThreshold = 0.2f; // tiempo tap

    [Header("Sonidos")]
    [SerializeField] private AudioClip jumpSound; // sonido salto
    [SerializeField] private AudioClip hurtSound; // sonido daño

    [Header("Salida al recibir daño")]
    [SerializeField] private float salidaVelocidadX = 40f; // velocidad horizontal de salida
    [SerializeField] private float salidaVelocidadY = 10f; // velocidad vertical de salida
    [SerializeField] private float salidaDelay = 1f;       // tiempo antes de cargar escena

    private AudioSource audioSource; // audio
    private Rigidbody2D FoxRb; // cuerpo fisico
    private SpriteRenderer sr; // render sprite
    private BoxCollider2D boxCol; // collider
    private Vector3 posicionInicial; // posicion base

    private bool enElAire = false; // estado salto
    private bool isPressing = false; // tocando
    private float pressStartTime = 0f; // inicio toque

    private bool hasTakenDamage = false; // daño recibido

    void Start()
    {
        FoxRb = GetComponent<Rigidbody2D>(); // rigidbody
        sr = GetComponent<SpriteRenderer>(); // sprite
        boxCol = GetComponent<BoxCollider2D>(); // collider
        audioSource = GetComponent<AudioSource>(); // audio
        audioSource.playOnAwake = false;

        posicionInicial = transform.localPosition; // guarda posicion
        sr.sprite = idleSprite; // sprite inicial
    }

    void Update()
    {
        // Si ya recibió daño, no aplicar lógica normal
        if (hasTakenDamage) return;

        LeerInputTouch(); // procesa input

        if (enElAire) // salto
        {
            sr.sprite = idleSprite;
        }
        else // idle
        {
            sr.sprite = idleSprite;
            float offsetX = Mathf.Sin(Time.time * tambaleoFrecuencia) * tambaleoAmplitud;
            transform.localPosition = new Vector3(posicionInicial.x + offsetX, posicionInicial.y, posicionInicial.z);
        }

        // Aplica límite horizontal solo si no está en salida
        if (!hasTakenDamage)
        {
            float clampedX = Mathf.Clamp(transform.position.x, -limiteX, limiteX);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }
    }

    private void LeerInputTouch()
    {
        if (Input.touchCount == 0) return;

        Touch t = Input.GetTouch(0);

        if (t.phase == TouchPhase.Began) // inicio toque
        {
            isPressing = true;
            pressStartTime = Time.time;
        }

        if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) // fin toque
        {
            float duracion = Time.time - pressStartTime;

            if (duracion < tapThreshold && !enElAire) // salto corto
                StartCoroutine(SaltoLateral());

            isPressing = false;
        }
    }

    public void RecibirDato()
    {
        if (hasTakenDamage) return;
        hasTakenDamage = true;

        sr.sprite = slideSprite; // sprite al recibir daño

        if (hurtSound != null)
            audioSource.PlayOneShot(hurtSound);

        StartCoroutine(SalirDePantalla()); // inicia salida
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTakenDamage && (other.CompareTag("Ventana") || other.CompareTag("Mail"))) // colision
            RecibirDato();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!hasTakenDamage && (other.collider.CompareTag("Ventana") || other.collider.CompareTag("Mail"))) // colision
            RecibirDato();
    }

    private IEnumerator SaltoLateral()
    {
        enElAire = true; // inicia salto

        if (jumpSound != null)
            audioSource.PlayOneShot(jumpSound);

        FoxRb.velocity = new Vector2(sideForce, FoxRb.velocity.y); // derecha
        yield return new WaitForSeconds(0.7f);

        FoxRb.velocity = new Vector2(-sideForce, FoxRb.velocity.y); // izquierda
        yield return new WaitForSeconds(0.7f);

        FoxRb.velocity = Vector2.zero; // detiene
        enElAire = false; // fin salto
    }

    private IEnumerator SalirDePantalla()
    {
        // Usa valores configurables desde el Inspector
        FoxRb.velocity = new Vector2(salidaVelocidadX, salidaVelocidadY);
        yield return new WaitForSeconds(salidaDelay);
        SceneManager.LoadScene("Home");
    }
}