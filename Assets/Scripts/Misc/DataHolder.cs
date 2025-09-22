using System;
using System.Collections.Generic;

public class DataHolder
{
    private Dictionary<Type, object> AttachedData = new();

    public TData GetDataAttached<TData>()
    {
        return (TData)AttachedData[typeof(TData)];
    }

    public bool HasDataAttached<TData>()
    {
        return AttachedData.ContainsKey(typeof(TData));
    }

    public bool TryGetDataAttached<TData>(out TData typedData)
    {
        if (AttachedData.TryGetValue(typeof(TData), out object data))
        {
            typedData = (TData)data;
            return true;
        }

        typedData = default(TData);
        return false;
    }

    public void AddFlag<TData>()
        where TData : new()
    {
        AttachedData.Add(typeof(TData), new TData());
    }

    public void RemoveFlag<TData>()
        where TData : new()
    {
        AttachedData.Remove(typeof(TData));
    }

    public void AttachData<TData>(TData data)
    {
        AttachedData.Add(typeof(TData), data);
    }

    public void AttachData(Type type, object data)
    {
        AttachedData.Add(type, data);
    }

    public bool TryAttachingData<TData>(TData data)
    {
        if (AttachedData.ContainsKey(typeof(TData)))
        {
            return false;
        }

        AttachedData.Add(typeof(TData), data);
        return true;
    }
}
