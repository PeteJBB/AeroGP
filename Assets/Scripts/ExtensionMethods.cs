using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods
{
    public static Transform[] FindChildren(this Transform t, string name)
    {
        var children = new List<Transform>();
        for (var i = 0; i < t.childCount; i++)
        {
            var child = t.GetChild(i);
            if(child.name == name)
                children.Add(child);
        }
        return children.ToArray();
    }

    public static Vector3 IgnoreX(this Vector3 v, float newX = 0)
    {
        return new Vector3(newX, v.y, v.z);
    }
    
    public static Vector3 IgnoreY(this Vector3 v, float newY = 0)
    {
        return new Vector3(v.x, newY, v.z);
    }

    public static Vector3 IgnoreZ(this Vector3 v, float newZ = 0)
    {
        return new Vector3(v.x, v.y, newZ);
    }


}
