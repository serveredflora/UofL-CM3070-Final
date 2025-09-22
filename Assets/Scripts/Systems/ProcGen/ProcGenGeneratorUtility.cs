using System;
using System.Collections.Generic;
using UnityEngine;

public class ProcGenGeneratorUtility : IDisposable
{
    private readonly GameObject CategoryGameObject;
    private readonly Dictionary<string, GameObject> CategoryGameObjects = new();

    public ProcGenGeneratorUtility(GameObject ownerGameObject)
    {
        CategoryGameObject = new GameObject("Categories");
        CategoryGameObject.transform.SetParent(ownerGameObject.transform, false);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(CategoryGameObject);
        CategoryGameObjects.Clear();
    }

    public GameObject RequestCategoryGameObject(string category)
    {
        if (!CategoryGameObjects.TryGetValue(category, out GameObject gameObj))
        {
            gameObj = new GameObject(category);
            gameObj.transform.SetParent(CategoryGameObject.transform, false);

            CategoryGameObjects[category] = gameObj;
        }

        return gameObj;
    }

    public void ClearAllCategoryGameObjects()
    {
        foreach (var gameObj in CategoryGameObjects.Values)
        {
            UnityEngine.Object.Destroy(gameObj);
        }

        CategoryGameObjects.Clear();
    }
}