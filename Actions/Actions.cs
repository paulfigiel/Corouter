using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static partial class Actions
{
    public static IEnumerator Timed<T>(T init, System.Func<T, T, float, T> lerpFunction, System.Action<T> assign, T endvalue, float duration)
    {
        return Timed(() => init, lerpFunction, assign, () => endvalue, duration);
    }
    public static IEnumerator Timed<T>(System.Func<T> init, System.Func<T, T, float, T> lerpFunction, System.Action<T> assign, System.Func<T> end, float duration)
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
    public static IEnumerator TimedList<T>(System.Func<int, T> init, System.Func<T, T, float, T> lerpFunction, System.Action<T, int> assign, System.Func<int, T> end, int minIndex, int maxIndex, float duration)
    {
        float startedTime = Time.time;
        float buff = 0;
        T[] startValues = new T[maxIndex - minIndex];
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
                assign(lerpFunction(startValues[i + minIndex], endValues[i + minIndex], buff), i);
            }
            yield return null;
            buff += Time.deltaTime / duration;
        }
        for (int i = minIndex; i < maxIndex; i++)
        {
            assign(endValues[i + minIndex], i);
        }
    }
    public static IEnumerator SliceAction<T>(IList<T> collection, System.Action<T> action, int iterationPerFrame = 1)
    {
        int iteration = 0;
        int i = 0;
        while (i < collection.Count)
        {
            iteration = 0;
            while (iteration < iterationPerFrame && i < collection.Count)
            {
                action(collection[i]);
                i++;
                iteration++;
            }
            yield return null;
        }
    }

    public static IEnumerator Threaded(System.Action action, object lockobject = null)
    {
        Thread t = new Thread(
            () =>
            {
                lock (lockobject)
                {
                    action();
                }
            }
            );
        t.Start();
        while (t.IsAlive)
        {
            yield return null;
        }
    }
}
