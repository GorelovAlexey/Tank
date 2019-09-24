using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Скрипт вращает предмет (target) можно настраивать: желаемый угол, скорость вращения, ось вокруг которой вращает обьект,
/// максимальный и минимальный угол
/// </summary>
public class AxisRotator : MonoBehaviour
{
    [SerializeField]
    GameObject target;

    [SerializeField]
    Vector3 rotationAxis = new Vector3(1, 0, 0);

    [SerializeField]
    [Range(0.1f, 1000)]
    float rotationSpeed = 1f;

    [SerializeField]
    float maxAngle = 30;

    [SerializeField]
    float minAngle = -30;
       
    float _angle;
    public float Angle
    {
        get { return _angle; }
        set
        {
            value = Mathf.Clamp(value, minAngle, maxAngle);
            if (value != _angle)
            {
                var delta = value - _angle;
                target.transform.Rotate(rotationAxis, delta);
                _angle = value;
            }
        }
    }

    float _targetAngle;
    public float TargetAngle
    {
        get { return _targetAngle; }
        set
        {
            value = Mathf.Clamp(value, minAngle, maxAngle);
            _targetAngle = value;
        }
    }
    
    void Update()
    {
        if (Angle != TargetAngle)
        {
            var diff = TargetAngle - Angle;
            var step = rotationSpeed * Time.deltaTime;
            if (Mathf.Abs(diff) < step) Angle = TargetAngle;
            else if (Angle > TargetAngle) Angle -= step;
            else Angle += step;
        }
    }
    
    public void SetRotation(float input)
    {
        var zero = Zero();
        Angle = Mathf.Clamp(input, minAngle, maxAngle);
        Set(zero);
    }

    public void Rotate(float input)
    {
        var zero = Zero();
        Angle += input * Time.deltaTime * rotationSpeed;
        Angle = Mathf.Clamp(Angle, minAngle, maxAngle);
        Set(zero);
    }

    public void RotateTowards(float targetAngle)
    {
        var zero = Zero();

        var change = Time.deltaTime * rotationSpeed;
        if (Mathf.Abs(targetAngle - Angle) <= change) Angle = targetAngle;
        else if (targetAngle > Angle) Angle += change;
        else Angle -= change;
        Angle = Mathf.Clamp(Angle, minAngle, maxAngle);

        Set(zero);
    }

    private Quaternion Zero()
    {
        return target.transform.rotation * Quaternion.AngleAxis(-Angle, rotationAxis);
    }

    private void Set(Quaternion zero)
    {
        target.transform.rotation = zero * Quaternion.AngleAxis(Angle, rotationAxis);
    }
}
