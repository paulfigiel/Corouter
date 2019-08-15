using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Corouter
{
    public class CorouterManager : Singleton<CorouterManager>
    {
        private HashSet<Routine> runningRoutines = new HashSet<Routine>();

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

        public void UnregisterRoutine(Routine routine)
        {
            runningRoutines.Remove(routine);
        }

        private void Update()
        {
            foreach (var routine in runningRoutines)
            {
                if (routine.Running)
                    routine.Tick();
            }
            runningRoutines.RemoveWhere((r) => !r.Running);
        }
    }
}