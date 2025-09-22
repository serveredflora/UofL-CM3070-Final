using System;
using UnityEngine;

public class BuildingInteriorEnemy : IDisposable
{
    public AIAgent AgentInstance;

    public void Dispose()
    {
        if (AgentInstance != null)
        {
            GameObject.Destroy(AgentInstance.gameObject);
        }
    }
}