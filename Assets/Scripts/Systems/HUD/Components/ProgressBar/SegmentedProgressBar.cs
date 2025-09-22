using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;

public class SegmentedProgressBar : MonoBehaviour, IProgressBar<int>
{
    public int Value
    {
        get => _Value;
        set
        {
            value = Math.Clamp(value, 0, _Range);

            if (_Value == value)
            {
                return;
            }

            int oldValue = _Value;
            _Value = value;
            UpdateValue(oldValue, _Value);
        }
    }

    public int Range
    {
        get => _Range;
        set
        {
            value = Math.Max(value, 1);

            if (_Range == value)
            {
                return;
            }

            Value = Math.Min(Value, value);

            int oldRange = _Range;
            _Range = value;
            UpdateRange(oldRange, _Range);
        }
    }

    private int _Value;
    private int _Range;

    [Header("Settings")]
    [SerializeField]
    [OnValueChanged(nameof(InspectorValueChange))]
    private int InspectorValue;
    [SerializeField]
    [OnValueChanged(nameof(InspectorRangeChange))]
    private int InspectorRange;

    [Header("References")]
    [SerializeField]
    private ASegmentedProgressBarSegment SegmentPrefab;

    private List<ASegmentedProgressBarSegment> EnabledSegments = new();
    private List<ASegmentedProgressBarSegment> DisabledSegments = new();

    private void Start()
    {
        Range = InspectorRange;
        Value = InspectorValue;
    }

    private void UpdateValue(int oldValue, int value)
    {
        var diff = value - oldValue;
        if (diff == 0)
        {
            return;
        }

        if (diff > 0)
        {
            // Increase
            for (int i = 0; i < diff; ++i)
            {
                EnabledSegments[oldValue + i].SetActive(true);
            }
        }
        else
        {
            // Decrease
            for (int i = 0; i > diff; --i)
            {
                // Offset by one as (value: 1) => (index: 0)
                EnabledSegments[oldValue + i - 1].SetActive(false);
            }
        }
    }

    private void UpdateRange(int oldRange, int range)
    {
        var diff = range - oldRange;
        if (diff == 0)
        {
            return;
        }

        if (diff > 0)
        {
            // Increase
            for (int i = 0; i < diff; ++i)
            {
                bool shouldBeActive = oldRange + i <= Value;

                ASegmentedProgressBarSegment segment;
                if (DisabledSegments.Count > 0)
                {
                    // Use disabled segments first
                    segment = DisabledSegments[^1];
                    DisabledSegments.RemoveAt(DisabledSegments.Count - 1);
                    segment.gameObject.SetActive(true);
                }
                else
                {
                    segment = Instantiate(SegmentPrefab, transform);
                }

                segment.SetActive(shouldBeActive);
                EnabledSegments.Add(segment);
            }
        }
        else
        {
            // Decrease
            for (int i = 0; i > diff; --i)
            {
                ASegmentedProgressBarSegment segment;
                if (EnabledSegments.Count > 0)
                {
                    // Use disabled segments first
                    segment = EnabledSegments[^1];
                    EnabledSegments.RemoveAt(EnabledSegments.Count - 1);
                    segment.gameObject.SetActive(false);
                }
                else
                {
                    throw new InvalidOperationException();
                }

                DisabledSegments.Add(segment);
            }
        }
    }

    private void InspectorValueChange()
    {
        if (Application.isPlaying)
        {
            Value = InspectorValue;
            InspectorValue = Value;
        }
    }

    private void InspectorRangeChange()
    {
        if (Application.isPlaying)
        {
            Range = InspectorRange;
            InspectorValue = Value;
            InspectorRange = Range;
        }
    }

}
