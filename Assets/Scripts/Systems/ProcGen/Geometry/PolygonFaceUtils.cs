using UnityEngine;

public static class PolygonFaceUtils
{
    public static void Inset(PolygonFace input, PolygonFace output, float insetAmount)
    {
        var center = input.Center;
        for (int currPointIndex = 0; currPointIndex < input.Points.Count; ++currPointIndex)
        {
            int prevPointIndex = MathUtils.Mod(currPointIndex - 1, input.Points.Count);
            int nextPointIndex = MathUtils.Mod(currPointIndex + 1, input.Points.Count);

            Vector3 prevPoint = input.Points[prevPointIndex];
            Vector3 currPoint = input.Points[currPointIndex];
            Vector3 nextPoint = input.Points[nextPointIndex];

            Vector3 diffA = (currPoint - prevPoint).normalized;
            Vector3 diffB = (nextPoint - currPoint).normalized;

            Vector3 insetDir = (-diffA + diffB).normalized;

            Vector3 newPoint = currPoint + insetDir * insetAmount;
            output.Points.Add(newPoint);
        }
    }
}