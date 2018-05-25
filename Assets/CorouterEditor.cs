using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Corouter))]
public class CorouterEditor : Editor{
    public override void OnInspectorGUI()
    {
        Corouter corouter = (Corouter)target;

        foreach (var item in corouter.coroutineGroups)
        {
            if(DisplayGroup(item.Value))
            {
                break;
            }
        }
    }
    private bool DisplayGroup(CoroutineGroup group)
    {
        bool destroyed = false;
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(group.Name);
        if(GUILayout.Button("StopAllCoroutine"))
        {
            ((Corouter)target).StopGroup(group.Name);
            destroyed = true;
        }
        EditorGUILayout.EndHorizontal();
        return destroyed;
    }
}
