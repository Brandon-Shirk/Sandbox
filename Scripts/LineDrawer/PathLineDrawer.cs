using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(LineRenderer))]
public class PathLineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    public Vector3[] testPath;
    public float testDuration = 5.0f;


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawPath(testPath, testDuration);
        }
    }


    private void Reset()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawPath(Vector3[] points, float duration)
    {
        if (points == null || points.Length < 2)
            return;

        DOTween.Kill(this);

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, points[0]);

        float totalLength = GetTotalLength(points);

        Sequence sequence = DOTween.Sequence().SetTarget(this);

        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector3 start = points[i];
            Vector3 end = points[i + 1];

            float segmentLength = Vector3.Distance(start, end);
            float segmentDuration = duration * (segmentLength / totalLength);

            int index = i + 1;

            sequence.AppendCallback(() =>
            {
                lineRenderer.positionCount = index + 1;
                lineRenderer.SetPosition(index, start);
            });

            float t = 0f;

            sequence.Append(
                DOTween.To(
                    () => t,
                    value =>
                    {
                        t = value;
                        lineRenderer.SetPosition(index, Vector3.LerpUnclamped(start, end, t));
                    },
                    1f,
                    segmentDuration
                ).SetEase(Ease.Linear)
            );
        }
    }

    private float GetTotalLength(Vector3[] points)
    {
        float length = 0f;
        for (int i = 0; i < points.Length - 1; i++)
        {
            length += Vector3.Distance(points[i], points[i + 1]);
        }
        return length;
    }
}
