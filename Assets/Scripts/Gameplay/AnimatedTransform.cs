using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedTransform : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private List<Vector3Layer> PositionLayers = new();
    [SerializeField]
    private List<Vector3Layer> RotationLayers = new();

    [Header("References")]
    [SerializeField]
    private Transform TargetTransform;

    private float TimeOffset;

    private void Start()
    {
        TimeOffset = UnityEngine.Random.Range(0.0f, 1000.0f);
    }

    private void Update()
    {
        float t = Time.time + TimeOffset;
        UpdatePosition(t);
        UpdateRotation(t);
    }

    private void UpdatePosition(float t)
    {
        var position = Vector3LayerUtils.Accumulate(PositionLayers, t);
        TargetTransform.localPosition = position;
    }

    private void UpdateRotation(float t)
    {
        var euler = Vector3LayerUtils.Accumulate(RotationLayers, t);
        TargetTransform.localRotation = Quaternion.Euler(euler);
    }
}
