using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ElevatorChamberTrigger : MonoBehaviour
{
    public IReadOnlyList<Rigidbody> Bodies => _Bodies;
    private List<Rigidbody> _Bodies = new();
}