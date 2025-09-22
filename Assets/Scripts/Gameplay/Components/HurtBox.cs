using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class HurtBox : MonoBehaviour
{
    public UnityEvent<HitBox> OnHurt;
}