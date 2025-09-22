using TriInspector;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    public GameObject PrefabToSpawn;

    [Button]
    [EnableInPlayMode]
    public void Trigger() => Trigger(transform.position);

    public GameObject Trigger(Vector3 spawnPos, Transform spawnParent = null)
    {
        Debug.Assert(PrefabToSpawn != null);

        var instance = spawnParent != null ? Instantiate(PrefabToSpawn, spawnParent) : Instantiate(PrefabToSpawn);
        instance.transform.position = spawnPos;

        return instance;
    }
}