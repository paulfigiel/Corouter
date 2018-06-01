using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Linear {

    public static System.Func<float, float, float, float> Lerp
    {
        get
        {
            return (v0, v1, t) => (1 - t) * v0 + t * v1;
        }
    }

    public static System.Func<double, double, float, double> LerpDouble
    {
        get
        {
            return (v0, v1, t) => (1 - t) * v0 + t * v1;
        }
    }

    public static System.Func<Vector3, Vector3, float, Vector3> LerpVector
    {
        get
        {
            return (v0, v1, t) => new Vector3(Lerp(v0.x, v1.x, t), Lerp(v0.y, v1.y, t), Lerp(v0.z, v1.z, t));
        }
    }

    public static System.Func<Quaternion, Quaternion, float, Quaternion> LerpQuaternion
    {
        get
        {
            return (v0, v1, t) => new Quaternion((1 - t) * v0.x + t * v1.x, (1 - t) * v0.y + t * v1.y, (1 - t) * v0.z + t * v1.z, (1 - t) * v0.w + t * v1.w);
        }
    }

    public static System.Func<Color, Color, float, Color> LerpColor
    {
        get
        {
            return (v0, v1, t) => new Color(Lerp(v0.r, v1.r, t), Lerp(v0.g, v1.g, t), Lerp(v0.b, v1.b, t), Lerp(v0.a, v1.a, t));
        }
    }
}
