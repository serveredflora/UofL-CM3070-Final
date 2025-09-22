using UnityEngine;

[RequireComponent(typeof(Health))]
public class Death : MonoBehaviour
{
    [Header("Settings")]
    public Spawner DeathSpawner;

    [Header("References")]
    public Health Health;

    private void OnEnable()
    {
        Health.OnDeplete.AddListener(Trigger);
    }

    private void OnDisable()
    {
        Health.OnDeplete.RemoveListener(Trigger);
    }

    public void Trigger()
    {
        DeathSpawner.Trigger(transform.position);
        Destroy(gameObject);
    }
}