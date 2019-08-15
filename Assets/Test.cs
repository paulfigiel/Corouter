using Corouter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Routine r;
    public List<GameObject> gameObjects;
    void Start()
    {
        // Creating base routine with IEnumerator Test1
        r = new Routine(Test1);
        // Adding some behaviour to the routine using a nice Fluent API
        r.Wait(1).Do(() => print("Waiting 1 sec")).Wait(1).Do(() => print("Waiting for any key press")).If(()=>Input.anyKey).Do(() => print("Key pressed, launching thread...")).InsideThread(HeavyComputation).Do(() => print("Finished ---- Disabling all gameobjects from list."));
        // Map function execute a function on a list of elements, with one element excuted per frame
        r.Map(gameObjects, (g) => g.SetActive(false));
        // Starting coroutine
        r.Start();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            r.Stop();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            r.Start();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            r.Reset();
        }
    }

    IEnumerator Test1()
    {
        print("Routine Test1");
        yield return 5.0f;
        print(Time.time);
        yield return new Routine(Test2);
        print("End of Routine Test1");
    }
    IEnumerator Test2()
    {
        print("Routine Test2");
        print(Time.time);
        yield return null;
        print("End of Routine Test2");
    }
    void HeavyComputation()
    {
        float[] list = new float[100000000];
        for (int i = 0; i < list.Length; i++)
        {
            list[i] += Mathf.Sin(i);
        }
    }
}
