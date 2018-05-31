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

    public void StartRoutine(Routine routine)
    {
        /*routines.Remove(routine);
        runningRoutines.Add(routine);
        Coroutine c = StartCoroutine(routine.Enumerator());
        if (!routineToCoroutine.ContainsKey(routine))
            routineToCoroutine.Add(routine, c);
        else
            routineToCoroutine[routine] = c;*/
    }

    public void StopRoutine(Routine routine)
    {
        /*if (!runningRoutines.Contains(routine))
            return;
        runningRoutines.Remove(routine);
        routines.Add(routine);
        if (routineToCoroutine.ContainsKey(routine))
            StopCoroutine(routineToCoroutine[routine]);*/
    }

    private void Update()
    {

    }

    protected override void Awake()
    {
        base.Awake();
    }
}
