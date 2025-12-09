using UnityEngine;

public class Arrow
{
    private LineRenderer lineRenderer;
    private GameObject obj;

    public static Color startColor = Color.green;
    public static Color endColor = Color.red;
    public Arrow()
    {
        obj = new GameObject();
        lineRenderer = obj.AddComponent<LineRenderer>();
        lineRenderer.sortingOrder = 1000;
        // Set the material
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color
        lineRenderer.endColor = endColor;
        lineRenderer.startColor = startColor;

        // Set the width
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.2f;

        // Set the number of vertices
        lineRenderer.positionCount = 2;

        // Set the positions of the vertices
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));

        lineRenderer.SetPosition(1, new Vector3(1, 1, 0));
    }
    public void Show(Vector2 launchVector, Vector2 position2d, float MaxLaunchLength,float LaunchRampingFactor )
    {
        lineRenderer.endColor = Color.Lerp(startColor, endColor, launchVector.magnitude / MaxLaunchLength);

        lineRenderer.endWidth = Mathf.Lerp(.2f, .5f, launchVector.magnitude / MaxLaunchLength);


        lineRenderer.SetPosition(0, position2d);
        lineRenderer.SetPosition(1, position2d + launchVector * LaunchRampingFactor);
    }

    public void Hide()
    {
        lineRenderer.SetPosition(0, Vector3.one * 10000000000);
        lineRenderer.SetPosition(1, Vector3.one * 10000000000);
    }
}
    