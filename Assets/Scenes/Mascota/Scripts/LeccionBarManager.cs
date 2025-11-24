using UnityEngine;
using System;

public class LeccionBarManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float currentValue = 0f;

    public Transform fillTransform;

    private const float totalDurationHours = 24f;
    private const string keyCooldownStart = "LeccionesCooldownStart";

    void Start()
    {
        if (PlayerPrefs.HasKey(keyCooldownStart))
        {
            DateTime start = DateTime.Parse(PlayerPrefs.GetString(keyCooldownStart));
            TimeSpan elapsed = DateTime.Now - start;

            float progress = Mathf.Clamp01(1f - (float)(elapsed.TotalHours / totalDurationHours));
            currentValue = progress;

            if (currentValue <= 0f)
            {
                currentValue = 0f;
                PlayerPrefs.DeleteKey(keyCooldownStart);
                PlayerPrefs.SetInt("LeccionesBloqueadas", 0);
            }
        }
        else
        {
            currentValue = 0f;
        }

        UpdateFill();
    }

    void Update()
    {
        if (PlayerPrefs.HasKey(keyCooldownStart))
        {
            DateTime start = DateTime.Parse(PlayerPrefs.GetString(keyCooldownStart));
            TimeSpan elapsed = DateTime.Now - start;

            float progress = Mathf.Clamp01(1f - (float)(elapsed.TotalHours / totalDurationHours));
            currentValue = progress;

            if (currentValue <= 0f)
            {
                currentValue = 0f;
                PlayerPrefs.DeleteKey(keyCooldownStart);
                PlayerPrefs.SetInt("LeccionesBloqueadas", 0);
            }

            UpdateFill();
        }
    }

    public void StartCooldown()
    {
        currentValue = 1f;
        PlayerPrefs.SetString(keyCooldownStart, DateTime.Now.ToString());
        PlayerPrefs.SetInt("LeccionesBloqueadas", 1);
        UpdateFill();
    }

    public bool EstaDesbloqueada()
    {
        return currentValue <= 0.01f && PlayerPrefs.GetInt("LeccionesBloqueadas", 0) == 0;
    }

    private void UpdateFill()
    {
        if (fillTransform == null) return;

        Vector3 scale = fillTransform.localScale;
        scale.x = currentValue;
        fillTransform.localScale = scale;
    }
}
