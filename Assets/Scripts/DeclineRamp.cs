using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeclinedRampAngleAdjuster : MonoBehaviour
{
    [Header("UI Reference")]
    public Slider angleSlider;

    [Header("Rotation Settings")]
    public float minAngle = 0f;     // Horizontal
    public float maxAngle = 45f;    // Declined

    [Header("Optional Display")]
    public TextMeshProUGUI angleText;

    private void Start()
    {
        if (angleSlider != null)
        {
            angleSlider.minValue = minAngle;
            angleSlider.maxValue = maxAngle;
            angleSlider.onValueChanged.AddListener(UpdateRampAngle);
            UpdateRampAngle(angleSlider.value);
        }
    }

    public void UpdateRampAngle(float angle)
    {
        // Rotate downward around X axis (negative rotation for decline)
        Vector3 currentEuler = transform.eulerAngles;
        transform.eulerAngles = new Vector3(-angle, currentEuler.y, currentEuler.z);

        if (angleText != null)
            angleText.text = $"{angle:F1}°";
    }
}
