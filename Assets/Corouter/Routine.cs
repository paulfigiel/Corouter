using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Routine {
    IEnumerator currentEnumerator;
    Queue<IEnumerator> enumerators = new Queue<IEnumerator>();

    bool MoveNext()
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
                return false;
        }
    }
}
