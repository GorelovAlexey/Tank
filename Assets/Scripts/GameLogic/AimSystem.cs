using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class AimSystem : MonoBehaviour
{
    [SerializeField]
    RectTransform RealAim;

    [SerializeField]
    RectTransform TargetAim;

    [SerializeField]
    AimingPlane aimingPlane;

    public Color AimMovingColor;
    public Color AimSetColor;

    [SerializeField]
    public float treshold = 0.1f;

    public bool RealAimActive
    {
        get { return RealAim.gameObject.activeSelf; }
        set { RealAim.gameObject.SetActive(value); }
    }

    public bool TargetAimActive
    {
        get { return TargetAim.gameObject.activeSelf; }
        set { TargetAim.gameObject.SetActive(value); }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTargetAim(Vector3 screenPos)
    {
        if (TargetAimActive)
        {
            TargetAim.position = screenPos;
        }

    }


    public void SetRealAim(Vector3 worldPos)
    {
        if (RealAimActive)
        {
            var screenPos = Camera.main.WorldToScreenPoint(worldPos);
            RealAim.position = new Vector3(screenPos.x, screenPos.y, 0);
            var dist = (RealAim.position - TargetAim.position).magnitude;

            if (dist < treshold) RealAim.GetComponent<Image>().color = AimSetColor;
            else RealAim.GetComponent<Image>().color = AimMovingColor;
        }
    }

    public Vector3 AimSpherePoint(Ray ray)
    {
        ray.origin = ray.GetPoint(0);

        // Поиск попаданий по прямому ходу луча
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.PositiveInfinity))
            return hitInfo.point;

        ray.origin = ray.GetPoint(aimingPlane.Radius * 2);
        ray.direction = -ray.direction;

        // обратный ход луча - поиск только обьекта на слое "AimingLayer"
        Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, 1 << LayerMask.NameToLayer("AimingLayer"));
        return hitInfo.point;
    }
}



