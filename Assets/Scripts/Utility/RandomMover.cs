using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Простой скрипт для передвижения обьекта по определенной оси в случайном направлении за заданные интервалы времени
/// </summary>
public class RandomMover : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    Vector3 direction;

    float randomModifier = 0;

    [SerializeField]
    [Range(0, 5)]
    float minInterval = 0.5f;

    [SerializeField]
    [Range(0, 5)]
    float randomAddedIntervalMax = 0.5f; 



    private float timer = 0;
    private float maxTimer = 1f;


    // Start is called before the first frame update
    void Start()
    {
        if (minInterval < 0) minInterval = 0;
        if (randomAddedIntervalMax < 0) randomAddedIntervalMax = 0;

        maxTimer = minInterval + Random.Range(0, randomAddedIntervalMax);
        randomModifier = Random.Range(-1f, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        MoveObject();
        timer += Time.deltaTime;
        if (timer >= maxTimer)
        {
            timer = 0;
            randomModifier = Random.Range(-1f, 1f);
            maxTimer = minInterval + Random.Range(0, randomAddedIntervalMax);
        }
    }

    void MoveObject()
    {
        transform.position += randomModifier * direction * Time.deltaTime * speed;
    }

}
