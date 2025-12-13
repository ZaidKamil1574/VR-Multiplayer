using UnityEngine;
using UnityEngine.UI;

public class HeightAdjuster : MonoBehaviour
{
    [Header("UI")]
    public Scrollbar heightScrollbar;     // 👈 Assign in Inspector
    [Header("Player Transform")]
    public Transform playerBody;          // 👈 This should be your XR Rig or Player Root
    public float minY = 0.8f;             // 👈 Minimum height
    public float maxY = 2.2f;             // 👈 Maximum height

    void Start()
    {
        if (heightScrollbar != null)
            heightScrollbar.onValueChanged.AddListener(AdjustHeight);
    }

    void AdjustHeight(float value)
    {
        float newY = Mathf.Lerp(minY, maxY, value); // Map 0-1 to height range
        Vector3 newPos = playerBody.position;
        newPos.y = newY;
        playerBody.position = newPos;
    }
}