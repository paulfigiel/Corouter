using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Corouter : Singleton<Corouter>
{
    private List<Routine> runningRoutines = new List<Routine>();

    public int RunningRoutine
    {
        get
        {
            return runningRoutines.Count;
        }
    }

    public void RegisterRoutine(Routine routine)
    {
        runningRoutines.Add(routine);
    }

    private void Update()
    {
        for (int i = 0; i < runningRoutines.Count; i++)
        {
            if (runningRoutines[i].Running)
                runningRoutines[i].Tick();
        }
        runningRoutines.RemoveAll((r) => !r.handle.IsAlive && !r.Running);
    }

    private void LateUpdate()
    {

    }

    private void FixedUpdate()
    {

    }

    protected override void Awake()
    {
        base.Awake();
    }
}
