using System;
using UnityEngine;

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.items;
    }

    public static string ToJson<T>(T[] array, bool prettyPrint = false)
    {
        Wrapper<T> wrapper = new Wrapper<T> { items = array };
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }
}
