using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    Routine routine;
    public GameObject prefab;
    private List<GameObject> InstancedPrefabs = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i <= 10000; i++)
        {
            GameObject g = Instantiate(prefab);
            g.transform.position = Random.insideUnitSphere * 100;
            InstancedPrefabs.Add(g);
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            /*for (int i = 0; i < 1000; i++)
            {
                routine[i] = new Routine(
                    () => Timed(InstancedPrefabs[i].transform.rotation, Linear.LerpQuaternion, (v) => InstancedPrefabs[i].transform.rotation = v, Quaternion.Euler(180,90,0), 10));

                routine[i].Start();

            }*/
                routine = new Routine(
                    () => TimedList((i)=>InstancedPrefabs[i].transform.position, Linear.LerpVector, (v,index) => InstancedPrefabs[index].transform.position = v, (i)=>Random.insideUnitSphere*100,0,10000, 10));

                routine.Start();
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
        return Timed(() => init, lerpFunction, assign, ()=>endvalue, duration);
    }
    IEnumerator Timed<T>(System.Func<T> init, System.Func<T, T, float, T> lerpFunction, System.Action<T> assign, System.Func<T> end, float duration)
    {
        float startedTime = Time.time;
        float buff = 0;
        T startvalue = init();
        T endvalue = end();
        while (buff <= 1)
        {
            assign(lerpFunction(startvalue, endvalue, buff));
            yield return null;
            buff += Time.deltaTime / duration;
        }
        assign(endvalue);
    }
    IEnumerator TimedList<T>(System.Func<int,T> init, System.Func<T, T, float, T> lerpFunction, System.Action<T,int> assign, System.Func<int,T> end, int minIndex,int maxIndex, float duration)
    {
        float startedTime = Time.time;
        float buff = 0;
        T[] startValues = new T[maxIndex-minIndex];
        for (int i = 0; i < startValues.Length; i++)
        {
            startValues[i] = init(i);
        }
        T[] endValues = new T[maxIndex - minIndex];
        for (int i = 0; i < endValues.Length; i++)
        {
            endValues[i] = end(i);
        }
        while (buff <= 1)
        {
            for (int i = minIndex; i < maxIndex; i++)
            {
                assign(lerpFunction(startValues[i+minIndex], endValues[i+minIndex], buff),i);
            }
            yield return null;
            buff += Time.deltaTime / duration;
        }
        for (int i = minIndex; i < maxIndex; i++)
        {
            assign(endValues[i + minIndex], i);
        }
    }
}
