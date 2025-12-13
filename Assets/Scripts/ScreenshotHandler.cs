using System.IO;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    public void TakeScreenshot()  // ✅ MUST be public, void, and no parameters!
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Screenshots");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string fileName = $"screenshot_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
        string fullPath = Path.Combine(folderPath, fileName);

        ScreenCapture.CaptureScreenshot(fullPath);
        Debug.Log("📸 Screenshot saved to: " + fullPath);
    }
}
