using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple helper that loads a different Unity scene when its public
/// <c>LoadScene</c> method is invoked (e.g. by a UI Button).
/// </summary>
public class SceneChangeButton : MonoBehaviour
{
    [Tooltip("Name *or* build-index of the scene to load")]
    public string sceneToLoad = "MyOtherScene";

    /// <summary>Called from a UI Button’s OnClick(), or anywhere in code.</summary>
    public void LoadScene()
    {
        if (string.IsNullOrWhiteSpace(sceneToLoad))
        {
            Debug.LogError("[SceneChangeButton] Scene name / index is empty.", this);
            return;
        }

        // Allow either a numeric build-index or a human-readable name
        if (int.TryParse(sceneToLoad, out int buildIndex))
            SceneManager.LoadScene(buildIndex);
        else
            SceneManager.LoadScene(sceneToLoad);
    }
}
