using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class BlockReplacer : MonoBehaviour
{
    private List<Transform> _blocks;

    void Start()
    {
    }

    [ContextMenu("ToIntPosition")]
    private void ReplaceAll()
    {
        foreach (Transform child in transform)
        {
             Undo.RecordObject(child,"Move"); 
            var pos = child.localPosition;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            pos.z = Mathf.Round(pos.z);
            child.localPosition = pos;
            EditorUtility.SetDirty(child);
       }
   }
    [ContextMenu("Sort")]
    private void Sort()
    {
        var children = GetComponentsInChildren<Transform>();
        var c = children.OrderBy(x => x.position).ToArray();
        for (var i = 0; i < c.Length; i++)
        {
            c[i].SetSiblingIndex(i);
            c[i].name = "block " + i.ToString();
        }
    }
    
    [ContextMenu("Distinct")]
    private void Distinct()
    {
        var c = GetComponentsInChildren<Transform>().GroupBy(x => x.position).SelectMany(grp => grp.Skip(1)).ToArray();

        for (var i = 0; i < c.Length; i++)
        {
            DestroyImmediate(c[i].gameObject);
        }
        
    }

    private class PositionComparer : IEqualityComparer<Transform>
    {
        public bool Equals(Transform x, Transform y)
        {
            return x.position == y.position;
        }

        public int GetHashCode(Transform obj)
        {
            return obj.position.GetHashCode();
        }
    }

    void Update()
    {
    }
}