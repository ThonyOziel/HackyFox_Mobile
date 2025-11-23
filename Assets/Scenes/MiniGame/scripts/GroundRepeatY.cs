using UnityEngine;

public class GroundRepeatY : MonoBehaviour
{
    private static float spriteHeight;
    [SerializeField] private float velocidad = 8f;

    void Start()
    {
        if (spriteHeight == 0f)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            spriteHeight = sr.bounds.size.y;
        }

        Vector3 posicionClon = new Vector3(-22.3f, -156.3f, 0f);
        GameObject clon = Instantiate(gameObject, posicionClon, transform.rotation);

        Destroy(clon.GetComponent<GroundRepeatY>());
        clon.AddComponent<GroundRepeatYClone>().Init(spriteHeight, velocidad);
    }

    void Update()
    {
        transform.position += transform.up * velocidad * Time.deltaTime;

        if (Vector3.Distance(transform.position, Vector3.zero) >= spriteHeight)
        {
            transform.position -= transform.up * 2f * spriteHeight;
        }
    }
}

public class GroundRepeatYClone : MonoBehaviour
{
    private float spriteHeight;
    private float velocidad;
    private Vector3 posicionInicial;

    public void Init(float height, float speed)
    {
        spriteHeight = height;
        velocidad = speed;
        posicionInicial = transform.position;
    }

    void Update()
    {
        transform.position += transform.up * velocidad * Time.deltaTime;

        if (Vector3.Distance(transform.position, posicionInicial) >= spriteHeight)
        {
            transform.position -= transform.up * 2f * spriteHeight;
        }
    }
}