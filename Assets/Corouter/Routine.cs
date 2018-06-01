using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Routine {
    System.Func<IEnumerator> enumeratorFunc;
    IEnumerator currentEnumerator;
    Queue<IEnumerator> enumerators = new Queue<IEnumerator>();
    private bool running = false;
    public bool Running
    {
        get
        {
            return running;
        }
    }

    public Routine(System.Func<IEnumerator> enumerator)
    {
        enumeratorFunc = enumerator;
        currentEnumerator = enumeratorFunc();
        Corouter.Instance.RegisterRoutine(this);
    }

    public Routine(params Func<IEnumerator>[] functions)
    {

    }

    public void Start()
    {
        running = true;
    }

    public void Stop()
    {
        running = false;
    }

    public void Reset()
    {
        currentEnumerator = enumeratorFunc();
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
            else if(currentEnumerator.Current is Result)
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
            else
            {
                running = false;
                return false;
            }
        }
    }

    ~Routine()
    {
        Corouter.Instance.RegisterRoutineDestruction(this);
    }
}
