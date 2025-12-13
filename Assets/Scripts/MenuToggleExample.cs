using UnityEngine;
using UnityEngine.InputSystem;

public class MenuToggleExample : MonoBehaviour
{
    public GameObject menuPanel; // Drag your menu panel here in Inspector
    private InputAction toggleAction;

    void Awake()
    {
        // Create input action in code
        toggleAction = new InputAction(
            name: "toggleMenu",
            type: InputActionType.Button,
            binding: "<XRController>{RightHand}/primaryButton"
        );

        toggleAction.performed += ctx => ToggleMenu();
        toggleAction.Enable();
    }

    private void ToggleMenu()
    {
        if (menuPanel != null)
        {
            bool isActive = menuPanel.activeSelf;
            menuPanel.SetActive(!isActive);
        }
    }

    private void OnDisable()
    {
        toggleAction.performed -= ctx => ToggleMenu();
        toggleAction.Disable();
    }
}
