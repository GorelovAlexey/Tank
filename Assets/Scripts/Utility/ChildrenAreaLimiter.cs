using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Простой скрипт который управляет координатами всех обьектов потомков (которые были таковыми изначально)
/// и не дает им покинуть определенные рамки
/// </summary>
public class ChildrenAreaLimiter : MonoBehaviour
{
    [SerializeField]
    Vector3 worldLowerBound;

    [SerializeField]
    Vector3 worldHigherBound;

    Transform[] targets;

    // Start is called before the first frame update
    void Start()
    {
        targets = gameObject.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach(var t in targets.Where((x) => x != null))
        {
            var x = Mathf.Clamp(t.position.x, worldLowerBound.x, worldHigherBound.x);
            var y = Mathf.Clamp(t.position.y, worldLowerBound.y, worldHigherBound.y);
            var z = Mathf.Clamp(t.position.z, worldLowerBound.z, worldHigherBound.z);
            t.position = new Vector3(x, y, z);
        }
    }
}
