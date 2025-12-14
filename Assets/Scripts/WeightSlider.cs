using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class BoxWeightController : MonoBehaviour
{
    [Header("Weight / Mass Settings")]
    public float minMass = 0.5f;
    public float maxMass = 20f;
    public float startMass = 1.5f;

    [Header("UI - Slider")]
    public Slider weightSlider;                 // normalized 0..1

    [Header("UI - Input (optional)")]
    public TMP_InputField massInputField;        // kg (optional)

    [Header("UI - TMP Text (optional)")]
    public TextMeshProUGUI massValueTMP;         // "1.5 kg"
    public TextMeshProUGUI weightValueTMP;       // "14.7 N"

    [Header("UI - Normal Text (optional)")]
    public Text massValueText;                   // legacy Text
    public Text weightValueText;                 // legacy Text

    [Header("Display")]
    public bool showWeightInNewtons = true;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = Mathf.Clamp(startMass, minMass, maxMass);

        if (weightSlider)
        {
            weightSlider.minValue = 0f;
            weightSlider.maxValue = 1f;
            weightSlider.wholeNumbers = false;

            float n = Mathf.InverseLerp(minMass, maxMass, rb.mass);
            weightSlider.SetValueWithoutNotify(n);
            weightSlider.onValueChanged.AddListener(OnSliderChanged);
        }

        UpdateUI();
    }

    void OnSliderChanged(float value01)
    {
        float newMass = Mathf.Lerp(minMass, maxMass, value01);
        SetMass(newMass);
    }

    public void ApplyManualMass()
    {
        if (massInputField && float.TryParse(massInputField.text, out float m))
        {
            SetMass(m);

            if (weightSlider)
            {
                float n = Mathf.InverseLerp(minMass, maxMass, rb.mass);
                weightSlider.SetValueWithoutNotify(n);
            }
        }
    }

    void SetMass(float m)
    {
        rb.mass = Mathf.Clamp(m, minMass, maxMass);
        UpdateUI();
    }

    void UpdateUI()
    {
        string massStr = $"{rb.mass:F2} kg";

        if (massValueTMP) massValueTMP.text = massStr;
        else if (massValueText) massValueText.text = massStr;

        if (!showWeightInNewtons) return;

        float weightN = rb.mass * Physics.gravity.magnitude;
        string weightStr = $"{weightN:F1} N";

        if (weightValueTMP) weightValueTMP.text = weightStr;
        else if (weightValueText) weightValueText.text = weightStr;
    }
}
