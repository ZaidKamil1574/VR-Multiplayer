using UnityEngine;

public class HandMenuWhenPalmVisible : MonoBehaviour
{
    [Header("References")]
    public GameObject handMenu;
    public Transform palmTransform;

    [Header("Palm Detection Settings")]
    [Range(0f, 90f)]
    public float showAngleThreshold = 20f;     // Must be tightly facing camera
    [Range(0f, 90f)]
    public float hideAngleThreshold = 30f;     // Adds buffer to avoid flicker
    public float activationDelay = 0.2f;       // Time delay before showing menu

    private float visibleTimer = 0f;
    private bool isMenuVisible = false;

    private void Start()
    {
        if (handMenu != null)
            handMenu.SetActive(false);
    }

    private void Update()
    {
        if (handMenu == null || palmTransform == null) return;

        float angle = Vector3.Angle(palmTransform.up, -Camera.main.transform.forward);
        Debug.Log("Palm Facing Camera Angle: " + angle);

        if (angle < showAngleThreshold)
        {
            visibleTimer += Time.deltaTime;
            if (!isMenuVisible && visibleTimer >= activationDelay)
            {
                ShowMenu();
            }
        }
        else if (angle > hideAngleThreshold)
        {
            visibleTimer = 0f;
            if (isMenuVisible)
            {
                HideMenu();
            }
        }
        else
        {
            visibleTimer = 0f; // Reset timer if in-between
        }
    }

    private void ShowMenu()
    {
        handMenu.SetActive(true);
        isMenuVisible = true;
    }

    private void HideMenu()
    {
        handMenu.SetActive(false);
        isMenuVisible = false;
    }
}
