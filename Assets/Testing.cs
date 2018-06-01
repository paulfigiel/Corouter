using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public Texture2D texture;
    Routine routine;
    public float restriction = 10;
    float i = 0;
    Vector3 iVec = new Vector3();
    Quaternion initialRotation;

    private void Awake()
    {
        for (int i = 0; i < 64000; i++)
        {
            routine = new Routine(ToBuilder);
            routine.Start();
            initialRotation = transform.rotation;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            routine.Start();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            routine.Stop();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            routine.Reset();
        }
    }

    IEnumerator Starting()
    {
        print("Start");
        yield return null;
    }

    IEnumerator ToBuilder()
    {
        return Timed(Quaternion.identity, LerpQuaternion, (m) => transform.rotation = m, Quaternion.Euler(180, 180, 90), 10);
    }

    IEnumerator Timed<T>(T init, System.Func<T, T, float, T> lerpFunction, System.Action<T> assign, T endvalue, float duration)
    {
        return Timed(() => init, lerpFunction, assign, endvalue, duration);
    }
    IEnumerator Timed<T>(System.Func<T> init, System.Func<T, T, float, T> lerpFunction, System.Action<T> assign, T endvalue, float duration)
    {
        float startedTime = Time.time;
        float buff = 0;
        T startValue = init();
        while (buff <= 1)
        {
            assign(lerpFunction(startValue, endvalue, buff));
            yield return null;
            buff += Time.deltaTime / duration;
        }
        assign(endvalue);
    }

    System.Func<float, float, float, float> Lerp
    {
        get
        {
            return (v0, v1, t) => (1 - t) * v0 + t * v1;
        }
    }

    System.Func<double, double, float, double> LerpDouble
    {
        get
        {
            return (v0, v1, t) => (1 - t) * v0 + t * v1;
        }
    }

    System.Func<Vector3, Vector3, float, Vector3> LerpVector
    {
        get
        {
            return (v0, v1, t) => new Vector3(Lerp(v0.x, v1.x, t), Lerp(v0.y, v1.y, t), Lerp(v0.z, v1.z, t));
        }
    }

    System.Func<Quaternion, Quaternion, float, Quaternion> LerpQuaternion
    {
        get
        {
            return (v0, v1, t) => new Quaternion((1 - t) * v0.x + t * v1.x, (1 - t) * v0.y + t * v1.y, (1 - t) * v0.z + t * v1.z, (1 - t) * v0.w + t * v1.w);
        }
    }
}
