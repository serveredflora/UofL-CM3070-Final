using System;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using TriInspector;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("Settings")]
    public ElevatorFloorConfig[] FloorConfigs;
    public float TravelSpeed;

    [Header("References")]
    public Rigidbody ChamberBody;
    public ColliderTriggerEvents ChamberTrigger;
    public Transform ChamberControlsParent;
    public ElevatorChamberControl ChamberControlPrefab;

    public ElevatorFloorConfig CurrentFloorConfig => CurrentFloorIndex != -1 ? FloorConfigs[CurrentFloorIndex] : null;
    public int CurrentFloorIndex { get; private set; } = -1;

    private ElevatorChamberControl[] ChamberControlInstances;

    private List<Rigidbody> ChamberExcludeBodies = new();
    private List<Rigidbody> CurrentChamberBodies = new();
    private List<IPhysicsTeleportable> CurrentChamberPhysicsTeleportables = new();

    private float CurrentHeight;
    private Tween? CurrentHeightTween;

    private void Start()
    {
        CreateCamberControls();
        SwitchToFloor(FloorConfigs[0]);
    }

    private void OnEnable()
    {
        ChamberTrigger.OnColliderTriggerEnter += ChamberTriggerEnter;
        ChamberTrigger.OnColliderTriggerExit += ChamberTriggerExit;
    }

    private void OnDisable()
    {
        ChamberTrigger.OnColliderTriggerEnter -= ChamberTriggerEnter;
        ChamberTrigger.OnColliderTriggerExit -= ChamberTriggerExit;
    }

    private void FixedUpdate()
    {
        if (CurrentFloorIndex == -1)
        {
            return;
        }

        // ChamberBody.MovePosition(transform.position + CurrentHeight * Vector3.up);
    }

    [Button]
    public void SwitchToFloor(int floorIndex)
    {
        if (floorIndex < 0 || floorIndex >= FloorConfigs.Length)
        {
            return;
        }

        if (CurrentFloorIndex == floorIndex)
        {
            return;
        }

        bool isInitializing = CurrentFloorIndex == -1;
        float currentHeight = isInitializing ? 0.0f : CurrentFloorConfig.Height;

        CurrentFloorIndex = floorIndex;

        float newHeight = CurrentFloorConfig.Height;
        ChamberBody.position = transform.position + Vector3.up * newHeight;

        float diffHeight = newHeight - currentHeight;
        Vector3 diff = Vector3.up * diffHeight;

        // Teleport any bodies within the chamber
        foreach (var chamberBody in CurrentChamberBodies)
        {
            chamberBody.position += diff;
        }
        foreach (var chamberPlayerCharacter in CurrentChamberPhysicsTeleportables)
        {
            chamberPlayerCharacter.Teleport(chamberPlayerCharacter.Position + diff, false);
        }


        // if (isInitializing)
        // {
        //     CurrentHeight = CurrentFloorConfig.Height;
        // }
        // else
        // {
        //     if (CurrentHeightTween != null)
        //     {
        //         CurrentHeightTween.Value.Stop();
        //     }

        //     float duration = Mathf.Abs(targetHeight - CurrentHeight) / TravelSpeed;

        //     var tweenSettings = new TweenSettings(duration: duration, updateType: UpdateType.FixedUpdate, ease: Ease.InOutSine);
        //     CurrentHeightTween = Tween.Custom(startValue: CurrentHeight, endValue: targetHeight, settings: tweenSettings, onValueChange: newVal => CurrentHeight = newVal);
        // }

        // Tween.LocalPositionAtSpeed(target: ChamberTransform, endValue: Vector3.up * floorConfig.Height, averageSpeed: TravelSpeed, ease: Ease.InOutSine);
    }

    public void SwitchToFloor(ElevatorFloorConfig floorConfig)
    {
        int index = Array.FindIndex(FloorConfigs, v => v == floorConfig);
        SwitchToFloor(index);
    }

    private void CreateCamberControls()
    {
        const int CamberControlGridX = 2;
        int CamberControlGridY = FloorConfigs.Length / CamberControlGridX;

        const float CamberControlSpacingX = 0.4f;
        const float CamberControlSpacingY = 0.4f;

        float CamberControlPosOffsetX = (CamberControlGridX - 1) * CamberControlSpacingX * -0.5f;
        float CamberControlPosOffsetY = (CamberControlGridY - 1) * CamberControlSpacingY * -0.5f;

        ChamberControlInstances = new ElevatorChamberControl[FloorConfigs.Length];
        for (int i = 0; i < FloorConfigs.Length; ++i)
        {
            ElevatorFloorConfig floorConfig = FloorConfigs[i];

            var controlInstance = Instantiate(ChamberControlPrefab, ChamberControlsParent);
            ChamberControlInstances[i] = controlInstance;
            controlInstance.Setup(this, floorConfig, i);

            int cellX = i % CamberControlGridX;
            int cellY = i / CamberControlGridX;

            var position = new Vector3(cellX * CamberControlSpacingX + CamberControlPosOffsetX, cellY * CamberControlSpacingY + CamberControlPosOffsetY, 0.0f);
            var rotation = Quaternion.identity;

            controlInstance.transform.localPosition = position;
            controlInstance.transform.localRotation = rotation;

            ChamberExcludeBodies.Add(controlInstance.GetComponent<Rigidbody>());
        }
    }

    private void ChamberTriggerEnter(Collider collider)
    {
        var rb = collider.attachedRigidbody;
        if (rb != null)
        {
            if (ChamberExcludeBodies.Contains(rb))
            {
                return;
            }

            CurrentChamberBodies.Add(rb);
        }

        if (collider.gameObject.TryGetComponent<IPhysicsTeleportable>(out IPhysicsTeleportable playerCharacter))
        {
            CurrentChamberPhysicsTeleportables.Add(playerCharacter);
        }
    }

    private void ChamberTriggerExit(Collider collider)
    {
        var rb = collider.attachedRigidbody;
        if (rb != null)
        {
            if (ChamberExcludeBodies.Contains(rb))
            {
                return;
            }

            CurrentChamberBodies.Remove(rb);
        }

        if (collider.gameObject.TryGetComponent<IPhysicsTeleportable>(out IPhysicsTeleportable playerCharacter))
        {
            CurrentChamberPhysicsTeleportables.Remove(playerCharacter);
        }
    }

}
