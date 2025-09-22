using TriInspector;
using UnityEngine;

public class ElevatorChamberControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerInteractableAccessor PlayerInteractableAccessor;
    [SerializeField]
    private MeshRenderer EmissiveRenderer;
    [SerializeField]
    private Material CurrentEmissiveMaterial;
    [SerializeField]
    private Material NotCurrentEmissiveMaterial;

    private Elevator Elevator;
    private ElevatorFloorConfig FloorConfig;
    private int FloorIndex;

    private int LastElevatorCurrentFloorIndex = -1;

    private IPlayerInteractable PlayerInteractable;

    private void Start()
    {
        PlayerInteractable = new SimplePlayerInteractable("Control", new InteractAction(Elevator, FloorConfig));
        PlayerInteractableAccessor.Interactable = PlayerInteractable;
    }

    private void FixedUpdate()
    {
        if (LastElevatorCurrentFloorIndex != Elevator.CurrentFloorIndex)
        {
            bool isCurrentFloor = Elevator.CurrentFloorIndex == FloorIndex;
            EmissiveRenderer.sharedMaterial = isCurrentFloor ? CurrentEmissiveMaterial : NotCurrentEmissiveMaterial;
        }

        LastElevatorCurrentFloorIndex = Elevator.CurrentFloorIndex;
    }

    public void Setup(Elevator elevator, ElevatorFloorConfig floorConfig, int floorIndex)
    {
        Elevator = elevator;
        FloorConfig = floorConfig;
        FloorIndex = floorIndex;
    }

    private class InteractAction : IPlayerInteractableAction
    {
        public string Info => _Info;
        private readonly string _Info;

        private readonly Elevator Elevator;
        private readonly ElevatorFloorConfig FloorConfig;

        public InteractAction(Elevator elevator, ElevatorFloorConfig floorConfig)
        {
            Elevator = elevator;
            FloorConfig = floorConfig;
            _Info = $"Go to {floorConfig.Name}";
        }

        public PlayerInteractableActionPerformResult Perform(Player player)
        {
            Elevator.SwitchToFloor(FloorConfig);

            var result = new PlayerInteractableActionPerformResult(true);
            return result;
        }
    }
}