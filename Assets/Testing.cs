using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Testing : MonoBehaviour
{
    Routine routine;
    public GameObject prefab;
    private List<GameObject> InstancedPrefabs = new List<GameObject>();
    private object _lockObject = new object();
    private float[] hugeArray = new float[1000*100];

    private void Awake()
    {
        for (int i = 0; i <= 1000; i++)
        {
            GameObject g = Instantiate(prefab);
            g.transform.position = Random.insideUnitSphere * 100;
            InstancedPrefabs.Add(g);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            routine.Start();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            routine.Stop();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            routine.Reset();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            /*for (int i = 0; i < 1000; i++)
            {
                routine[i] = new Routine(
                    () => Timed(InstancedPrefabs[i].transform.rotation, Linear.LerpQuaternion, (v) => InstancedPrefabs[i].transform.rotation = v, Quaternion.Euler(180,90,0), 10));

                routine[i].Start();

            }*/
            routine = new Routine(
                () => Actions.TimedList((i) => InstancedPrefabs[i].transform.position, Linear.LerpVector((t)=> Mathf.Pow(t, 1 / 5f)), (v, index) => InstancedPrefabs[index].transform.position = v, (i) => InstancedPrefabs[i].transform.position + Random.insideUnitSphere * 100, 0, 1000, 10));
            //(t)=>Mathf.Pow(t,1/5f)
            //(t)=>Mathf.Pow(t,5)
            routine.Start();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Routine threaded = new Routine(() => Actions.Threaded(Test,_lockObject));
            threaded.Start();
        }
        print(Corouter.Instance.RunningRoutine);
    }

    void Test()
    {
        Debug.Log("bug");
        for (int i = 0; i < hugeArray.Length; i++)
        {
            hugeArray[i] = i;
        }
        print("Array : "+hugeArray[hugeArray.Length-1]);
    }

    IEnumerator Starting()
    {
        print(InstancedPrefabs.FindAll((g) => g).Count);
        yield return null;
    }
}