using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс игрока, обрабатывает ввод игрока и поведение 
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    CameraController cameraController;

    [SerializeField]
    WeaponController weaponController;

    [SerializeField]
    AimSystem aimSystem;

    bool LockedCamMode = true;
    Text scoreUi;



    // Start is called before the first frame update
    void Start()
    {
        scoreUi = GameObject.Find("Score")?.GetComponent<Text>();
        Score = 0;
    }

    int _score;
    public int Score {
        get { return _score; }
        set
        {
            _score = value;
            if (scoreUi != null) scoreUi.text = $"Счет: {_score}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            LockedCamMode = false;
        }
        else LockedCamMode = true;


        if (LockedCamMode)
        {
            weaponController.Aim(aimSystem.AimSpherePoint(Camera.main.ScreenPointToRay(Input.mousePosition)));

            var h = Input.mousePosition.x / Screen.width;
            var v = Input.mousePosition.y / Screen.height;
            if (h < 0.25f) cameraController.RotationUpdate(-1);
            else if (h > 0.75f) cameraController.RotationUpdate(1);
            if (v < 0.2f) cameraController.AngleUpdate(1);
            else if (v > 0.8f) cameraController.AngleUpdate(-1);
        }
        else
        {
            cameraController.AngleUpdate(Input.GetAxis("Mouse Y"));
            cameraController.RotationUpdate(Input.GetAxis("Mouse X"));
        }

        UpdateAimingCursor(LockedCamMode);

        cameraController.CameraDistanceUpdate(Input.mouseScrollDelta.y);

        if (Input.GetMouseButtonDown(0)) weaponController.Shoot(gameObject);

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }


    void UpdateAimingCursor(bool lockedCam)
    {
        if (lockedCam)
        {
            aimSystem.TargetAimActive = true;
            aimSystem.SetTargetAim(Input.mousePosition);
        }
        else aimSystem.TargetAimActive = false;

        var h = Mathf.Abs(cameraController.HorizontalRotation % 360);
        if (h < 90 || h > 270) aimSystem.RealAimActive = true;
        else aimSystem.RealAimActive = false;      
    }

    public void AddScore()
    {
        Score++;
    }

}
