using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable), typeof(Rigidbody))]
public class HeldLayerSwap : MonoBehaviour
{
    public string heldLayerName = "HeldItem";
    public bool makeKinematicWhileHeld = true;

    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    Rigidbody rb;
    int originalLayer;

    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb   = GetComponent<Rigidbody>();
        originalLayer = gameObject.layer;
    }

    void OnEnable()
    {
        // UnityEvent-style in newer XRI
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }
    void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs _)
    {
        originalLayer = gameObject.layer;

        int heldLayer = LayerMask.NameToLayer(heldLayerName);
        if (heldLayer >= 0) SetLayerRecursively(transform, heldLayer);

        if (makeKinematicWhileHeld && rb) rb.isKinematic = true; // rock-solid while held
    }

    void OnRelease(SelectExitEventArgs _)
    {
        SetLayerRecursively(transform, originalLayer);
        if (makeKinematicWhileHeld && rb) rb.isKinematic = false; // restore physics
    }

    static void SetLayerRecursively(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; i++)
            SetLayerRecursively(t.GetChild(i), layer);
    }
}
