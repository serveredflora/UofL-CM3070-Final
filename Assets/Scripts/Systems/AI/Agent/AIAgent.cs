using System.Collections.Generic;
using System.Linq;
using FSM;
using TriInspector;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    [Header("Settings")]
    public StateAsset InitialState;

    [SerializeReference]
    public IAIAgentTarget Target;

    [Space(10)]
    public List<Vector3> Waypoints;
    private int CurrentWaypoint;

    [Space(10)]
    [Tooltip("Only evaluate the state machine every X fixed updates, to reduce CPU load and/or support more agents at once")]
    public int TimeSlicingInterval = 5;

    [Header("References")]
    [SerializeField]
    private NavMeshAgent Agent;
    [SerializeField]
    [AssetsOnly]
    private GameObject AttackPrefab;

    private FiniteStateMachine StateMachine;
    private IFiniteStateMachineStorage StateMachineStorage = new SimpleFiniteStateMachineStorage();

    private int TimeSlicingPos = 0;

    private void Start()
    {
        // Randomize avoidance priority to reduce "deadlocks" when multiple agents just block each other
        // due to having the same priority
        Agent.avoidancePriority = Random.Range(1, 99);

        CurrentWaypoint = 0;

        StateMachineStorage = new SimpleFiniteStateMachineStorage();
        StateMachineStorage.Write<AIAgent>(this);

        StateMachine = new(StateMachineStorage, InitialState);

        // Randomize where the time slicing "position" starts from
        TimeSlicingPos = Random.Range(0, TimeSlicingInterval);
    }

    private void FixedUpdate()
    {
        TimeSlicingPos = (TimeSlicingPos + 1) % TimeSlicingInterval;
        if (TimeSlicingPos == 0)
        {
            StateMachine.Tick();
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        // Draw current state name for agents near the camera
        float maxDistFromCam = 20.0f;
        if (Target != null && Target.IsValid && (Camera.main.transform.position - transform.position).magnitude <= maxDistFromCam)
        {
            // ALINE usage commented out
            // Drawing.Draw.Label2D(transform.position, (StateMachine.CurrentState as StateAsset).name, Color.white);
        }
    }
#endif

    public void GoToNextWaypoint()
    {
        Agent.destination = Waypoints[CurrentWaypoint];
        CurrentWaypoint++;
        if (CurrentWaypoint >= Waypoints.Count)
        {
            CurrentWaypoint = 0;
        }
    }

    public void GoToTarget()
    {
        if (Target.IsValid)
        {
            Agent.destination = Target.Position;
        }
    }

    public void StopMoving()
    {
        Agent.isStopped = true;
        Agent.ResetPath();
    }

    public bool IsAtDestination()
    {
        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void PerformAttack()
    {
        // The prefab is responsible for performing the attack
        Instantiate(AttackPrefab, transform.position, transform.rotation);
    }
}