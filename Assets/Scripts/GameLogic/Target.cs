using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт для цели стрельбы 
/// </summary>
public class Target : MonoBehaviour
{
    [SerializeField]
    [Range(1, 100)]
    private float maxHealth = 100;

    private float health;
    private Material material;
    

    // Start is called before the first frame update
    void Start()
    {
        material = gameObject.GetComponent<Renderer>().material;
        material.SetColor("_ColorTint", new Color(1, 1, 1));
        if (maxHealth < 0) maxHealth = 1;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void GetHit(GameObject hitter, float damage)
    {
        health -= damage;
        Debug.Log($" {health}!");
        if (health <= 0)
        {
            hitter.GetComponent<PlayerController>()?.AddScore();
            Destroy(gameObject, 0);
        }
        else
        {
            float healthPercent = health / maxHealth;
            material.SetColor("_ColorTint", new Color(1, healthPercent, healthPercent));
        }
    }

}
