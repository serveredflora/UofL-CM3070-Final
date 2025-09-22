using System;
using UnityEngine;

public static class PhysicsUtils
{
    public static int SortedRaycast(
        Ray ray,
        RaycastHit[] raycastHits,
        float maxDistance = float.PositiveInfinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction triggerOpt = QueryTriggerInteraction.UseGlobal)
    {
        int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, maxDistance, layerMask, triggerOpt);

        // Sort raycast hits by distance (lowest first)
        Array.Sort(raycastHits, (a, b) => a.distance.CompareTo(b.distance));

        return hitCount;
    }

    public static void DrawRaycastInfo(Ray ray, RaycastHit[] raycastHits, int hitCount)
    {
        for (int hitIndex = 0; hitIndex < hitCount; ++hitIndex)
        {
            RaycastHit hit = raycastHits[hitIndex];

            // ALINE usage commented out
            // Drawing.Draw.Ray(ray, hit.distance);
            // Drawing.Draw.Label2D(hit.point, $"[{hitIndex}]: {hit.collider.name}");
        }
    }
}