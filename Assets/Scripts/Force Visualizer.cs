using UnityEngine;
using TMPro;

public class ForceVectorVisualizer_TMPText : MonoBehaviour
{
    [Header("Physics Source (must expose public Vector3 forces)")]
    public MonoBehaviour forceSource;

    [Header("Target Rigidbody (for positioning)")]
    public Rigidbody targetRigidbody;

    [Header("Arrow UI Objects (RectTransform, Z+ forward)")]
    public RectTransform pushArrow;
    public RectTransform frictionArrow;
    public RectTransform normalArrow;
    public RectTransform weightArrow;

    [Header("Text Labels (TMP Text)")]
    public TextMeshProUGUI pushText;
    public TextMeshProUGUI frictionText;
    public TextMeshProUGUI normalText;
    public TextMeshProUGUI weightText;

    [Header("Scaling")]
    [Tooltip("World meters per Newton")]
    public float metersPerNewton = 0.001f;

    public float arrowOffset = 0.3f;
    public float textOffset = 0.12f;

    // Cached accessors (reflection once)
    System.Func<Vector3> getPush, getFriction, getNormal, getWeight;

    void Awake()
    {
        if (!forceSource || !targetRigidbody)
        {
            Debug.LogError("ForceVectorVisualizer: Missing references.");
            enabled = false;
            return;
        }

        getPush     = Bind("PushForceWorld");
        getFriction = Bind("FrictionForceWorld");
        getNormal   = Bind("NormalForceWorld");
        getWeight   = Bind("WeightForceWorld");
    }

    System.Func<Vector3> Bind(string fieldName)
    {
        var field = forceSource.GetType().GetField(fieldName);
        if (field == null)
        {
            Debug.LogError($"ForceVectorVisualizer: Missing field {fieldName}");
            return () => Vector3.zero;
        }
        return () => (Vector3)field.GetValue(forceSource);
    }

    void LateUpdate()
    {
        Vector3 origin = targetRigidbody.worldCenterOfMass;

        Draw(pushArrow,     getPush(),     pushText,     "F",  Color.red,    origin);
        Draw(frictionArrow, getFriction(), frictionText, "f",  Color.yellow, origin);
        Draw(normalArrow,   getNormal(),   normalText,   "N",  Color.green,  origin);
        Draw(weightArrow,   getWeight(),   weightText,   "mg", Color.cyan,   origin);
    }

    void Draw(
        RectTransform arrow,
        Vector3 force,
        TextMeshProUGUI text,
        string symbol,
        Color color,
        Vector3 origin
    )
    {
        if (!arrow || force.magnitude < 0.01f)
        {
            if (arrow) arrow.gameObject.SetActive(false);
            if (text)  text.gameObject.SetActive(false);
            return;
        }

        arrow.gameObject.SetActive(true);
        text.gameObject.SetActive(true);

        Vector3 dir = force.normalized;
        float mag = force.magnitude;

        Vector3 worldPos = origin + dir * arrowOffset;

        arrow.position = worldPos;
        arrow.rotation = Quaternion.LookRotation(dir);

        // Stretch arrow on Y (change if your prefab uses Z)
        Vector3 scale = arrow.localScale;
        scale.y = mag * metersPerNewton;
        arrow.localScale = scale;

        text.text = $"{symbol} = {mag:F1} N";
        text.color = color;
        text.transform.position = worldPos + Vector3.up * textOffset;
        text.transform.rotation =
            Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
