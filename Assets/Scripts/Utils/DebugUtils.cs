using System;
using System.Collections.Generic;
using UnityEngine;

public static class DebugUtils
{
    public static void DrawMesh(Mesh mesh, Matrix4x4 trs, Color color, float duration = 0.0f)
    {
        if (mesh == null) return;

        var tris = mesh.triangles;
        var verts = mesh.vertices;

        for (int t = 0; t < tris.Length; t += 3)
        {
            Vector3 a = trs.MultiplyPoint(verts[tris[t]]);
            Vector3 b = trs.MultiplyPoint(verts[tris[t + 1]]);
            Vector3 c = trs.MultiplyPoint(verts[tris[t + 2]]);

            DrawTriangle(a, b, c, color, duration);
        }
    }

    public static void DrawTriangle(Vector3 a, Vector3 b, Vector3 c, Color color, float duration = 0.0f)
    {
        Debug.DrawLine(a, b, color, duration);
        Debug.DrawLine(b, c, color, duration);
        Debug.DrawLine(c, a, color, duration);
    }

    public static void DrawPolyline(IReadOnlyList<Vector3> points, bool loop, Color color, float duration = 0.0f)
    {
        int count = points.Count;
        for (int i = 1; i < count; ++i)
        {
            Debug.DrawLine(points[i - 1], points[i], color, duration);
        }

        if (loop)
        {
            Debug.DrawLine(points[^1], points[0], color, duration);
        }
    }

    public static void DrawBounds(Bounds bounds, Color color, float duration = 0.0f) => DrawCube(bounds.center, bounds.size, color, duration);

    public static void DrawCube(Vector3 origin, Vector3 dimensions, Color color, float duration = 0.0f)
    {
        if (dimensions == Vector3.zero) return;

        Vector3 cornerOne = origin - (dimensions / 2.0f);
        Vector3 cornerTwo = origin + (dimensions / 2.0f);

        Vector3 dx = new Vector3(dimensions.x, 0, 0);
        Vector3 dy = new Vector3(0, dimensions.y, 0);
        Vector3 dz = new Vector3(0, 0, dimensions.z);

        Vector3 dxy = new Vector3(dimensions.x, dimensions.y, 0);
        Vector3 dyz = new Vector3(0, dimensions.y, dimensions.z);

        Debug.DrawLine(cornerOne, cornerOne + dx, color, duration);
        Debug.DrawLine(cornerOne, cornerOne + dy, color, duration);
        Debug.DrawLine(cornerOne, cornerOne + dz, color, duration);

        Debug.DrawLine(cornerTwo, cornerTwo - dx, color, duration);
        Debug.DrawLine(cornerTwo, cornerTwo - dy, color, duration);
        Debug.DrawLine(cornerTwo, cornerTwo - dz, color, duration);

        Debug.DrawLine(cornerOne + dxy, cornerOne + dx, color, duration);
        Debug.DrawLine(cornerOne + dxy, cornerOne + dy, color, duration);

        Debug.DrawLine(cornerTwo - dxy, cornerTwo - dx, color, duration);
        Debug.DrawLine(cornerTwo - dxy, cornerTwo - dy, color, duration);

        Debug.DrawLine(cornerOne + dyz, cornerOne + dy, color, duration);
        Debug.DrawLine(cornerTwo - dyz, cornerTwo - dy, color, duration);
    }

    public static void DrawAxis(Vector3 origin, Vector3 lengths, Color color, float duration = 0.0f) => DrawAxis(origin, lengths, new Color[3] { color, color, color }, duration);
    public static void DrawAxis(Vector3 origin, Vector3 lengths, float duration = 0.0f) => DrawAxis(origin, lengths, new Color[3] { Color.red, Color.green, Color.blue }, duration);

    public static void DrawAxis(Vector3 origin, Vector3 lengths, Color[] colors, float duration = 0.0f)
    {
        if (lengths == Vector3.zero) return;

        Vector3 lx = new Vector3(lengths.x, 0, 0);
        Vector3 ly = new Vector3(0, lengths.y, 0);
        Vector3 lz = new Vector3(0, 0, lengths.z);

        Debug.DrawLine(origin - lx, origin + lx, colors[0], duration);
        Debug.DrawLine(origin - ly, origin + ly, colors[1], duration);
        Debug.DrawLine(origin - lz, origin + lz, colors[2], duration);
    }

    public static void DrawCircleXY(Vector3 origin, float radius, int segments, Color color, float duration = 0.0f)
    {
        DrawCircle(origin, radius, segments, true, false, false, color, duration);
    }

    public static void DrawCircleYZ(Vector3 origin, float radius, int segments, Color color, float duration = 0.0f)
    {
        DrawCircle(origin, radius, segments, false, true, false, color, duration);
    }

    public static void DrawCircleZX(Vector3 origin, float radius, int segments, Color color, float duration = 0.0f)
    {
        DrawCircle(origin, radius, segments, false, false, true, color, duration);
    }

    public static void DrawSphere(Vector3 origin, float radius, int segments, Color color, float duration = 0.0f)
    {
        DrawCircleXY(origin, radius, segments, color, duration);
        DrawCircleYZ(origin, radius, segments, color, duration);
        DrawCircleZX(origin, radius, segments, color, duration);
    }

    private static void DrawCircle(Vector3 origin, float radius, int segments, bool xy, bool yz, bool zx, Color color, float duration)
    {
        if (radius <= 0 || segments <= 0) return;

        float angleStep = (360.0f / segments) * Mathf.Deg2Rad;

        Vector3 lineStart = Vector3.zero;
        Vector3 lineEnd = Vector3.zero;
        for (int i = 0; i < segments; i++)
        {
            if (xy)
            {
                CircleStepXY(ref lineStart, angleStep, i);
                CircleStepXY(ref lineEnd, angleStep, i + 1);
            }
            else if (yz)
            {
                CircleStepYZ(ref lineStart, angleStep, i);
                CircleStepYZ(ref lineEnd, angleStep, i + 1);
            }
            else
            {
                CircleStepZX(ref lineStart, angleStep, i);
                CircleStepZX(ref lineEnd, angleStep, i + 1);
            }

            lineStart *= radius;
            lineEnd *= radius;

            lineStart += origin;
            lineEnd += origin;

            Debug.DrawLine(lineStart, lineEnd, color, duration);
        }
    }

    private static void CircleStepXY(ref Vector3 vec, float angleStep, int currentStep)
    {
        vec.x = Mathf.Cos(angleStep * currentStep);
        vec.y = Mathf.Sin(angleStep * currentStep);
        vec.z = 0.0f;
    }

    private static void CircleStepYZ(ref Vector3 vec, float angleStep, int currentStep)
    {
        vec.x = 0.0f;
        vec.y = Mathf.Cos(angleStep * currentStep);
        vec.z = Mathf.Sin(angleStep * currentStep);
    }

    private static void CircleStepZX(ref Vector3 vec, float angleStep, int currentStep)
    {
        vec.x = Mathf.Sin(angleStep * currentStep);
        vec.y = 0.0f;
        vec.z = Mathf.Cos(angleStep * currentStep);
    }
}