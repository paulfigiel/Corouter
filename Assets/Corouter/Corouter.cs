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

    public void RegisterRoutineDestruction(Routine routine)
    {
        print("Routine destroyed");
        runningRoutines.Remove(routine);
    }

    private void Update()
    {
        for (int i = 0; i < runningRoutines.Count; i++)
        {
            if (runningRoutines[i].Running)
                runningRoutines[i].Tick();
        }
        runningRoutines.RemoveAll((r) => r.DestroyAfterFinished && !r.Running);
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
