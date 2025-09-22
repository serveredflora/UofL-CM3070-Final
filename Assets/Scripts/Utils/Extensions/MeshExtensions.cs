using UnityEngine;

public static class MeshExtensions
{
    public static Mesh Clone(this Mesh mesh)
    {
        var combineInstances = new CombineInstance[1];
        combineInstances[0].mesh = mesh;
        combineInstances[0].transform = Matrix4x4.identity;

        Mesh clone = new();
        clone.CombineMeshes(combineInstances);
        return clone;
    }
}