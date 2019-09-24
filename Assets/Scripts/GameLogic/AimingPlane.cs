using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingPlane : MonoBehaviour
{
    [SerializeField]
    SphereCollider aimingField;

    [SerializeField]
    public Camera targetCamera;

    public float Radius { get; private set; }

    Vector3 lastCameraPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        AdjustSphereRadius();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastCameraPosition != targetCamera.transform.position)
        {
            AdjustSphereRadius();
        }
    }

    void AdjustSphereRadius()
    {
        lastCameraPosition = targetCamera.transform.position;
        Radius = (aimingField.transform.position - targetCamera.transform.position).magnitude + 150;
        aimingField.radius = Radius;
    }

}
