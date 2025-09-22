using System;
using System.Diagnostics.Contracts;
using UnityEngine;

[Serializable]
public struct Vector3Layer
{
    public Vector3 Amplitude;
    public Vector3 Frequency;
    public Vector3 Phase;

    [Pure]
    public Vector3 Evaluate(float t)
    {
        return new Vector3(
            Mathf.Sin((t + Phase.x) * Frequency.x) * Amplitude.x,
            Mathf.Sin((t + Phase.y) * Frequency.y) * Amplitude.y,
            Mathf.Sin((t + Phase.z) * Frequency.z) * Amplitude.z
        );
    }
}
