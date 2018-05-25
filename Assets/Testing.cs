using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
    private List<Vector3> list = new List<Vector3>(100000);
    public bool kill = true;

    private void Awake()
    {
        Corouter.Instance.StartCoroutine(Corouter.Base.Then(() => print("start")).Then(Count).Then(() => print("Finished")),"OtherGroup");
        Corouter.Instance.SetGroupHandle(()=>this,"OtherGroup");
    }

    IEnumerator Count()
    {
        int i = 0;
        while(i<5)
        {
            Debug.Log(i);
            i++;
            yield return new WaitForSeconds(1);
        }
    }

    void Update () {

	}
}
