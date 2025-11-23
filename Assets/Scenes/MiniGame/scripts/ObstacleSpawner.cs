using UnityEngine;

// Sistema que genera obstáculos en tiempo de juego
public class ObstacleSystem : MonoBehaviour
{
    [Header("Prefabs tipo ventana (pegados al suelo)")]
    [SerializeField] private GameObject[] ventanaPrefabs; // prefabs arrastrados desde Hierarchy

    [Header("Prefabs tipo mail (altura del zorro)")]
    [SerializeField] private GameObject[] mailPrefabs; // prefabs arrastrados desde Hierarchy

    [Header("Spawning y movimiento")]
    [SerializeField] private float spawnInterval = 1.75f; // tiempo entre spawns
    [SerializeField] private float obstacleSpeed = 8f;    // velocidad en Y+
    [SerializeField] private float ySpawnStart = -180f;   // posición inicial en Y
    [SerializeField] private float yDespawn = 50f;        // posición de destrucción

    [Header("Carriles X (pantalla rotada)")]
    [SerializeField] private float xGroundLane = -25.6f;  // X para ventanas
    [SerializeField] private float xFoxLane = -20f;       // X para mails

    [Header("Probabilidad de ventana (0–1)")]
    [SerializeField, Range(0f, 1f)] private float probVentana = 0.5f; // 0.5 = mitad ventana, mitad mail

    private float timer = 0f; // contador de tiempo

    void Start()
    {
        // Desactiva los prefabs originales en escena
        foreach (var go in ventanaPrefabs)
            if (go != null) go.SetActive(false);

        foreach (var go in mailPrefabs)
            if (go != null) go.SetActive(false);
    }

    void Update()
    {
        // Acumula tiempo y genera obstáculo si se cumple el intervalo
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            Spawn();
            timer = 0f;
        }
    }

    void Spawn()
    {
        // Decide si se genera ventana o mail
        bool esVentana = Random.value < probVentana;

        if (esVentana && ventanaPrefabs.Length > 0)
        {
            int i = Random.Range(0, ventanaPrefabs.Length);
            Vector3 pos = new Vector3(xGroundLane, ySpawnStart, 0f);

            // Instancia el prefab y lo activa
            GameObject go = Instantiate(ventanaPrefabs[i], pos, Quaternion.Euler(0f, 0f, -90f));
            go.SetActive(true); // asegura visibilidad
            go.transform.localScale = ventanaPrefabs[i].transform.localScale; // copia escala
            go.layer = ventanaPrefabs[i].layer; // copia capa

            AttachMover(go);
        }
        else if (!esVentana && mailPrefabs.Length > 0)
        {
            int i = Random.Range(0, mailPrefabs.Length);
            Vector3 pos = new Vector3(xFoxLane, ySpawnStart, 0f);

            GameObject go = Instantiate(mailPrefabs[i], pos, Quaternion.Euler(0f, 0f, -90f));
            go.SetActive(true);
            go.transform.localScale = mailPrefabs[i].transform.localScale;
            go.layer = mailPrefabs[i].layer;

            AttachMover(go);
        }
    }

    void AttachMover(GameObject go)
    {
        // Añade componente de movimiento si no lo tiene
        var mover = go.GetComponent<ObstacleMover>();
        if (mover == null) mover = go.AddComponent<ObstacleMover>();

        // Configura velocidad y punto de destrucción
        mover.speed = obstacleSpeed;
        mover.yDespawn = yDespawn;
    }
}

// Componente que mueve el obstáculo y lo destruye
public class ObstacleMover : MonoBehaviour
{
    [HideInInspector] public float speed = 8f;       // velocidad en Y+
    [HideInInspector] public float yDespawn = 50f;   // punto de destrucción

    void Update()
    {
        // Mueve hacia arriba
        transform.position += Vector3.up * speed * Time.deltaTime;

        // Destruye si sale del área útil
        if (transform.position.y >= yDespawn)
            Destroy(gameObject);
    }
}