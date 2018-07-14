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
        runningRoutines.RemoveAll((r) => !r.Running);
        for (int i = 0; i < runningRoutines.Count; i++)
        {
            runningRoutines[i].Tick();
        }
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
