using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CheckpointPath : MonoBehaviour
{
    [SerializeField] private Color pathColor = Color.yellow;
    [SerializeField] private Color completedPathColor = Color.green;
    [SerializeField] private float lineWidth = 0.2f;
    [SerializeField] private bool showPath = true;

    private LineRenderer pathRenderer;
    private List<Vector3> pathPoints = new List<Vector3>();

    private void Awake()
    {
        InitializeLineRenderer();
        //CollectPathPoints();
    }

    private void InitializeLineRenderer()
    {
        pathRenderer = gameObject.AddComponent<LineRenderer>();
        pathRenderer.startWidth = lineWidth;
        pathRenderer.endWidth = lineWidth;
        pathRenderer.material = new Material(Shader.Find("Sprites/Default"));
        pathRenderer.startColor = pathColor;
        pathRenderer.endColor = pathColor;
    }

    private void CollectPathPoints()
    {
        pathPoints.Clear();
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Checkpoint>() != null)
            {
                pathPoints.Add(child.position);
            }
        }

        pathRenderer.positionCount = pathPoints.Count;
        pathRenderer.SetPositions(pathPoints.ToArray());
    }

    public async void SetPathPoints(List<Checkpoint> points)
    {
        pathPoints.Clear();
        foreach (Checkpoint child in points)
        {
            pathPoints.Add(child.transform.position);
        }

        await Task.Delay(1000);
        pathRenderer.positionCount = pathPoints.Count;
        pathRenderer.SetPositions(pathPoints.ToArray());
    }

    public void UpdatePathVisualization(int lastReachedIndex)
    {
        if (!showPath) return;

        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

        float progress = (float)(lastReachedIndex + 1) / pathPoints.Count;

        colorKeys[0] = new GradientColorKey(completedPathColor, progress);
        colorKeys[1] = new GradientColorKey(pathColor, progress);

        alphaKeys[0] = new GradientAlphaKey(1.0f, 0.0f);
        alphaKeys[1] = new GradientAlphaKey(1.0f, 1.0f);

        gradient.SetKeys(colorKeys, alphaKeys);
        pathRenderer.colorGradient = gradient;
    }

    private void OnValidate()
    {
        if (pathRenderer != null)
        {
            pathRenderer.startWidth = lineWidth;
            pathRenderer.endWidth = lineWidth;
            pathRenderer.enabled = showPath;
        }
    }
}
