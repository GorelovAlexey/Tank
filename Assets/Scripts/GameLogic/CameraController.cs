using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт управляет положением, наклоном, вращением и отдалением камеры
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField]
    [Range(0.001f, 100)]
    float zoomingSpeed = 0.1f;

    [SerializeField]
    [Range(0.1f, 1000)]
    float minCamDistance = 5;

    [SerializeField]
    [Range(0.1f, 1000)]
    float maxCamDistance = 10;

    [SerializeField]
    [Range(float.Epsilon, 50)]
    float horisontalMouseSensetivity = 1f;

    [SerializeField]
    [Range(float.Epsilon, 50)]
    float verticalMouseSensetivity = 0.5f;

    [SerializeField]
    [Range(0, 90)]
    float maxAngle = 50;

    [SerializeField]
    public Camera Camera { get; private set; }


    // Расстояние от камеры до якоря
    float _cameraDistance;
    public float CameraDistance
    {
        get { return _cameraDistance;  }
        set
        {
            if (_cameraDistance != value)
            {
                if (value > maxCamDistance) value = maxCamDistance;
                else if (value < minCamDistance) value = minCamDistance;

                var direction = (Camera.transform.position - Pivot.position).normalized;
                Camera.transform.position = Pivot.position + direction * value;
                _cameraDistance = value;
            }
        }
    }


    // Угол, вращение вокруг якоря
    float _horizontalRotation;
    public float HorizontalRotation
    {
        get { return _horizontalRotation; }
        set
        {
            if (value != _horizontalRotation)
            {
                var delta = value - _horizontalRotation;
                Pivot.Rotate(Vector3.up, delta, Space.World);
                _horizontalRotation = value;
            }
        }
    }

    // Угол наклона к якорю 
    float _verticalAngle;
    public float VerticalAngle
    {
        get { return _verticalAngle; }
        set
        {
            var nextAngle = value;
            nextAngle = Mathf.Clamp(nextAngle, -maxAngle, maxAngle);
            if (nextAngle != _verticalAngle)
            {
                var delta = nextAngle - _verticalAngle;
                Pivot.Rotate(Vector3.right, delta, Space.Self);
                _verticalAngle = nextAngle;
            }
        }
    }


    // Якорь 
    public Transform Pivot {
        get { return transform; }
    }


    public Transform Following;
    
    public Vector3 followingOffset;


    // Start is called before the first frame update
    void Start()
    {
        followingOffset = Pivot.position - Following.transform.position;

        if (minCamDistance <= 0) minCamDistance = 0.001f;
        if (maxCamDistance < minCamDistance) maxCamDistance = minCamDistance;

        Camera = gameObject.GetComponentInChildren<Camera>();
        _cameraDistance = CalculateCameraDistance();
    }


    // Update is called once per frame
    void Update()
    {
        if (Following != null) Pivot.position = Following.position + followingOffset;
    }

    // Методы контроля расстояния камеры от якоря (pivot)
    public void CameraDistanceUpdate(float wheelInput)
    {
        // Колесико мыши вверх = приближение, чтобы приблизть - уменьшаем дистанцию 
        CameraDistance -= wheelInput * Time.deltaTime * zoomingSpeed;
    }

    float CalculateCameraDistance()
    {
        return (Pivot.position - Camera.transform.position).magnitude;
    }


    public void RotationUpdate(float horizontalInput)
    {
        HorizontalRotation += horizontalInput * Time.deltaTime * horisontalMouseSensetivity;
    }

    public void AngleUpdate(float verticalInput)
    {
        VerticalAngle += verticalInput * Time.deltaTime * verticalMouseSensetivity;
    }
}
