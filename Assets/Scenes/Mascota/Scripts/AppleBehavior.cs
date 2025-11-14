using UnityEngine;

public class AppleBehavior : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeAngle = 10f;
    public float shakeFrequency = 40f;
    public float fadeDuration = 0.5f;

    private SpriteRenderer sr;
    private AudioSource audioSource;

    public void StartShakeAndFade()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ShakeThenFade());
    }

    private System.Collections.IEnumerator ShakeThenFade()
    {
        if (audioSource != null)
            audioSource.Play();

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float angle = Mathf.Sin(elapsed * shakeFrequency) * shakeAngle;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.identity;

        float fadeElapsed = 0f;
        Color startColor = sr.color;

        while (fadeElapsed < fadeDuration)
        {
            float t = fadeElapsed / fadeDuration;
            sr.color = new Color(startColor.r, startColor.g, startColor.b, 1f - t);
            fadeElapsed += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
        Destroy(gameObject);
    }
}