using UnityEngine;
using System.Collections;

public class FoxRunner : MonoBehaviour
{
    [Header("Fuerza de salto horizontal")]
    [SerializeField] private float sideForce = 10f;

    [Header("Límite horizontal")]
    [SerializeField] private float limiteX = 5f;

    [Header("Sprites")]
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite slideSprite;
    [SerializeField] private Sprite hurtRunSprite;
    [SerializeField] private Sprite hurtSlideSprite;

    [Header("Tambaleo visual horizontal")]
    [SerializeField] private float tambaleoAmplitud = 0.25f;
    [SerializeField] private float tambaleoFrecuencia = 4f;

    [Header("Detección de input (solo touch)")]
    [SerializeField] private float tapThreshold = 0.2f;     // Tap corto
    [SerializeField] private float holdThreshold = 0.25f;   // Mantener

    private Rigidbody2D FoxRb;
    private SpriteRenderer sr;
    private Vector3 posicionInicial; // base fija para tambaleo
    private bool enElAire = false;   // indica si está en salto lateral

    // Estados de input
    private bool isPressing = false; // si el dedo está presionando
    private bool isHolding = false;  // si se convirtió en mantener
    private float pressStartTime = 0f; // tiempo en que empezó el toque

    void Start()
    {
        // Inicializa componentes y estado inicial
        FoxRb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        posicionInicial = transform.localPosition;
        sr.sprite = idleSprite;
    }

    void Update()
    {
        // Detecta y procesa input táctil
        LeerInputTouch();

        // Estados visuales en suelo
        if (!enElAire)
        {
            if (isHolding)
            {
                // Estado deslizar: sprite + ajuste visual a la izquierda
                sr.sprite = slideSprite;
                transform.localPosition = posicionInicial + new Vector3(-0.1f, 0f, 0f);
            }
            else
            {
                // Estado idle: sprite + tambaleo cartoon
                sr.sprite = idleSprite;
                float offsetX = Mathf.Sin(Time.time * tambaleoFrecuencia) * tambaleoAmplitud;
                transform.localPosition = posicionInicial + new Vector3(offsetX, 0f, 0f);
            }
        }

        // Limitar posición horizontal
        float clampedX = Mathf.Clamp(transform.position.x, -limiteX, limiteX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    // Detecta input táctil y decide si es tap (salto lateral) o mantener (slide)
    private void LeerInputTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                isPressing = true;
                isHolding = false;
                pressStartTime = Time.time;
            }

            // Si se mantiene más allá del umbral, pasa a estado "holding"
            if (isPressing && !isHolding &&
                (t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved) &&
                (Time.time - pressStartTime) >= holdThreshold)
            {
                isHolding = true;
            }

            // Fin del toque
            if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                float duracion = Time.time - pressStartTime;

                // Tap corto = salto lateral cartoon
                if (duracion < tapThreshold && !enElAire)
                {
                    StartCoroutine(SaltoLateral());
                }

                // Reset de estados
                isPressing = false;
                isHolding = false;
            }
        }
    }

    // Cambia sprite según daño recibido
    public void RecibirDaño()
    {
        if (enElAire)
        {
            sr.sprite = hurtRunSprite;
        }
        else if (isHolding)
        {
            sr.sprite = hurtSlideSprite;
        }
        else
        {
            sr.sprite = hurtRunSprite;
        }
    }

    // Coroutine que ejecuta el salto lateral cartoon:
    // primero impulsa a la derecha, luego regresa a la izquierda,
    // y finalmente se detiene en la posición base
    private IEnumerator SaltoLateral()
    {
        enElAire = true;

        // Impulso a la derecha
        FoxRb.velocity = new Vector2(sideForce, FoxRb.velocity.y);
        yield return new WaitForSeconds(0.2f);

        // Impulso de regreso a la izquierda
        FoxRb.velocity = new Vector2(-sideForce, FoxRb.velocity.y);
        yield return new WaitForSeconds(0.2f);

        // Detener y volver a estado en suelo
        FoxRb.velocity = Vector2.zero;
        enElAire = false;
    }
}