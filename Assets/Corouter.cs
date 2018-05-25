using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Corouter : Singleton<Corouter> {
    public const string SHARED_COROUTINES = "SHARED_COROUTINES";
    public static IEnumerator Base
    {
        get
        {
            yield return 0;
        }
    }
    //public List<CoroutineGroup> coroutineGroups = new List<CoroutineGroup>();
    public Dictionary<string, CoroutineGroup> coroutineGroups = new Dictionary<string, CoroutineGroup>();

    private List<CoroutineGroup> garbageCollected = new List<CoroutineGroup>();

    private void Update()
    {
        garbageCollected.Clear();
        foreach (var item in coroutineGroups)
        {
            if(!item.Value.handle())
            {
                garbageCollected.Add(item.Value);
            }
        }
        garbageCollected.ForEach((g) => StopGroup(g.Name));
    }
    public void SetGroupHandle(System.Func<bool> func,string group)
    {
        GetGroup(group).handle = func;
    }
    protected override void Awake()
    {
        base.Awake();
        AddGroup(SHARED_COROUTINES);
    }
    new public Coroutine StartCoroutine(IEnumerator enumerator)
    {
        return AddGroup(SHARED_COROUTINES).StartCoroutine(enumerator);
    }
    public Coroutine StartCoroutine(IEnumerator enumerator,string coroutineGroup)
    {
        return AddGroup(coroutineGroup).StartCoroutine(enumerator);
    }
    public void StopGroup(string group)
    {
        CoroutineGroup coroutineGroup = GetGroup(group);
        coroutineGroup.StopAllCoroutines();
        coroutineGroups.Remove(group);
        Destroy(coroutineGroup.gameObject);
    }
    string[] GetGroups()
    {
        return coroutineGroups.Keys.ToArray();
    }
    CoroutineGroup GetGroup(string groupName)
    {
        return coroutineGroups[groupName];
    }
    CoroutineGroup AddGroup(string groupName)
    {
        if (coroutineGroups.ContainsKey(groupName))
            return coroutineGroups[groupName];
        else
        {
            GameObject g = new GameObject(groupName);
            CoroutineGroup group = g.AddComponent<CoroutineGroup>();
            group.Name = groupName;
            group.transform.parent = transform;
            coroutineGroups.Add(groupName, group);
            return group;
        }
    }
}
