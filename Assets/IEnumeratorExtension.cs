using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IEnumeratorExtension
{
    public delegate void LerpDelegate(float lerp);
    public static IEnumerator Base()
    {
        yield return null;
    }
    public static IEnumerator Then(this IEnumerator baseEnum, System.Func<IEnumerator> enumerator)
    {
        yield return baseEnum;
        yield return enumerator();
    }
    public static IEnumerator Then<T>(this IEnumerator baseEnum, System.Func<T, IEnumerator> enumerator, T obj1)
    {
        yield return baseEnum;
        yield return enumerator(obj1);
    }
    public static IEnumerator Then(this IEnumerator baseEnum, System.Action action)
    {
        yield return baseEnum;
        action();
    }
    public static IEnumerator LerpValue(this IEnumerator baseEnum, System.Action<float> function, float from, float to, float duration)
    {
        yield return baseEnum;
        float elapsed = 0;
        while (elapsed < duration)
        {
            function(Mathf.Lerp(from, to, elapsed / duration));
            yield return null;
            elapsed += Time.deltaTime;
        }
        function(to);
    }
    public static IEnumerator Wait(this IEnumerator baseEnum, float time)
    {
        yield return baseEnum;
        yield return new WaitForSeconds(time);
    }
    public static IEnumerator Predicate(this IEnumerator baseEnum, System.Func<bool> predicate)
    {
        yield return baseEnum;
        while (!predicate())
        {
            yield return null;
        }
    }
    /// <summary>
    /// While the predicate is false, the enumerator is reapeating
    /// </summary>
    /// <param name="baseEnum"></param>
    /// <returns></returns>
    public static IEnumerator While(this IEnumerator baseEnum, System.Func<bool> predicate, System.Func<IEnumerator> looping)
    {
        yield return baseEnum;
        while (predicate())
        {
            yield return looping();
        }
    }
    public static IEnumerator Iterate<T>(this IEnumerator baseEnum, IList<T> list, System.Func<T, T> function, int iterationPerFrame = 1)
    {
        yield return baseEnum;
        int i = 0, j = 0;
        iterationPerFrame = Mathf.Min(iterationPerFrame, list.Count);
        while (i < list.Count)
        {
            while (j < iterationPerFrame && i<list.Count)
            {
                list[i] = function(list[i]);
                i++;
                j++;
            }
            j = 0;
            if(i!=list.Count)
                yield return null;
        }
    }
    public static IEnumerator Repeat(this IEnumerator baseEnum,System.Action action,int howMany,int iterationPerFrame = 1)
    {
        yield return baseEnum;
        int i = 0, j = 0;
        iterationPerFrame = Mathf.Min(iterationPerFrame, howMany);
        while (i < howMany)
        {
            while(j<iterationPerFrame && i < howMany)
            {
                action();
                j++;
                i++;
            }
            j = 0;
            if(i!= howMany)
                yield return null;
        }
    }
}