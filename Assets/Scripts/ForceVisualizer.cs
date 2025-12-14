using UnityEngine;
using TMPro;

public class ForceVectorVisualizer : MonoBehaviour
{
    [Header("Physics Source (any script that exposes forces)")]
    public MonoBehaviour forceSource;

    [Header("Box Rigidbody (for positioning)")]
    public Rigidbody targetRigidbody;

    [Header("Arrow Prefabs (Z+ forward)")]
    public Transform pushArrow;
    public Transform frictionArrow;
    public Transform normalArrow;
    public Transform weightArrow;

    [Header("Labels (TMP)")]
    public TextMeshPro pushLabel;
    public TextMeshPro frictionLabel;
    public TextMeshPro normalLabel;
    public TextMeshPro weightLabel;

    [Header("Scaling")]
    [Tooltip("World meters per Newton")]
    public float metersPerNewton = 0.001f;

    public float arrowOffset = 0.3f;
    public float labelOffset = 0.1f;

    // cached reflection
    System.Func<Vector3> getPush, getFriction, getNormal, getWeight;

    void Awake()
    {
        if (!forceSource)
        {
            Debug.LogError("ForceVectorVisualizer: No force source assigned.");
            enabled = false;
            return;
        }

        // Reflection once, not every frame
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
        if (!targetRigidbody) return;

        Vector3 origin = targetRigidbody.worldCenterOfMass;

        Draw(pushArrow,     getPush(),     pushLabel,     "F",  Color.red,    origin);
        Draw(frictionArrow, getFriction(), frictionLabel, "f",  Color.yellow, origin);
        Draw(normalArrow,   getNormal(),   normalLabel,   "N",  Color.green,  origin);
        Draw(weightArrow,   getWeight(),   weightLabel,   "mg", Color.cyan,   origin);
    }

    void Draw(
        Transform arrow,
        Vector3 force,
        TextMeshPro label,
        string symbol,
        Color color,
        Vector3 origin
    )
    {
        if (!arrow || force.magnitude < 0.01f)
        {
            if (arrow) arrow.gameObject.SetActive(false);
            if (label) label.gameObject.SetActive(false);
            return;
        }

        arrow.gameObject.SetActive(true);
        if (label) label.gameObject.SetActive(true);

        Vector3 dir = force.normalized;
        float mag = force.magnitude;

        Vector3 arrowPos = origin + dir * arrowOffset;

        arrow.position = arrowPos;
        arrow.rotation = Quaternion.LookRotation(dir);
        arrow.localScale = new Vector3(
            arrow.localScale.x,
            arrow.localScale.y,
            mag * metersPerNewton
        );

        if (label)
        {
            label.text = $"{symbol} = {mag:F1} N";
            label.color = color;
            label.transform.position = arrowPos + Vector3.up * labelOffset;
            label.transform.rotation =
                Quaternion.LookRotation(Camera.main.transform.forward);
        }
    }
}
