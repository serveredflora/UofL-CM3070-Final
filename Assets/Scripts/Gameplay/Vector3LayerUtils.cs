using System.Collections.Generic;
using UnityEngine;

public static class Vector3LayerUtils
{
    public static Vector3 Accumulate(IReadOnlyList<Vector3Layer> layers, float t)
    {
        Vector3 accumulated = Vector3.zero;
        for (int i = 0; i < layers.Count; i++)
        {
            Vector3Layer layer = layers[i];
            accumulated += layer.Evaluate(t);
        }

        return accumulated;
    }
}