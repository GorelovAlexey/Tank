using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Скрипт управляет турелью, стрельбой и прицеливанием
/// </summary>
public class WeaponController : MonoBehaviour
{
    public GameObject target;

    [SerializeField]
    AxisRotator gunRotator;

    [SerializeField]
    AxisRotator turretRotator;

    [SerializeField]
    AimSystem aimSystem;

    [SerializeField]
    GameObject gun;

    [SerializeField]
    LineRenderer laserRenderer;

    [SerializeField]
    float laserMaxDistance = 100;

    [SerializeField]
    private float laserFadeTime = 0.05f;

    private float timer = 0;
    private bool timerActive = false;


    [SerializeField]
    GameObject shotTrace;

    // Start is called before the first frame update
    void Start()
    {
        laserRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        LaserTimer();

        // Сообщаем системе прицеливания куда направлен ствол турели, чтобы она могла передвинуть курсор
        var gunDirection = new Ray(gun.transform.position, gun.transform.forward);
        aimSystem.SetRealAim(aimSystem.AimSpherePoint(gunDirection));
    }  

    /// <summary>
    /// Выстрел орудия
    /// </summary>
    /// <param name="shooter"> Обьект производящий выстрел </param>
    public void Shoot(GameObject shooter)
    {
        // Значение по дефолту - если не произошло попаданий
        var hitPosition = gun.transform.position + gun.transform.forward * laserMaxDistance;

        // Поиск попаданий - перебор всех обьектов с коллайдерами на линии
        var ray = new Ray(gun.transform.position, gun.transform.forward);
        var hits = Physics.RaycastAll(ray, laserMaxDistance);
        foreach(var h in hits)
        {
            // Лазер будет рисоваться до первой попавшейся цели
            hitPosition = h.point;
            var t = h.collider.gameObject.GetComponent<Target>();
            if (t != null)
            {
                t.GetHit(shooter, 10);
                break;
            }
        }

        //Отображение выстрела
        var trace = Instantiate(shotTrace).GetComponent<ShotTrace>();
        trace.start = transform.position;
        trace.end = hitPosition;
        trace.timer = laserFadeTime;
        trace.Reset();
 
    }

    // !! УДАЛИТЬ!!
    /// <summary>
    /// Таймер по истечении которого пропадает след выстрела
    /// </summary>
    void LaserTimer()
    {
        if (timerActive)
        {
            timer += Time.deltaTime;
            if (timer >= laserFadeTime)
            {
                timer = 0;
                timerActive = false;
                laserRenderer.enabled = false;
            }
        }
    }

    /// <summary>
    /// Запуск таймера для исчезновения выстрела
    /// </summary>
    void StartLaserTimer()
    {
        laserRenderer.enabled = true;
        timer = 0;
        timerActive = true;
    }

    /// <summary>
    /// Вычисление необходимого поворота турели и орудия, поворот турели и орудия
    /// </summary>
    /// <param name="target">Координаты цели в мире</param>
    public void Aim(Vector3 target)
    {
        // Вектор до цели от центра орудия
        var targetLook = target - transform.position;
        
        // Проецируем полученный вектор на плоскости - горизонтальную (по отношению к турели) и вертикальную 
        var horP = Vector3.ProjectOnPlane(targetLook, transform.up);
        var vertP = Vector3.ProjectOnPlane(targetLook, gun.transform.right);

        // Находим углы на которые нужно повернуть тело турели и поднять орудие
        var horA = Vector3.SignedAngle(horP, transform.forward, Vector3.up);
        var vertA = Vector3.SignedAngle(vertP, gun.transform.forward, gun.transform.parent.transform.right);
        
        // Вращатели сами повороачивают обьект с заданной скоростью пока не достигнут желаемого угла
        turretRotator.TargetAngle = -horA;
        gunRotator.TargetAngle = gunRotator.Angle - vertA;
    }
}
