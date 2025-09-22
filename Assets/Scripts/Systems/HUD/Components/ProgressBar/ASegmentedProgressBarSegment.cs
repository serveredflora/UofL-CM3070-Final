using TriInspector;
using UnityEngine;

public abstract class ASegmentedProgressBarSegment : MonoBehaviour
{
    public bool Active { get => _Active; protected set => _Active = value; }
    private bool _Active;

    public abstract void SetActive(bool value);
}
