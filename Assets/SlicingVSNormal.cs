using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicingVSNormal : MonoBehaviour {
    public GameObject sphere;
    public List<GameObject> gameobjects;
    public int howMany = 5000;
    public int slicing = 100;

	void Start () {
		
	}
	

	void Update () {
		/*if(Input.GetKeyDown(KeyCode.S))
        {
            Corouter.Instance.StartCoroutine(
                Corouter.Base
                .While(()=>true,loop)
                );
        }*/
    }

    /*IEnumerator loop()
    {
        return Corouter.Base
                .Repeat(() => InstanciateSphere(), howMany, 100)
                .Then(() => InstanciateShpereLoop())
                .Wait(1)
                .Then(() => Clear());
    }*/

    void InstanciateSphere()
    {
        GameObject g = Instantiate(sphere);
        gameobjects.Add(g);
        g.transform.position = Random.insideUnitSphere * 100;
    }

    void InstanciateShpereLoop()
    {
        for (int i = 0; i < howMany; i++)
        {
            InstanciateSphere();
        }
    }

    void Clear()
    {
        gameobjects.ForEach((g) => Destroy(g));
        gameobjects.Clear();
    }
}
