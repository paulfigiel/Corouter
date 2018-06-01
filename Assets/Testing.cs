using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public Texture2D texture;
    Routine routine;
    RoutineHandle handle,handle2;
    public float restriction = 10;
    int i = 0;
    Vector3 iVec = new Vector3();

    private void Awake()
    {
        for (int i = 0; i < 64000; i++)
        {
        Routine routine = new Routine(
            () => Timed(transform.position, Linear.LerpVector, (v) => transform.position = v, Random.insideUnitSphere, 1))
            .Then(() => Timed(transform.position, Linear.LerpVector, (v) => transform.position = v, Random.insideUnitSphere, 1))
            .Then(() => Timed(transform.position, Linear.LerpVector, (v) => transform.position = v, Random.insideUnitSphere, 1))
            .Then(() => Timed(transform.position, Linear.LerpVector, (v) => transform.position = v, Random.insideUnitSphere, 1));
        routine.Start();

        }
        routine = new Routine(ToBuilder);
        routine.Start();
    }

    IEnumerator Restarrt()
    {
        routine.Reset();
        yield return null;
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            handle = null;
            handle2 = routine.Start();
        }
        print(Corouter.Instance.RunningRoutine);
    }

    IEnumerator Starting()
    {
        print("Start");
        yield return null;
    }

    IEnumerator ToBuilder()
    {
        return Timed(Quaternion.identity, Linear.LerpQuaternion, (m) => transform.rotation = m, Quaternion.Euler(180, 180, 90), 3);
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
}
