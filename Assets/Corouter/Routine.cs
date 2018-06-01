using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Routine
{
    List<System.Func<IEnumerator>> enumeratorFuncs;
    int enumeratorFuncIndex = 0;
    IEnumerator currentEnumerator;
    Queue<IEnumerator> enumerators = new Queue<IEnumerator>();
    private bool _running = false;
    public WeakReference handle;
    public bool Running
    {
        get
        {
            return _running;
        }
    }

    public Routine(params Func<IEnumerator>[] enumerators)
    {
        enumeratorFuncs = new List<Func<IEnumerator>>(enumerators);
        currentEnumerator = enumeratorFuncs[0]();
    }

    public RoutineHandle Start()
    {
        Corouter.Instance.RegisterRoutine(this);
        _running = true;
        RoutineHandle routineHandle = new RoutineHandle();
        handle = new WeakReference(routineHandle);
        return routineHandle;
    }

    public void Stop()
    {
        _running = false;
    }

    public void Reset()
    {
        enumeratorFuncIndex = 0;
        currentEnumerator = enumeratorFuncs[0]();
    }

    public bool Tick()
    {
        bool b = currentEnumerator.MoveNext();
        if (b)
        {
            // Skipping a frame when it's null
            if (currentEnumerator.Current == null)
            {

            }
            else if (currentEnumerator.Current is Result)
            {

            }
            else if (currentEnumerator.Current is IEnumerator)
            {
                enumerators.Enqueue(currentEnumerator);
                currentEnumerator = currentEnumerator.Current as IEnumerator;
                currentEnumerator.MoveNext();
            }
            return true;
        }
        else
        {
            if (enumerators.Count > 0)
            {
                currentEnumerator = enumerators.Dequeue();
                currentEnumerator.MoveNext();
                return true;
            }
            else if (enumeratorFuncIndex + 1 < enumeratorFuncs.Count)
            {
                currentEnumerator = enumeratorFuncs[enumeratorFuncIndex + 1]();
                currentEnumerator.MoveNext();
                enumeratorFuncIndex++;
                return true;
            }
            else
            {
                _running = false;
                return false;
            }
        }
    }
    public Routine Then(Func<IEnumerator> other)
    {
        enumeratorFuncs.Add(other);
        return this;
    }
}
public class RoutineHandle
{
    public RoutineHandle()
    {
    }
    ~RoutineHandle()
    {
    }
}