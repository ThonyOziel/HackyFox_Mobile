using UnityEngine;

public class LeccionBarManager : MonoBehaviour
{
    [Range(0f, 1f)]
    public float currentValue = 1f;

    public float decreaseRate = 0.05f; // unidades por segundo
    public float refillAmount = 0.3f;

    public Transform fillTransform;

    void Update()
    {
        currentValue -= decreaseRate * Time.deltaTime;
        currentValue = Mathf.Clamp01(currentValue);
        UpdateFill();
    }

    public void Refill()
    {
        currentValue = Mathf.Clamp01(currentValue + refillAmount);
        UpdateFill();
    }

    private void UpdateFill()
    {
        Vector3 scale = fillTransform.localScale;
        scale.x = currentValue;
        fillTransform.localScale = scale;
    }

    public void ConsumirExtra(float cantidad)
    {
        currentValue = Mathf.Clamp01(currentValue - cantidad);
        UpdateFill();
    }

}
