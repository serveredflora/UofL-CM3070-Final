using UnityEngine;

public static class GameObjectUtils
{
    public static GameObject InstantiateTRS(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        GameObject gameObj = GameObject.Instantiate(prefab, position, rotation, parent);
        gameObj.transform.localScale = scale;
        return gameObj;
    }

    public static T InstantiateTRS<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
        where T : Component
    {
        T obj = GameObject.Instantiate<T>(prefab, position, rotation, parent);
        obj.gameObject.transform.localScale = scale;
        return obj;
    }
}