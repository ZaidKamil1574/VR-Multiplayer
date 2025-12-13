using UnityEngine;

public class PenDrawer : MonoBehaviour
{
    public TrailRenderer trail;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DrawingSurface"))
        {
            Debug.Log("Pen touching notepad");
            trail.emitting = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DrawingSurface"))
        {
            Debug.Log("Pen left notepad");
            trail.emitting = false;
        }
    }
}
