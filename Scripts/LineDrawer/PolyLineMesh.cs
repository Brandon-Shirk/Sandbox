using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PolylineMesh : MonoBehaviour
{
    public float width = 0.2f;

    public Vector3[] testPath;
    public float testDuration = 5.0f;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Build(testPath);
        }
    }

    public void Build(Vector3[] points)
    {
        if (points == null || points.Length < 2)
            return;

        var mesh = new Mesh();
        mesh.name = "PolylineMesh";

        int count = points.Length;

        var vertices = new List<Vector3>(count * 2);
        var normals = new List<Vector3>(count * 2);
        var uvs = new List<Vector2>(count * 2);
        var indices = new List<int>((count - 1) * 6);

        for (int i = 0; i < count; i++)
        {
            Vector3 prev = i > 0 ? points[i - 1] : points[i];
            Vector3 curr = points[i];
            Vector3 next = i < count - 1 ? points[i + 1] : points[i];

            Vector3 dirA = (curr - prev).normalized;
            Vector3 dirB = (next - curr).normalized;

            if (dirA == Vector3.zero) dirA = dirB;
            if (dirB == Vector3.zero) dirB = dirA;

            Vector3 normalA = new Vector3(-dirA.y, dirA.x, 0);
            Vector3 normalB = new Vector3(-dirB.y, dirB.x, 0);

            Vector3 joinNormal = (normalA + normalB).normalized;

            // Miter length clamp
            float miter = 1f / Mathf.Max(Vector3.Dot(joinNormal, normalB), 0.5f);

            Vector3 offset = joinNormal * (width * 0.5f * miter);

            vertices.Add(curr + offset); // left
            vertices.Add(curr - offset); // right

            normals.Add(Vector3.forward);
            normals.Add(Vector3.forward);

            float v = i / (float)(count - 1);
            uvs.Add(new Vector2(0, v));
            uvs.Add(new Vector2(1, v));
        }

        for (int i = 0; i < count - 1; i++)
        {
            int baseIndex = i * 2;

            indices.Add(baseIndex + 0);
            indices.Add(baseIndex + 2);
            indices.Add(baseIndex + 1);

            indices.Add(baseIndex + 2);
            indices.Add(baseIndex + 3);
            indices.Add(baseIndex + 1);
        }

        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(indices, 0);
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }
}


//#if UNITY_EDITOR
//using UnityEditor;
//using UnityEngine;

//public static class PolylineMeshBaker
//{
//    public static void Bake(
//        Vector3[] points,
//        float width,
//        string assetPath)
//    {
//        Mesh mesh = PolylineMeshGenerator.GenerateMesh(points, width);

//        AssetDatabase.CreateAsset(mesh, assetPath);
//        AssetDatabase.SaveAssets();
//        AssetDatabase.Refresh();
//    }
//}
//#endif

//PolylineMeshBaker.Bake(
//    pathPoints,
//    0.2f,
//    "Assets/Meshes/MyPathMesh.asset"
//);
