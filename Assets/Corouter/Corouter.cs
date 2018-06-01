using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Corouter : Singleton<Corouter>
{
    private List<Routine> routines = new List<Routine>();
    private List<Routine> runningRoutines = new List<Routine>();
    private Dictionary<Routine, Coroutine> routineToCoroutine = new Dictionary<Routine, Coroutine>();

    public void RegisterRoutine(Routine routine)
    {
        routines.Add(routine);
    }

    public void RegisterRoutineDestruction(Routine routine)
    {
        print("Routine destroyed");
        routines.Remove(routine);
    }

    private void Update()
    {
        for (int i = 0; i < routines.Count; i++)
        {
            if(routines[i].Running)
                routines[i].Tick();
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
