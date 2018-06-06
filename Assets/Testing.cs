using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    Routine routine;
    public GameObject prefab;
    private List<GameObject> InstancedPrefabs = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i <= 64000; i++)
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
                () => Actions.TimedList((i) => InstancedPrefabs[i].transform.position, Linear.LerpVector, (v, index) => InstancedPrefabs[index].transform.position = v, (i) => InstancedPrefabs[i].transform.position + Random.insideUnitSphere * 100, 0, 64000, 1));
            routine.Then(()=>Actions.SliceAction(InstancedPrefabs, (g) => Destroy(g), 64010));
            routine.Then(Starting);

            routine.Start();
        }
        print(Corouter.Instance.RunningRoutine);
    }

    IEnumerator Starting()
    {
        print(InstancedPrefabs.FindAll((g)=>g).Count);
        yield return null;
    }
}