using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineGroup : MonoBehaviour
{
    public System.Func<bool> handle = () => true;
    public string Name = Corouter.SHARED_COROUTINES;
}
