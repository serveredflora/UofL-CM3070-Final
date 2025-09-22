using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColliderTriggerEvents : MonoBehaviour
{
    public event Action<Collider> OnColliderTriggerEnter;
    public event Action<Collider> OnColliderTriggerStay;
    public event Action<Collider> OnColliderTriggerExit;

    private void OnTriggerEnter(Collider other) => OnColliderTriggerEnter?.Invoke(other);
    private void OnTriggerStay(Collider other) => OnColliderTriggerStay?.Invoke(other);
    private void OnTriggerExit(Collider other) => OnColliderTriggerExit?.Invoke(other);
}