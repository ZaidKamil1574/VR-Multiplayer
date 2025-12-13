using UnityEngine;

public class FingerDraw : MonoBehaviour
{
    public RenderTexture renderTexture;
    private Texture2D texture;
    private Rect rect;
    private bool isDrawing;

    private void Start()
    {
        rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
    }

    void Update()
    {
        if (Input.GetButton("Fire1")) // You can replace this with trigger or pinch detection
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("DrawingSurface"))
                {
                    Vector2 pixelUV = hit.textureCoord;
                    int x = (int)(pixelUV.x * renderTexture.width);
                    int y = (int)(pixelUV.y * renderTexture.height);

                    texture.SetPixel(x, y, Color.black);
                    texture.Apply();

                    Graphics.Blit(texture, renderTexture);
                }
            }
        }
    }
}
