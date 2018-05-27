using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Routine {
    private System.Func<IEnumerator> _enumerator;
    public System.Func<IEnumerator> Enumerator { get { return _enumerator; } }
    public Routine(System.Func<IEnumerator> enumerator)
    {
        _enumerator = enumerator;
        Corouter.Instance.RegisterRoutine(this);
    }
    public void Start()
    {
        Corouter.Instance.StartRoutine(this);
    }
    public void Stop()
    {
        Corouter.Instance.StopRoutine(this);
    }
    public void Reset()
    {
        Stop();
        Start();
    }
    public Routine Then(System.Func<IEnumerator> enumerator)
    {
        return new Routine(()=>Then(_enumerator(),enumerator));
    }
    public Routine Then(System.Action action)
    {
        return new Routine(() => Then(_enumerator(), action));
    }
    public Routine Repeat(System.Action action, int howMany, float maxTimeAllowedPerFrame = 1)
    {
        return new Routine(()=>Repeat(_enumerator(),action,howMany,maxTimeAllowedPerFrame));
    }
    private IEnumerator Then(IEnumerator baseEnum, System.Func<IEnumerator> enumerator)
    {
        yield return baseEnum;
        yield return enumerator();
    }
    private IEnumerator Then<T>(IEnumerator baseEnum, System.Func<T, IEnumerator> enumerator, T obj1)
    {
        yield return baseEnum;
        yield return enumerator(obj1);
    }
    private IEnumerator Then(IEnumerator baseEnum, System.Action action)
    {
        yield return baseEnum;
        action();
    }
    private IEnumerator Repeat(IEnumerator baseEnum, System.Action action, int howMany, float maxTimeAllowedPerFrame = 1)
    {
        yield return baseEnum;
        maxTimeAllowedPerFrame /= 1000;
        int i = 0;
        while (i < howMany)
        {
            float startedTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup-startedTime < maxTimeAllowedPerFrame && i < howMany)
            {
                action();
                i++;
            }
            if (i != howMany)
                yield return null;
        }
    }
    private IEnumerator Repeat(IEnumerator baseEnum, System.Action action, int howMany, int iterationPerFrame = 1)
    {
        yield return baseEnum;
        int i = 0, j = 0;
        iterationPerFrame = Mathf.Min(iterationPerFrame, howMany);
        while (i < howMany)
        {
            while (j < iterationPerFrame && i < howMany)
            {
                action();
                j++;
                i++;
            }
            j = 0;
            if (i != howMany)
                yield return null;
        }
    }
}
